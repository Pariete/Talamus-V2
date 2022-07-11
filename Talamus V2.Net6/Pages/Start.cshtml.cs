using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Talamus_V2.Net6.Models;
using TinkoffPaymentClientApi;
using TinkoffPaymentClientApi.Commands;
using TinkoffPaymentClientApi.Models;


namespace Talamus_V2.Net6.Pages
{
    public class StartModel : PageModel
    {
        private readonly ILogger<StartModel> _logger;
        private readonly TalamusContext _context;


        public StartModel(ILogger<StartModel> logger, TalamusContext context)
        {
            _logger = logger;
            _context = context;
        }

        public bool withDialog { get; set; }
        public IList<Book> Books { get; set; } = default!;
        public List<Saving> Savings { get; set; } = default!;

        public string HelloMsg = "Welcome to Talamus";
        public long UserId { get; set; }

        public int LastPart = -1;


        public IActionResult OnPostBuyAsync(int bookId, long userId)
        {
            Book book = _context.Books.FirstOrDefault(Books => Books.Id == bookId);
            var clientApi = new TinkoffPaymentClient("1656491303444", "jz29cal5o6vly75c");
            CancellationToken cancellationToken = CancellationToken.None;

            Init init = new Init(Guid.NewGuid() + "", (uint)book.Price * 100)
            {
                Receipt = new Receipt(string.Empty, "example@example.com", TinkoffPaymentClientApi.Enums.ETaxation.UsnIncome,
               new List<ReceiptItem> {
                                  new ReceiptItem($"{userId}&{bookId}", 1, (uint)book.Price*100, TinkoffPaymentClientApi.Enums.ETax.None),
               }),
            }.SetEmail("example@example.com");

            var baseUrl = string.Format("{0}://{1}{2}", HttpContext.Request.Scheme, HttpContext.Request.Host, "/api/Payment");
            init.NotificationURL = baseUrl;

            string redirectUrl = "";
            try
            {
                var result = clientApi.InitAsync(init, cancellationToken).Result;
                Models.Payment payment = new Models.Payment()
                {
                    BookId = bookId,
                    UserId = userId,
                    Confirmed = false,
                    PaymentId = result.PaymentId
                };
                redirectUrl = result.PaymentURL;
                _context.Payments.Add(payment);
                _context.SaveChanges();
            }
            catch
            {

            }
            return Redirect(redirectUrl);
        }


        public PartialViewResult OnGetModalPartial()
        {
            // this handler returns _ModalPartial
            return new PartialViewResult
            {
                ViewName = "_ModalPartial"
            };
        }
        public async Task OnPostUserIdAsync(string usrId, string usrLogin)
        {
            withDialog = true;

            if (_context.Books != null)
            {
                Books = await _context.Books.ToListAsync();
            }
            UserId = Convert.ToInt64(usrId);
            if (!string.IsNullOrEmpty(usrLogin)) HelloMsg = $"Welcome, {usrLogin}";

            User user = _context.Users.Include(u => u.Savings).FirstOrDefaultAsync(u => u.UserId == UserId).Result;

            if (user != null)
            {
                Savings = user.Savings;
                if (user.Savings != null && user.Savings.Count() > 0)
                {
                    long timespan = long.MaxValue;
                    int index = 0;

                    for (int i = 0; i < user.Savings.Count(); i++)
                    {
                        if ((DateTime.Now.ToBinary() - user.Savings[i].Timestamp.ToBinary()) <= timespan)
                        {
                            timespan = DateTime.Now.ToBinary() - user.Savings[i].Timestamp.ToBinary();
                            index = i;
                        }
                    }
                    LastPart = user.Savings[index].PartId;
                }

                List<Models.Payment> acqs = _context.Payments.Where(a => a.UserId == UserId && a.Confirmed).ToList();

                foreach (Models.Payment acq in acqs)
                {
                    for (int i = 0; i < Books.Count; i++)
                    {
                        if (Books[i].Id == acq.BookId) Books[i].Price = 0;
                    }
                }
            }
            else
            {
                user = new User() { UserId = UserId, Username = usrLogin };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
        }
        public async Task OnGetUserIdAsync(string usrId, string usrLogin)
        {
            if (_context.Books != null)
            {
                Books = await _context.Books.ToListAsync();
            }
            UserId = Convert.ToInt64(usrId);
            if (!string.IsNullOrEmpty(usrLogin)) HelloMsg = $"Welcome, {usrLogin}";

            User user = _context.Users.Include(u => u.Savings).FirstOrDefaultAsync(u => u.UserId == UserId).Result;

            if (user != null)
            {
                Savings = user.Savings;
                if (user.Savings != null && user.Savings.Count() > 0)
                {
                    long timespan = long.MaxValue;
                    int index = 0;

                    for (int i = 0; i < user.Savings.Count(); i++)
                    {
                        if ((DateTime.Now.ToBinary() - user.Savings[i].Timestamp.ToBinary()) <= timespan)
                        {
                            timespan = DateTime.Now.ToBinary() - user.Savings[i].Timestamp.ToBinary();
                            index = i;
                        }
                    }
                    LastPart = user.Savings[index].PartId;
                }

                List<Models.Payment> acqs = _context.Payments.Where(a => a.UserId == UserId && a.Confirmed).ToList();

                foreach (Models.Payment acq in acqs)
                {
                    for (int i = 0; i < Books.Count; i++)
                    {
                        if (Books[i].Id == acq.BookId) Books[i].Price = 0;
                    }
                }
            }
            else
            {
                user = new User() { UserId = UserId, Username = usrLogin };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }


        }
    }
}