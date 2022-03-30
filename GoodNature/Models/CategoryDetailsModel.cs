using GoodNature.Entities;
using System.Collections.Generic;

namespace GoodNature.Models
{
    public class CategoryDetailsModel
    {
        public IEnumerable<GroupedCategoryItemsModel> GroupedCategoryItemsModels { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}
