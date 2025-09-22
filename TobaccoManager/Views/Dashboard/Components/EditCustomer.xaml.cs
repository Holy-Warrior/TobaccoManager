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

namespace TobaccoManager.Views.Dashboard.Components
{
    /// <summary>
    /// Provide parameter `customerId` to load and edit that customer.
    /// </summary>
    public partial class EditCustomer : Window
    {
        private int _customerId;
        private Models.Customer _customer;
        public EditCustomer(int customerId)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(customerId);
            InitializeComponent();
            _customerId = customerId;
            LoadCustomerData();
        }
        
        private void LoadCustomerData()
        {
            using var db = new Contexts.AppDbContext();
            _customer = db.Customers.FirstOrDefault(c => c.Id == _customerId)!;
            if (_customer != null)
            {
                // Populate the UI fields with customer data
                NameBox.Text = _customer.Name;
                PhoneBox.Text = _customer.Phone ?? string.Empty;
                AddressBox.Text = _customer.Address ?? string.Empty;

                // checking for an existing agreement
                var existing = db.QuotaAgreements
                    .FirstOrDefault(qa => qa.CustomerId == _customerId);

                if (existing != null)
                {
                    // do nothing
                }
                else
                {
                    AddAgreementButton.IsEnabled = true;
                }
            }
            else
            {
                MessageBox.Show("Customer not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }


        private void EditCustomerButton_Click(object sender, RoutedEventArgs e)
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

                // Update customer object
                _customer.Name = name;
            _customer.Phone = string.IsNullOrWhiteSpace(phone) ? null : phone;
            _customer.Address = string.IsNullOrWhiteSpace(address) ? null : address;

            // Save changes to database
            try
            {
                using var db = new Contexts.AppDbContext();
                db.Customers.Update(_customer);
                db.SaveChanges();
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to update customer: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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

        private void AddAgreementButton_Click(object sender, RoutedEventArgs e)
        {
            var addAgreementWindow = new AddAgreement(_customerId);
            var result = addAgreementWindow.ShowDialog();
            if (result == true)
            {
                MessageBox.Show("Agreement added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
