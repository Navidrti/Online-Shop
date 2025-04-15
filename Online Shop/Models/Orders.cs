namespace Online_Shop.Models
{
    public class Orders
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string lastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        //public string productName { get; set; }
        //public string Size { get; set; }
        //public string Color { get; set; }
        //public int Count { get; set; }
        //public int Price { get; set; }
        //public int totalPrice { get; set; }
        //public string UserName { get; set; }
        public ICollection<OrdersDetail> ordersDetails { get; set; }
    }
}
