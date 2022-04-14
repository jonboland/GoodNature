using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoodNature.Entities
{
    public class Content
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Title { get; set; }
        
        [Display(Name = Constants.HTMLContentDisplayName)]
        public string HTMLContent { get; set; }
        
        [Display(Name = Constants.VideoLinkDisplayName)]
        public string VideoLink { get; set; }
        
        public CategoryItem CategoryItem { get; set; }
        
        [NotMapped]
        public int CatItemId { get; set; }
        
        [NotMapped]
        public int CategoryId { get; set; }
    }
}
