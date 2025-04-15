using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Shop.Models
{
    public class Cart
    {

        public int id { get; set; }
        public string? Name { get; set; }
        public string Size { get; set; }
        public int sizeId { get; set; }
        public string Color { get; set; }
        public int colorId { get; set; }
       
        public int Count { get; set; }
        public int price { get; set; }
        public byte[]? img { get; set; }
        public int totalPrice { get; set; }
        public string Userid { get; set; }
        public string UserName { get; set; }
        public int productsId { get; set; }
        [ForeignKey("productsId")]
        public Products products { get; set; }

    }
}
