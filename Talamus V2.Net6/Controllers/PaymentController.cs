using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Talamus_V2.Net6.Models;
using TinkoffPaymentClientApi.ResponseEntity;

namespace Talamus_V2.Net6.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class PaymentController : Controller
    {

        public PaymentController(TalamusContext context)
        {
            _context = context;
        }

        private readonly TalamusContext _context;


        [HttpPost]
        public async Task<IActionResult> New()
        {
            string t1 = "";
            string errors = "";
            try
            {
                StreamReader sr = new StreamReader(Request.Body);
                t1 = await sr.ReadToEndAsync();
            }
            catch (Exception e) { errors += e.Message + "\n"; }


            TinkoffNotification tn = new TinkoffNotification();

            try
            {
                tn = JsonConvert.DeserializeObject<TinkoffNotification>(t1);

                if (tn.Success == true)
                {
                    Payment payment = _context.Payments.FirstOrDefault(p => p.PaymentId == tn.PaymentId);
                    if (payment != null)
                    {
                        payment.Confirmed = true;
                        _context.SaveChanges();
                    }
                }

            }
            catch (Exception e)
            {
            }


            StreamWriter sw = new StreamWriter(Response.Body);
            Response.ContentType = "application/json";
            Response.ContentLength = 2;
            Response.StatusCode = 200;
            await sw.WriteAsync("OK");
            await sw.FlushAsync();

            return Ok();
        }
    }
}
