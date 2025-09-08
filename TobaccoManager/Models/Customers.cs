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
    }
}