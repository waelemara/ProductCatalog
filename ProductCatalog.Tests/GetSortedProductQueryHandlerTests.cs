using Xunit;
using FluentAssertions;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using ProductCatalog.Api.Domain.HttpClients;
using ProductCatalog.Api.Domain.Product;

namespace ProductCatalog.Tests
{
    public class GetSortedProductQueryHandlerTests
    {
        [Fact]
        public async Task ShouldReturnEmptyListOfProdcuts()
        {
            var getSortedProductQuery = new GetSortedProductQuery("Low");
            var getSortedProductQueryHandler = new GetSortedProductQueryHandler(StubProductsHttpClient.WithEmptyProducts());
            var getSortedProductQueryResponse = await getSortedProductQueryHandler.Handle(getSortedProductQuery);
            getSortedProductQueryResponse.Products.Should().BeEmpty();
        }


        [Fact]
        public async Task ShouldReturnSortedFromLowToHigh()
        {
            var getSortedProductQuery = new GetSortedProductQuery("Low");
            var getSortedProductQueryHandler = new GetSortedProductQueryHandler(StubProductsHttpClient.WithProducts(_sortedProductsFormLowToHigh));
            var getSortedProductQueryResponse = await getSortedProductQueryHandler.Handle(getSortedProductQuery);
            getSortedProductQueryResponse.Products.Should().Equal(_sortedProductsFormLowToHigh);
        }


        [Fact]
        public async Task ShouldReturnSortedFromHighToLow()
        {
            var getSortedProductQuery = new GetSortedProductQuery("High");
            var getSortedProductQueryHandler = new GetSortedProductQueryHandler(StubProductsHttpClient.WithProducts(_sortedProductsFormLowToHigh));
            var getSortedProductQueryResponse = await getSortedProductQueryHandler.Handle(getSortedProductQuery);
            getSortedProductQueryResponse.Products.Should().Equal(_sortedProductsFormHighToLow);
        }

        private readonly List<Product> _aNotSortedProductsFormLowToHigh = new List<Product>
        {
            new Product("Test Product A", 99.99, 0),
            new Product("Test Product B", 101.99, 0),
            new Product("Test Product C", 10.99, 0),
            new Product("Test Product D", 5, 0),
            new Product("Test Product F", 999999999999, 0),
        };



        private readonly List<Product> _sortedProductsFormLowToHigh = new List<Product>
        {
            new Product("Test Product D", 5, 0),
            new Product("Test Product C", 10.99, 0),
            new Product("Test Product A", 99.99, 0),
            new Product("Test Product B", 101.99, 0),
            new Product("Test Product F", 999999999999, 0),
        };

        private readonly List<Product> _sortedProductsFormHighToLow = new List<Product>
        {
            new Product("Test Product F", 999999999999, 0),
            new Product("Test Product B", 101.99, 0),
            new Product("Test Product A", 99.99, 0),
            new Product("Test Product C", 10.99, 0),
            new Product("Test Product D", 5, 0),
        };
    }

    public class StubProductsHttpClient : IProductHttpClient
    {
        private List<Product> ProductsToReturn { get; set; }

        public static StubProductsHttpClient WithProducts(List<Product> productsToReturn)
        {
            return new StubProductsHttpClient
            {
                ProductsToReturn = productsToReturn
            };
        }

        public static StubProductsHttpClient WithEmptyProducts()
        {
            return new StubProductsHttpClient
            {
                ProductsToReturn = new List<Product>()
            };
        }

        public Task<IEnumerable<Product>> GetProducts()
        {
            return Task.FromResult(ProductsToReturn.AsEnumerable());
        }
    }
}