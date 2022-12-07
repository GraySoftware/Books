﻿using System.ComponentModel.DataAnnotations;

namespace BooksWeb.Models
{
    public class Category
    {
        // data annotation - tells entity framework that when the table is created ID is a key column
        [Key] 
        public int Id { get; set; }
        // data annotation - tells entity framework that Name is a required property 
        [Required] 
        public string Name { get; set; }
        [Display(Name="Display Order")]
        [Range(1,100, ErrorMessage ="Display Order must be between 1 and 100")]
        public int DisplayOrder { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }
}
