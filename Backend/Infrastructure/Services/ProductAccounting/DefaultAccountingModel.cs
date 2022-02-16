using Domain.Entities.Product;

namespace Infrastructure.Services
{
    public class DefaultAccountingModel : AccountingModel
    {
        public override decimal CalculateFinallyPrice(Product product)
        {
            return GetDiscount(product);
        }
    }
}
