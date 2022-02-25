﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoodNature.Entities
{
    public class Category
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string ThumbnailImagePath { get; set; }
        [ForeignKey("CategoryId")]
        public virtual ICollection<CategoryItem> CategoryItems { get; set; }
        [ForeignKey("CategoryId")]
        public virtual ICollection<UserCategory> UserCategories { get; set; }
    }
}