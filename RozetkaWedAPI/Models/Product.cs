namespace RozetkaWedAPI.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public Category Category { get; set; }
        public Brand Brand { get; set; }
    }
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
        public string? Description { get; set; }
    }
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
    }
    public class Buyer
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public int BuyerId { get; set; }
        public Buyer Buyer { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public Product Product { get; set; }
        public Order Order { get; set; }
    }
}