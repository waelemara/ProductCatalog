using Xunit;
using FluentAssertions;
using System.Threading.Tasks;
using ProductCatalog.Api.Domain.Product;
using ProductCatalog.Tests.DataHelpers;
using ProductCatalog.Tests.Stubs;

namespace ProductCatalog.Tests
{
    public class GetSortedProductQueryHandlerTests
    {
        [Fact]
        public async Task ShouldReturnEmptyListOfProducts()
        {
            var getSortedProductQuery = new GetSortedProductQuery("Low");
            var getSortedProductQueryHandler = new GetSortedProductQueryHandler(
                StubProductsHttpClient.WithEmptyProducts(),
                new RecommendationsService(StubShopperHistoryHttpClient.WithNoHistory()));
            var getSortedProductQueryResponse = await getSortedProductQueryHandler.Handle(getSortedProductQuery);
            getSortedProductQueryResponse.Products.Should().BeEmpty();
        }


        [Fact]
        public async Task ShouldReturnSortedFromLowToHigh()
        {
            var getSortedProductQuery = new GetSortedProductQuery("Low");
            var getSortedProductQueryHandler = new GetSortedProductQueryHandler(
                StubProductsHttpClient.WithProducts(ListOfProduct.ANotSortedProductsFormLowToHigh),
                new RecommendationsService(StubShopperHistoryHttpClient.WithNoHistory()));
            var getSortedProductQueryResponse = await getSortedProductQueryHandler.Handle(getSortedProductQuery);
            getSortedProductQueryResponse.Products.Should().Equal(ListOfProduct.SortedProductsFormLowToHigh);
        }


        [Fact]
        public async Task ShouldReturnSortedFromHighToLow()
        {
            var getSortedProductQuery = new GetSortedProductQuery("High");
            var getSortedProductQueryHandler = new GetSortedProductQueryHandler(
                StubProductsHttpClient.WithProducts(ListOfProduct.ANotSortedProductsFormLowToHigh),
                new RecommendationsService(StubShopperHistoryHttpClient.WithNoHistory()));
            var getSortedProductQueryResponse = await getSortedProductQueryHandler.Handle(getSortedProductQuery);
            getSortedProductQueryResponse.Products.Should().Equal(ListOfProduct.SortedProductsFormHighToLow);
        }
        
        [Fact]
        public async Task ShouldReturnSortedAscending()
        {
            var getSortedProductQuery = new GetSortedProductQuery("Ascending");
            var getSortedProductQueryHandler = new GetSortedProductQueryHandler(
                StubProductsHttpClient.WithProducts(ListOfProduct.ANotSortedProductsFormLowToHigh),
                new RecommendationsService(StubShopperHistoryHttpClient.WithNoHistory()));
            var getSortedProductQueryResponse = await getSortedProductQueryHandler.Handle(getSortedProductQuery);
            getSortedProductQueryResponse.Products.Should().Equal(ListOfProduct.SortedAscending);
        }
        
        [Fact]
        public async Task ShouldReturnSortedDescending()
        {
            var getSortedProductQuery = new GetSortedProductQuery("Descending");
            var getSortedProductQueryHandler = new GetSortedProductQueryHandler(
                StubProductsHttpClient.WithProducts(ListOfProduct.ANotSortedProductsFormLowToHigh), 
                new RecommendationsService(StubShopperHistoryHttpClient.WithNoHistory()));
            var getSortedProductQueryResponse = await getSortedProductQueryHandler.Handle(getSortedProductQuery);
            getSortedProductQueryResponse.Products.Should().Equal(ListOfProduct.SortedDescending);
        }
        
        [Fact]
        public async Task ShouldReturnSortedRecommended()
        {
            var getSortedProductQuery = new GetSortedProductQuery("Recommended");
            var getSortedProductQueryHandler = new GetSortedProductQueryHandler(
                StubProductsHttpClient.WithProducts(ListOfProduct.ANotSortedProductsFormLowToHigh),
                new RecommendationsService(StubShopperHistoryHttpClient.WithHistory(ShopperHistoryData.History)));
            var getSortedProductQueryResponse = await getSortedProductQueryHandler.Handle(getSortedProductQuery);
            getSortedProductQueryResponse.Products.Should().Equal(ListOfProduct.SortedBasedOnRecommended);
        }
    }
}