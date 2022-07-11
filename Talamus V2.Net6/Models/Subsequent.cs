using System.ComponentModel.DataAnnotations;

namespace Talamus_V2.Net6.Models
{
    public class Subsequent
    {
        [Key]
        public int Id { get; set; }
        public int PartId { get; set; }
        public int NextPartId { get; set; }
    }
}
