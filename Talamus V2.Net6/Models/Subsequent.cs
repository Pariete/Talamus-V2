using System.ComponentModel.DataAnnotations;

namespace Talamus_V2.Net6.Models
{
    public class Subsequent
    {
        [Key]
        public int Id { get; set; }
        public Part Part { get; set; }
        public Part NextPart { get; set; }
    }
}
