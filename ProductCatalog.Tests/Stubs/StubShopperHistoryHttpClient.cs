using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductCatalog.Api.Domain.HttpClients;
using ProductCatalog.Api.Domain.Product;

namespace ProductCatalog.Tests.Stubs
{
    public class StubShopperHistoryHttpClient : IShopperHistoryHttpClient
    {
        public List<ShopperHistory> ShopperHistories { get; set; }
        
        public static StubShopperHistoryHttpClient WithHistory(List<ShopperHistory> shopperHistories)
        {
            return new StubShopperHistoryHttpClient
            {
                ShopperHistories = shopperHistories
            };
        }
        
        public Task<IEnumerable<ShopperHistory>> GetShopperHistory()
        {
            return Task.FromResult(ShopperHistories.AsEnumerable());
        }

        public static IShopperHistoryHttpClient WithNoHistory()
        {
            return new StubShopperHistoryHttpClient();
        }
    }
}