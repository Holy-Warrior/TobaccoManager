using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace TobaccoManager.Models
{
    public class QuotaAgreement
    {
        /// <summary>
        /// Represents a customer's tobacco quota agreement.
        /// </summary>
        /// <param name="customerId">ID of the customer this agreement applies to.</param>
        /// <param name="maximumQuota">Maximum allowed quota (in kg).</param>
        /// <param name="startDate">Agreement start date.</param>
        /// <param name="isActive">Whether the agreement is currently active.</param>

        [SetsRequiredMembers]
        public QuotaAgreement(
            int customerId,
            decimal maximumQuota,
            DateOnly startDate,
            bool isActive = true)
        {
            CustomerId = customerId;
            MaximumQuota = maximumQuota;
            StartDate = startDate;
            IsActive = isActive;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(Customer))]
        public int CustomerId { get; set; }

        public required decimal MaximumQuota { get; set; } // In kilograms

        public required DateOnly StartDate { get; set; }

        public DateOnly? EndDate { get; set; }

        public required bool IsActive { get; set; } = true;

        public string? Notes { get; set; }

        // Navigation property
        public Customer? Customer { get; set; }
    }
}
