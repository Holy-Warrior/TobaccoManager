using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace TobaccoManager.Models
{
    public class QuotaAgreement
    {
        /// <summary>
        /// Represents a customer's tobacco quota agreement.<br/>
        /// Here are the parameters for creating a new quota agreement:<br/>
        /// 1. <c>customerId</c> (int): The ID of the customer this agreement applies to.<br/>
        /// 2. <c>maximumQuota</c> (decimal): The maximum allowed quota (in kg).<br/>
        /// 3. <c>startDate</c> (DateOnly): The start date of the agreement.<br/>
        /// 4. <c>endDate</c> (DateOnly?): The optional end date of the agreement.<br/>
        /// 5. <c>isActive</c> (bool): Whether the agreement is currently active.<br/>
        /// 6. <c>notes</c> (string?): Optional notes about the agreement.<br/>
        /// </summary>
        /// <param name="customerId">ID of the customer this agreement applies to.</param>
        /// <param name="maximumQuota">Maximum allowed quota (in kg).</param>
        /// <param name="startDate">Agreement start date.</param>
        /// <param name="endDate">Optional agreement end date.</param>
        /// <param name="isActive">Whether the agreement is currently active.</param>
        /// <param name="notes">Optional notes about the agreement.</param>

        [SetsRequiredMembers]
        public QuotaAgreement(
            int customerId,
            decimal maximumQuota,
            DateOnly startDate,
            DateOnly? endDate = null,
            bool isActive = true,
            string? notes = null
            )
        {
            CustomerId = customerId;
            MaximumQuota = maximumQuota;
            StartDate = startDate;
            EndDate = endDate;
            IsActive = isActive;
            Notes = notes;
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
