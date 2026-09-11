using TobaccoManager.Models;

namespace TobaccoManager.Services
{
    /// <summary>
    /// Holds the currently logged-in user for the lifetime of the application.
    /// </summary>
    public static class Session
    {
        public static User? CurrentUser { get; set; }
    }
}
