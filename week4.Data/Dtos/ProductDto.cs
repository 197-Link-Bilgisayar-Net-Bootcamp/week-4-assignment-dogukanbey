using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace week4.Service.Dtos
{
    public class ProductDto
    {
       
        public string Name { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }

        public string Supplier { get; set; }
        public decimal Discount { get; set; }
     


    }
}
