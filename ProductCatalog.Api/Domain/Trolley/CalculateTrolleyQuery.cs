using System;
using System.Linq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ProductCatalog.Api.Domain.Trolley
{
    [BindProperties]
    public class CalculateTrolleyQuery
    {
        [JsonProperty] public List<Domain.Trolley.Product> Products { get; set; }
        [JsonProperty] public List<Special> Specials { get; set; }
        [JsonProperty] public List<Quantity> Quantities { get; set; }
    }

    public class CalculateTrolleyQueryHandler
    {
        public double Handle(CalculateTrolleyQuery query)
        {
            TrolleyGuard.AgainstDuplicatePurchasedQuantities(query);

            var (appliedSpecials, remainingPurchasedQuantities) = SubstituteQuantitiesWithSpecials(query.Specials, query.Quantities);
            return CalculateTotalPrice(query.Products, appliedSpecials, remainingPurchasedQuantities);
        }

        private static (List<Special> appliedSpecials, List<Quantity> quantitiesPurchased) SubstituteQuantitiesWithSpecials(
            List<Special> specialsToApply, List<Quantity> quantitiesPurchased)
        {
            var appliedSpecials = new List<Special>();
            foreach (var special in specialsToApply)
            {
                var specialApplied = true;
                do
                {
                    foreach (var portionQuantityFromSpecial in special.Quantities)
                    {
                        if (!DoesPortionOfSpecialApply(quantitiesPurchased, portionQuantityFromSpecial))
                        {
                            specialApplied = false;
                        }
                    }

                    if (specialApplied)
                    {
                        appliedSpecials.Add(special);
                        ReducePurchasedQuantitiesBySpecialQuantities(quantitiesPurchased, special);
                    }
                } while (specialApplied);
            }

            return (appliedSpecials, quantitiesPurchased);
        }

        private static double CalculateTotalPrice(List<Product> productsCatalog, List<Special> appliedSpecials, List<Quantity> quantitiesPurchased)
        {
            var totalSpecial = appliedSpecials.Sum(special => special.Total);
            var totalRemainingTotal = quantitiesPurchased.Sum(quantityPurchased =>
            {
                return productsCatalog.Single(product => product.Name == quantityPurchased.Name).Price * quantityPurchased.Value;
            });

            return totalSpecial + totalRemainingTotal;
        }

        private static void ReducePurchasedQuantitiesBySpecialQuantities(List<Quantity> quantitiesPurchased, Special special)
        {
            special.Quantities.ForEach(quantity =>
            {
                var foundPurchasedQuantity = quantitiesPurchased.Single(product => product.Name == quantity.Name);
                foundPurchasedQuantity.Value = foundPurchasedQuantity.Value - quantity.Value;
            });
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

    public static class TrolleyGuard
    {
        public static void AgainstDuplicatePurchasedQuantities(CalculateTrolleyQuery query)
        {
            if (query.Quantities.GroupBy(quantity => quantity.Name).Any(grouping => grouping.Count() > 1))
            {
                throw new ArgumentException("Duplicate purchased product name not allowed");
            }
        }
    }
}
