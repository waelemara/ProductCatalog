using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using ProductCatalog.Api.Domain.HttpClients;

namespace ProductCatalog.Api.Domain.Product
{
    public class RecommendationsService
    {
        private readonly IShopperHistoryHttpClient _shopperHistoryHttpClient;

        public RecommendationsService(IShopperHistoryHttpClient shopperHistoryHttpClient)
        {
            _shopperHistoryHttpClient = shopperHistoryHttpClient;
        }
        public async Task<IEnumerable<Product>> Recommend(IEnumerable<Product> products)
        {
            var shopperHistory = await _shopperHistoryHttpClient.GetShopperHistory();

            var productsOrderedBasedOnNumberOfOrders = from shoppingHistory in shopperHistory
                let allOrders = shoppingHistory.Products
                from order in allOrders
                group order by order.Name into ordersGroupedByName
                let productsAndNumberOfOrders =new
                {
                    NumberOfOrders = ordersGroupedByName.Sum(product => product.Quantity),
                    Product = products.SingleOrDefault(product => product.Name == ordersGroupedByName.Key)
                }  
                orderby productsAndNumberOfOrders.NumberOfOrders descending 
                select productsAndNumberOfOrders.Product;
            
            var orderedProducts = productsOrderedBasedOnNumberOfOrders.ToList();
            var productsThatWereNotOrdered = products.Except(orderedProducts);
            orderedProducts.AddRange(productsThatWereNotOrdered);
            return orderedProducts;
        }
    }
}