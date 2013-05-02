﻿namespace MicroStackOverflow.Services.Models
{
    public class SearchPostsBy
    {
        public int PostTypeId { get; set; }
        public string Body { get; set; }
        public string Tags { get; set; }
        public int StartRowNum { get; set; }
        public int EndRowNum { get; set; }
        public long PageNumber { get; set; } //PetaPoco
    }
}