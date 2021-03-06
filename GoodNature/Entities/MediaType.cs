using GoodNature.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoodNature.Entities
{
    public class MediaType : IPrimaryProperties
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200, MinimumLength = 2)]
        [Display(Name = Constants.MediaTypeDisplayName)]
        public string Title { get; set; }
        
        [Required]
        [Display(Name = Constants.ThumbnailImagePathDisplayName)]
        public string ThumbnailImagePath { get; set; }
        
        [ForeignKey("MediaTypeId")]
        public virtual ICollection<CategoryItem> CategoryItems { get; set; }
    }
}
