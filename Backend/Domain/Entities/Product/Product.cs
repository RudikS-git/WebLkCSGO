using Domain.Purchase;

namespace Domain.Entities.Product
{
    public abstract class Product
    {
        public string Name { get; set; }
        
        public decimal Price { get; set; }

        public Discount Discount { get; set; }
    }
}
