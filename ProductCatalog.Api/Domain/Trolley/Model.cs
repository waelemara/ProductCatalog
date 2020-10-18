using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace ProductCatalog.Api.Domain.Trolley
{
    public class Product : ValueObject
    {
        public Product()
        {
        }
        public Product(string name, double price)
        {
            Name = name;
            Price = price;
        }
        
        [JsonProperty] public string Name { get; set; }
        [JsonProperty] public double Price { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
            yield return Price;
        }
    }
    
    public class Quantity : ValueObject
    {
        public Quantity()
        {
            
        }
        public Quantity(string name, double quantity)
        {
            Name = name;
            Value = quantity;
        }

        [JsonProperty] public string Name { get; set; }
        [JsonPropertyName("quantity")] public double Value { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
            yield return Value;
        }
    }
    
    public class Special : ValueObject
    {
        public Special()
        {
            
        }
        public Special(List<Quantity> quantities, double total)
        {
            Quantities = quantities;
            Total = total;
        }
        
        [JsonProperty] public List<Quantity> Quantities { get; set; }
        [JsonProperty] public double Total { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Quantities;
            yield return Total;
        }
    }
}