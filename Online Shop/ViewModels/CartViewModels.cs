using Microsoft.AspNetCore.Mvc;
using Online_Shop.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Shop.ViewModels
{
    public class CartViewModels
    {
        public int id { get; set; }
        public string? Name { get; set; }
        
        public int sizeId { get; set; }
        public int colorId { get; set; }
        [Range(1,3,ErrorMessage ="Maximum : 3")]
        public int count { get; set; }
        public int price { get; set; }
        public int totalPrice { get; set; }




    }
}
