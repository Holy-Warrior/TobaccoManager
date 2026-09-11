using TobaccoManager.Models;

namespace TobaccoManager.Services
{
    /// <summary>
    /// Holds the currently logged-in user for the lifetime of the application.
    /// </summary>
    public static class Session
    {
        private static User? _currentUser;

        public static User? CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                CurrentUserChanged?.Invoke();
            }
        }

        /// <summary>
        /// Raised whenever CurrentUser is set, including edits made from the Profile page,
        /// so views showing the user's name (e.g. the dashboard sidebar) can refresh.
        /// </summary>
        public static event Action? CurrentUserChanged;
    }
}
