using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Shop.Models
{
    public class Products
    {
        public int id { get; set; }
        public string? Name { get; set; }
        public int Price { get; set; }
        public byte[]? img { get; set; }
        public string description { get; set; }
        public ICollection<Variant> variant { get; set; }
        public ICollection<Cart> cart { get; set; }
        public ICollection<ProductImages> productImages { get; set; }
     
    }
}
