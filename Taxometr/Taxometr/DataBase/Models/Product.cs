using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProductDirectory_v4.Models
{
    public class Product : DataModel
    {
        public string Name { get; set; }
        public int Amount { get; set; } = 1;
        public string BarCode { get; set; }
        public string Model { get; set; }
        public string Sort { get; set; }
        public string Color { get; set; }
        public decimal Size { get; set; }
        public decimal Weight { get; set; }
        public int PriceId { get; set; }
        public DateTime DateChanged { get; set; }
    }
}
