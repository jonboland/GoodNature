using System.Linq;

namespace GoodNature.Models
{
    public class GroupedCategoryItemsModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public IGrouping<int, CategoryItemDetailsModel> Items { get; set; }
    }
}
