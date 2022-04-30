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
        private IDataFunctions _dataFunctions;

        public ContentController(IDataFunctions dataFunctions)
        {
            _dataFunctions = dataFunctions;
        }
        
        public async Task<IActionResult> Index(int categoryItemId)
        {
            Content content = await _dataFunctions.GetPieceOfContent(categoryItemId);

            return View(content);
        }
    }
}
