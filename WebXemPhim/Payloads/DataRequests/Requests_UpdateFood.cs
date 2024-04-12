﻿using System.ComponentModel.DataAnnotations;

namespace WebXemPhim.Payloads.DataRequests
{
    public class Requests_UpdateFood
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        [DataType(DataType.Upload)]
        public IFormFile Image { get; set; }
        public string NameOfFood { get; set; }
    }
}
