using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using TobaccoManager.Models;

namespace TobaccoManager.Views.Dashboard.Components
{
    /// <summary>
    /// Interaction logic for AddCustomers.xaml
    /// </summary>

    public partial class AddCustomers : Window
    {
        public Customer? NewCustomer { get; private set; }

        public AddCustomers()
        {
            InitializeComponent();
        }

        private void AddCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            // Validation: Name required, at least one of phone or address
            var name = NameBox.Text.Trim();
            var phone = PhoneBox.Text.Trim();
            var address = AddressBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Name is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(phone) && string.IsNullOrWhiteSpace(address))
            {
                MessageBox.Show("At least one of phone or address must be provided.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            // check phone is valid
            if (!string.IsNullOrWhiteSpace(phone) && !IsValidPhoneNumber(phone))
            {
                MessageBox.Show("Invalid phone number format.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            // Create new customer object
            NewCustomer = new Customer(
                    name = name,
                    phone = string.IsNullOrWhiteSpace(phone) ? null : phone,
                    address = string.IsNullOrWhiteSpace(address) ? null : address
                );

            try
            {
                using var db = new Contexts.AppDbContext();
                db.Customers.Add(NewCustomer);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to add customer: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                NewCustomer = null;
                return;
            }

            // check `AddAgreementCheckBox` is checked
            if (AddAgreementCheckBox.IsChecked == true)
            {
                var addAgreementWindow = new AddAgreement(NewCustomer.Id);
                var result = addAgreementWindow.ShowDialog();
                if (result == true)
                {
                    MessageBox.Show("Agreement added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Agreement addition cancelled or failed.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }

            this.DialogResult = true;
            this.Close();
        }

        private bool IsValidPhoneNumber(object phone)
        {
            var phoneStr = phone as string;
            if (string.IsNullOrWhiteSpace(phoneStr))
                return true; // Empty phone is considered valid here

            // Simple validation: allow digits, spaces, dashes, parentheses, and plus sign
            foreach (char c in phoneStr)
            {
                if (!char.IsDigit(c) && c != ' ' && c != '-' && c != '(' && c != ')' && c != '+')
                    return false;
            }
            return true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
