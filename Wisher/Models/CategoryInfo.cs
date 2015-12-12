﻿namespace Wisher.Models
{
    public class CategoryInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public string PictureUrl { get; set; }
        public int Level { get; set; }
        public int EbayCategoryId { get; set; }
        public int EbayParrentCategoryId { get; set; }
    }
}