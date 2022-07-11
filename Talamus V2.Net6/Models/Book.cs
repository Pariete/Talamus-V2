using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Talamus_V2.Net6.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Author { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        // if from google drive, must be formatted like this:
        //https://drive.google.com/uc?export=view&id= ID of image
        public string ImageUrl { get; set; } = string.Empty;
        public long Price { get; set; } = 0;
        public List<Part> Parts { get; set; } = new List<Part>();

        public bool Hidden { get; set; } = false;

    }
}
