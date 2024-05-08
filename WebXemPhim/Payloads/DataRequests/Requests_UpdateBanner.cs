﻿using System.ComponentModel.DataAnnotations;

namespace WebXemPhim.Payloads.DataRequests
{
    public class Requests_UpdateBanner
    {
        public int BannerId { get; set; }
        [DataType(DataType.Upload)]
        public IFormFile? ImageUrl { get; set; }
        public string Title { get; set; }
    }
}
