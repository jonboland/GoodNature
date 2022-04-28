using GoodNature.Entities;
using System.Collections.Generic;

namespace GoodNature.Models
{
    public class CategoryDetailsModel
    {
        public IEnumerable<GroupedCategoryItemsModel> GroupedSelectedCategoryItemsModels { get; set; }
        public IEnumerable<GroupedCategoryItemsModel> GroupedActiveCategoryItemsModels { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}
