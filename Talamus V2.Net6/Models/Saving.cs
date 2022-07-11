using System;

namespace Talamus_V2.Net6.Models
{
    public class Saving
    {
        public int Id { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public int PartId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
