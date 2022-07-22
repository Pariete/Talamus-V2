using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talamus_V2.Net6.Models;

namespace Talamus_V2.Net6.Pages
{
    public class PartModel : PageModel
    {
        public PartModel(ILogger<StartModel> logger, TalamusContext context)
        {
            _logger = logger;
            _context = context;
        }

        public string Title = "Welcome to Part page";
        private readonly ILogger<StartModel> _logger;
        private readonly TalamusContext _context;

        public Part Part = null;
        public List<Part> SubsequentParts = null;

        public long UserId { get; set; }
        private async Task Save(int partId, long userId)
        {
            User user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            Part part = _context.Parts.Include(b => b.Book).FirstOrDefault(p => p.Id == partId);

            Saving save = _context.Savings.FirstOrDefault(s => s.UserId == user.Id && s.BookId == part.Book.Id);

            if (save == null)
            {
                save = new Saving() { User = user, BookId = part.Book.Id, PartId = part.Id, Timestamp = DateTime.Now };
                _context.Savings.Add(save);
            }
            else
            {
                save.PartId = part.Id;
                save.Timestamp = DateTime.Now;
            }

            await _context.SaveChangesAsync();
        }
        public async Task OnPostReadAsync(int bookId, long userId)
        {
            Part part = _context.Parts.Include(b => b.Book).FirstOrDefault(p => p.Book.Id == bookId && p.PageNumber == 1);
            await OnPostAsync(part.Id, userId);
        }
        
        public async Task OnPostAsync(int partId, long userId)
        {
            Part = _context.Parts.FirstOrDefault(p => p.Id == partId);
            UserId = userId;
            if (partId != 0)
            {
                await Save(partId, userId);
            }


            try
            {
                Part = _context.Parts.FirstOrDefault(i => i.Id == partId);
                SubsequentParts = new List<Part>();

                List<Subsequent> subsequents = _context.Subsequents.Include(s=>s.Part).Include(s=>s.NextPart).Where(s => s.Part.Id == partId).ToList();

                foreach (var subsequent in subsequents)
                {
                    SubsequentParts.Add(_context.Parts.FirstOrDefault(p => p.Id == subsequent.NextPart.Id));
                }

                if (SubsequentParts.Count() == 0)
                {
                    SubsequentParts = new List<Part>();
                    SubsequentParts.Add(new Part() { Title = "Конец" });
                    return;
                }
            }
            catch (Exception e)
            {
            }
        }

        public async Task OnPostByIdAsync(int id, long userId)
        {
            UserId = userId;

            await Save(id, userId);

            try
            {
                Part = _context.Parts.FirstOrDefault(i => i.Id == id);
                SubsequentParts = new List<Part>();

                List<Subsequent> subsequents = _context.Subsequents.Include(s=>s.Part).Include(s=>s.NextPart).Where(s => s.Part.Id == id).ToList();

                foreach (var subsequent in subsequents)
                {
                    SubsequentParts.Add(_context.Parts.FirstOrDefault(p => p.Id == subsequent.NextPart.Id));
                }

                if (SubsequentParts == null || SubsequentParts.Count() == 0)
                {
                    SubsequentParts = new List<Part>();
                    SubsequentParts.Add(new Part() { Title = "Конец" });
                    return;
                }
            }
            catch (Exception e)
            {
            }

        }
    }
}
