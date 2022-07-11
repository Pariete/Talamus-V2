using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Talamus_V2.Net6.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public IndexModel(ILogger<IndexModel> logger, TalamusContext context)
        {
            _logger = logger;

        }

        public async Task OnGetAsync()
        {

        }
    }
}