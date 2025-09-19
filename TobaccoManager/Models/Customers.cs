using System.Diagnostics.CodeAnalysis;

namespace TobaccoManager.Models
{
    public class Customer
    {
        /// <summary>
        /// Initializes a new Customer.
        /// </summary>
        /// <remarks>
        /// Make sure at least one of the:
        /// - phone, or
        /// - address
        /// is not null or whitespace.
        /// </remarks>
        /// <param name="name">Customer name (required).</param>
        /// <param name="phone">Customer phone (optional).</param>
        /// <param name="address">Customer address (optional).</param>

        [SetsRequiredMembers]
        public Customer(
            string name,
            string? phone = null,
            string? address = null)
        {
            Name = name;
            if (string.IsNullOrWhiteSpace(phone) && string.IsNullOrWhiteSpace(address))
                throw new ArgumentException("At least one of phone or address must be provided.");
            Phone = phone;
            Address = address;
        }

        [System.ComponentModel.DataAnnotations.Key]
        [System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public List<QuotaAgreement> QuotaAgreements { get; set; } = new();
        public List<Stock> Stocks { get; set; } = new();

        // Returns the latest or first quota agreement
        public QuotaAgreement? LatestAgreement => QuotaAgreements.FirstOrDefault();

        // Exposed for DataGrid Binding
        public string? AgreementIdDisplay => LatestAgreement?.Id.ToString() ?? "—";
        public string? QuotaDisplay => LatestAgreement?.MaximumQuota.ToString("0.##") ?? "—";
        public string? StartDateDisplay => LatestAgreement?.StartDate.ToString("yyyy-MM-dd") ?? "—";
        public string? EndDateDisplay => LatestAgreement?.EndDate?.ToString("yyyy-MM-dd") ?? "—";
        public string StatusDisplay => LatestAgreement?.IsActive == true ? "Active" : (LatestAgreement == null ? "—" : "Inactive");
        public bool HasAgreement => LatestAgreement != null;


    }
}