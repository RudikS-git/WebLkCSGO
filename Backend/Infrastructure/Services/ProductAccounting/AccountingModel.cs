using Domain.Entities.Product;

namespace Infrastructure.Services
{
    public abstract class AccountingModel
    {
        abstract public decimal CalculateFinallyPrice(Product product);

        protected decimal GetDiscount(Product typePrivilege)
        {
            if (typePrivilege.Discount != null && typePrivilege.Discount.Percent != 0.0)
            {
                return typePrivilege.Price * (decimal)(1 - typePrivilege.Discount.Percent);
            }

            return typePrivilege.Price;
        }

        protected decimal GetDiscount(decimal price, double percent)
        {
            if (percent > 0)
            {
                return price * (decimal)(1 - percent);
            }

            return price;
        }
    }
}
