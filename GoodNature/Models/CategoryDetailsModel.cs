using GoodNature.Entities;
using System.Collections.Generic;

namespace GoodNature.Models
{
    public class CategoryDetailsModel
    {
        public IEnumerable<GroupedCategoryItemsModel> SelectedGroupedCategoryItemsModels { get; set; }
        public IEnumerable<GroupedCategoryItemsModel> ActiveGroupedCategoryItemsModels { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}
