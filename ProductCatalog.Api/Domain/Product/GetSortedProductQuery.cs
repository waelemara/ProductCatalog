using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductCatalog.Api.Domain.HttpClients;

namespace ProductCatalog.Api.Domain.Product
{
    public class GetSortedProductQuery
    {
        public string SortOption { get; }

        public GetSortedProductQuery(string sortOption)
        {
            SortOption = sortOption;
        }
    }

    public class GetSortedProductQueryHandler
    {
        private readonly IProductHttpClient _productHttpClient;

        public GetSortedProductQueryHandler(IProductHttpClient productHttpClient)
        {
            _productHttpClient = productHttpClient;
        }

        public async Task<GetSortedProductQueryResponse> Handle(GetSortedProductQuery getSortedProductQuery)
        {
            var products = await _productHttpClient.GetProducts();
            var productList = products.ToList();
            return getSortedProductQuery.SortOption switch
            {
                "Low" => new GetSortedProductQueryResponse(productList.OrderBy(product => product.Price)),
                "High" => new GetSortedProductQueryResponse(productList.OrderByDescending(product => product.Price)),
                "Ascending" => new GetSortedProductQueryResponse(productList.OrderBy(product => product.Name)),
                "Descending" => new GetSortedProductQueryResponse(productList.OrderByDescending(product => product.Name)),
                _ => new GetSortedProductQueryResponse(productList)
            };
        }
    }


    public class GetSortedProductQueryResponse
    {
        public GetSortedProductQueryResponse(IEnumerable<Product> products)
        {
            Products = products;
        }

        public IEnumerable<Product> Products { get; }
    }
}