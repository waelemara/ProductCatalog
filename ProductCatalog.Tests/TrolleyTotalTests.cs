using System.Collections.Generic;
using Xunit;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using FluentAssertions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using ProductCatalog.Api.Domain.Trolley;

namespace ProductCatalog.Tests
{
    public class TrolleyTotalTests
    {
        [Fact]
        public void ShouldApplySpecialForOneProduct()
        {
            // Arrange
            var calculateTrolleyQuery = new CalculateTrolleyQuery
            {
                Products = new List<Product>
                {
                    new Product("Product 1", 14.5),
                    new Product("Product 2", 29.6)
                },
                Specials = new List<Special>
                {
                    new Special(new List<Quantity>
                    {
                        new Quantity("Product 1", 3)
                    }, 20)
                },
                Quantities = new List<Quantity>
                {
                    new Quantity("Product 1", 3)
                }
            };

            // Act
            var total = new CalculateTrolleyQueryHandler().Handle(calculateTrolleyQuery);

            // Assert
            total.Should().Be(20.0d);
        }

        [Fact]
        public void SpecialsAppliedForMultipleProducts()
        {
            // Arrange
            var calculateTrolleyQuery = new CalculateTrolleyQuery
            {
                Products = new List<Product>
                {
                    new Product("Product 1", 14.5),
                    new Product("Product 2", 29.6),
                    new Product("Product 3", 24)
                },
                Specials = new List<Special>
                {
                    new Special(new List<Quantity>
                    {
                        new Quantity("Product 1", 3),
                        new Quantity("Product 2", 3)
                    }, 20)
                },
                Quantities = new List<Quantity>
                {
                    new Quantity("Product 1", 4),
                    new Quantity("Product 2", 4)
                }
            };

            // Act
            var total = new CalculateTrolleyQueryHandler().Handle(calculateTrolleyQuery);

            // Assert
            total.Should().Be(64.1d);
        }

        [Fact]
        public void SpecialsAppliedForMultipleProductsAndProductPricesAppliedCorrectlyForItemsMoreThanOne()
        {
            // Arrange
            var calculateTrolleyQuery = new CalculateTrolleyQuery
            {
                Products = new List<Product>
                {
                    new Product("Product 1", 14.5),
                    new Product("Product 2", 29.6),
                    new Product("Product 3", 24)
                },
                Specials = new List<Special>
                {
                    new Special(new List<Quantity>
                    {
                        new Quantity("Product 1", 3),
                        new Quantity("Product 2", 3)
                    }, 20)
                },
                Quantities = new List<Quantity>
                {
                    new Quantity("Product 1", 5),
                    new Quantity("Product 2", 5)
                }
            };

            // Act
            var total = new CalculateTrolleyQueryHandler().Handle(calculateTrolleyQuery);

            // Assert
            total.Should().Be(108.2);
        }

        [Fact]
        public void test()
        {
            // Arrange
            var calculateTrolleyQuery = new CalculateTrolleyQuery
            {
                Products = new List<Product>
                {
                    new Product("Product 1", 14.5),
                    new Product("Product 2", 29.6),
                    new Product("Product 3", 24)
                },
                Specials = new List<Special>
                {
                    new Special(new List<Quantity>
                    {
                        new Quantity("Product 1", 3),
                        new Quantity("Product 2", 3)
                    }, 20)
                },
                Quantities = new List<Quantity>
                {
                    new Quantity("Product 1", 6),
                    new Quantity("Product 2", 6)
                }
            };

            // Act
            var total = new CalculateTrolleyQueryHandler().Handle(calculateTrolleyQuery);

            // Assert
            total.Should().Be(40.0d);
        }


        [Fact]
        public async Task SortEndpointIsConfiguredAndReturnsCorrectJsonResponseForRecommended()
        {
            // Arrange
            var httpClient = new WebApplicationFactory<ProductCatalog.Api.Startup>().Server.CreateClient();
            var requestContent =
                "{\"products\": [{\"name\": \"test\",\"price\": 100.0}],\"specials\": [{\"quantities\": [{\"name\": \"test\",\"quantity\": 2}],\"total\":150}],\"quantities\": [{\"name\": \"test\",\"quantity\": 2}]}";


            // Act
            var httpResponseMessage =
                await httpClient.PostAsync("/trolleyTotal", new StringContent(requestContent, Encoding.UTF8, "application/json"));

            // Assert
            var readAsStringAsync = await httpResponseMessage.Content.ReadAsStringAsync();
            httpResponseMessage.StatusCode.Should().Be(StatusCodes.Status200OK);
            readAsStringAsync.Should().Be("150");
        }
    }
}