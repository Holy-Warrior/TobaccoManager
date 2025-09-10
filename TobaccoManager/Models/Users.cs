namespace TobaccoManager.Models
{
    public class User
    {
        /// <summary>
        /// Initializes a new User.
        /// </summary>
        /// <remarks>
        /// All parameters are required.
        /// </remarks>
        /// <param name="name">User name (required).</param>
        /// <param name="email">User email (required).</param>
        /// <param name="password">User password (required).</param>
        /// <param name="securityQuestion">User security question (required).</param>
        /// <param name="securityAnswer">User security answer (required).</param>

        public User(
            string name,
            string email,
            string password,
            string securityQuestion,
            string securityAnswer
            )
        {
            Name = name;
            Email = email;
            Password = password;
            SecurityQuestion = securityQuestion;
            SecurityAnswer = securityAnswer;
        }

        [System.ComponentModel.DataAnnotations.Key]
        [System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // Now auto-generated because of the 2 previous atrributes lines
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string SecurityQuestion { get; set; }
        public required string SecurityAnswer { get; set; }
    }
}