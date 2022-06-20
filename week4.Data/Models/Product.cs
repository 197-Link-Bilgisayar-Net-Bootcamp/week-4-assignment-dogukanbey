 using System.ComponentModel.DataAnnotations;
 

namespace week4.Data.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
 
        public string Supplier { get; set; }
        public int Stock { get; set; }
        public decimal Discount { get; set; }
    
    
    }
}