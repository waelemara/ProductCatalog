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
        public async Task ShouldReturnEmptyListOfProducts()
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
            var getSortedProductQueryHandler = new GetSortedProductQueryHandler(StubProductsHttpClient.WithProducts(ListOfProduct.ANotSortedProductsFormLowToHigh));
            var getSortedProductQueryResponse = await getSortedProductQueryHandler.Handle(getSortedProductQuery);
            getSortedProductQueryResponse.Products.Should().Equal(ListOfProduct.SortedProductsFormLowToHigh);
        }


        [Fact]
        public async Task ShouldReturnSortedFromHighToLow()
        {
            var getSortedProductQuery = new GetSortedProductQuery("High");
            var getSortedProductQueryHandler = new GetSortedProductQueryHandler(StubProductsHttpClient.WithProducts(ListOfProduct.ANotSortedProductsFormLowToHigh));
            var getSortedProductQueryResponse = await getSortedProductQueryHandler.Handle(getSortedProductQuery);
            getSortedProductQueryResponse.Products.Should().Equal(ListOfProduct.SortedProductsFormHighToLow);
        }
        
        [Fact]
        public async Task ShouldReturnSortedAscending()
        {
            var getSortedProductQuery = new GetSortedProductQuery("Ascending");
            var getSortedProductQueryHandler = new GetSortedProductQueryHandler(StubProductsHttpClient.WithProducts(ListOfProduct.ANotSortedProductsFormLowToHigh));
            var getSortedProductQueryResponse = await getSortedProductQueryHandler.Handle(getSortedProductQuery);
            getSortedProductQueryResponse.Products.Should().Equal(ListOfProduct.SortedAscending);
        }
        
        [Fact]
        public async Task ShouldReturnSortedDescending()
        {
            var getSortedProductQuery = new GetSortedProductQuery("Descending");
            var getSortedProductQueryHandler = new GetSortedProductQueryHandler(StubProductsHttpClient.WithProducts(ListOfProduct.ANotSortedProductsFormLowToHigh));
            var getSortedProductQueryResponse = await getSortedProductQueryHandler.Handle(getSortedProductQuery);
            getSortedProductQueryResponse.Products.Should().Equal(ListOfProduct.SortedDescending);
        }
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