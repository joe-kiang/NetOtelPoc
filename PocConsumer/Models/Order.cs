﻿using System.Diagnostics;

namespace PocWorker.Models
{
    public class Order
    {
        public Guid OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string? OrderOrigin { get; set; }
        public string? OrderTraceId { get; set; }
    }
}