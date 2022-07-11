using System;
using System.ComponentModel.DataAnnotations;

namespace Talamus_V2.Net6.Models
{
    public class Part
    {
        [Key]
        public int Id { get; set; }
        public int BookId { get; set; }
        public Book? Book { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        [Required]
        public int PageNumber { get; set; }
        public DateTime? Created { get; set; } = DateTime.Now;

    }
}
