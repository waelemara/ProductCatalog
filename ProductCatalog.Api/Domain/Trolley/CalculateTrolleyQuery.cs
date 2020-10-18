using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ProductCatalog.Api.Domain.Trolley
{
    [BindProperties]
    public class CalculateTrolleyQuery
    {
        [JsonProperty]
        public List<Domain.Trolley.Product> Products { get; set; }
        [JsonProperty]
        public List<Special> Specials { get; set; }
        [JsonProperty]
        public List<Quantity> Quantities { get; set; }
    }

    public class CalculateTrolleyQueryHandler
    {
        public double Handle(CalculateTrolleyQuery calculateTrolleyQuery)
        {
            var quantitiesPurchased = calculateTrolleyQuery.Quantities.ToList();
            var appliedSpecials = SubstituteQuantitiesWithSpecials(calculateTrolleyQuery.Specials, quantitiesPurchased);
            return CalculateTotalPrice(calculateTrolleyQuery, appliedSpecials, quantitiesPurchased);
        }

        private static double CalculateTotalPrice(CalculateTrolleyQuery calculateTrolleyQuery, List<Special> appliedSpecials, List<Quantity> quantitiesPurchased)
        {
            var totalSpecial = appliedSpecials.Sum(special => special.Total);
            var totalRemainingTotal = quantitiesPurchased.Sum(quantityPurchased =>
            {
                return calculateTrolleyQuery.Products.Single(product => product.Name == quantityPurchased.Name).Price * quantityPurchased.Value;
            });

            return totalSpecial + totalRemainingTotal;
        }

        private static List<Special> SubstituteQuantitiesWithSpecials(List<Special> specialsToApply, List<Quantity> quantitiesPurchased)
        {
            List<Special> appliedSpecials = new List<Special>();
            foreach (var special in specialsToApply)
            {
                bool specialApplied = true;
                do
                {
                    foreach (var portionQuantityFromSpecial in special.Quantities)
                    {
                        specialApplied = DoesPortionOfSpecialApply(quantitiesPurchased, portionQuantityFromSpecial);
                    }

                    if (specialApplied)
                    {
                        
                        appliedSpecials.Add(special);

                        special.Quantities.ForEach(quantity =>
                        {
                            var foundPurchasedQuantity = quantitiesPurchased.Single(product => product.Name == quantity.Name);
                            foundPurchasedQuantity.Value = foundPurchasedQuantity.Value - quantity.Value;
                        });
                    }
                } while (specialApplied);
            }

            return appliedSpecials;
        }

        private static bool DoesPortionOfSpecialApply(List<Quantity> quantitiesPurchased, Quantity quantity)
        {
            var foundPurchasedProduct = quantitiesPurchased.SingleOrDefault(product => product.Name == quantity.Name);
            if (foundPurchasedProduct == null) return false;

            if (foundPurchasedProduct.Value < quantity.Value)
            {
                return false;
            }

            return true;
        }
    }
}
