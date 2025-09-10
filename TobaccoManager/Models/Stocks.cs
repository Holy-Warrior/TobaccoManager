using System.Diagnostics.CodeAnalysis;

namespace TobaccoManager.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public class Stock
    {
        /// <summary>
        /// Initializes a new Stock.
        /// </summary>
        /// <remarks>
        /// DatePaid is optional to allow for unpaid stocks.
        /// </remarks>
        /// <param name="price">Stock price (required).</param>
        /// <param name="dateReceived">Date the stock was received (required).</param>
        /// <param name="datePaid">Date the stock was paid (optional).</param>

        [SetsRequiredMembers]
        public Stock(
            decimal price,
            DateOnly dateReceived,
            DateOnly? datePaid = null)
        {
            Price = price;
            DateReceived = dateReceived;
            DatePaid = datePaid;
        }

        [System.ComponentModel.DataAnnotations.Key]
        [System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public decimal Price { get; set; }
        public DateOnly DateReceived { get; set; }
        public DateOnly? DatePaid { get; set; } // Nullable to allow unpaid stocks

        // Navigation property to Bundles
        public List<Bundle> Bundles { get; set; } = new List<Bundle>();

        // Auto-generated: Number of Bundles
        public int NoOfBundles => Bundles?.Count ?? 0;

        // Auto-generated: Grand Total Price (sum of all Bundle TotalPrices)
        public decimal GrandTotalPrice => Bundles?.Sum(b => b.TotalPrice) ?? 0m;
    }

    public class Bundle
    {
        /// <summary>
        /// Initializes a new Bundle.
        /// </summary>
        /// <param name="weight">Weight in kg (required).</param>
        /// <param name="pricePerKg">Price per kg (required).</param>
        /// <param name="leafGrade">Leaf grade (required).</param>
        
        [SetsRequiredMembers]
        public Bundle(
            decimal weight,
            decimal pricePerKg,
            Grade leafGrade)
        {
            Weight = weight;
            PricePerKg = pricePerKg;
            LeafGrade = leafGrade;
        }

        [System.ComponentModel.DataAnnotations.Key]
        [System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public decimal Weight { get; set; } // Weight in kg
        public decimal PricePerKg { get; set; }
        public Grade LeafGrade { get; set; }

        // Auto-generated: Total Price for this bundle
        public decimal TotalPrice => Weight * PricePerKg;
    }

    public enum Grade
    {
        A, B, C, D, E
    }
}