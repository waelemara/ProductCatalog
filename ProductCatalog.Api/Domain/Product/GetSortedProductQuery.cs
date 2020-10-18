using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
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
        private readonly RecommendationsService _recommendationsService;


        public GetSortedProductQueryHandler(IProductHttpClient productHttpClient, RecommendationsService recommendationsService)
        {
            _productHttpClient = productHttpClient;
            _recommendationsService = recommendationsService;
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
                "Recommended" => new GetSortedProductQueryResponse(await _recommendationsService.Recommend(productList)),
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