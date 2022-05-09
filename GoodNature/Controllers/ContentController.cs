using GoodNature.Data;
using GoodNature.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNature.Controllers
{
    public class ContentController : Controller
    {
        private ICustomDataMethods _customDataMethods;

        public ContentController(ICustomDataMethods customDataMethods)
        {
            _customDataMethods = customDataMethods;
        }
        
        public async Task<IActionResult> Index(int categoryItemId)
        {
            Content content = await _customDataMethods.GetPieceOfContent(categoryItemId);

            return View(content);
        }
    }
}
