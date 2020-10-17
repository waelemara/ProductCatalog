using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductCatalog.Api.Domain.HttpClients
{
    public interface IProductHttpClient
    {
        Task<IEnumerable<Product.Product>> GetProducts();
    }
}