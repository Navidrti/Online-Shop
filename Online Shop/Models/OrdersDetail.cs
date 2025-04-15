namespace Online_Shop.Models
{
    public class OrdersDetail
    {
        public int id { get; set; }
        public string productName { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public int Count { get; set; }
        public int Price { get; set; }
        public int totalPrice { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public byte[]? img { get; set; }
        public Orders orders { get; set; }
    }
}
