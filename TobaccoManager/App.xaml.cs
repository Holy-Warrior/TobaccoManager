using System.Configuration;
using System.Data;
using System.Windows;
using TobaccoManager.Contexts;
using TobaccoManager.Models;

namespace TobaccoManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            using var context = new AppDbContext();
            if (context.Database.EnsureCreated())
            {
                var adminUser = new User(
                    name: "admin",
                    email: "admin@example.com",
                    password: "admin",
                    securityQuestion: "Default?",
                    securityAnswer: "Default"
                );
                context.Users.Add(adminUser);
                context.SaveChanges();
            }
        }
    }

}
