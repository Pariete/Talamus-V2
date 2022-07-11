using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Talamus_V2.Net6.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public long UserId { get; set; }
        public string? Username { get; set; }
        public List<Saving> Savings { get; set; }

    }
}
