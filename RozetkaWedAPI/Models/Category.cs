﻿using RozetkaWedAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace RozetkaWebAPI.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public string? Description { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}