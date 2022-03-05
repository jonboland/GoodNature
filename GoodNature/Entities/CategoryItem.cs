using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoodNature.Entities
{
    public class CategoryItem
    {
        private DateTime _releaseDate = DateTime.MinValue;
        public int Id { get; set; }
        [Required(ErrorMessage = Constants.TitleLengthErrorMessage)]
        [StringLength(200, MinimumLength = 2, ErrorMessage = Constants.TitleLengthErrorMessage)]
        public string Title { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        [Required(ErrorMessage = Constants.MissingMediaTypeErrorMessage)]
        [Display(Name = Constants.MediaTypeIdDisplayName)]
        public int MediaTypeId { get; set; }
        [NotMapped]
        public virtual ICollection<SelectListItem> MediaTypes { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = Constants.DateTimeItemReleasedDisplayName)]
        public DateTime DateTimeItemReleased 
        {
            get
            {
                return _releaseDate == DateTime.MinValue ? DateTime.Now : _releaseDate;
            }
            set
            {
                _releaseDate = value;
            }
        }
        [NotMapped]
        public int ContentId { get; set;}
    }   
}
