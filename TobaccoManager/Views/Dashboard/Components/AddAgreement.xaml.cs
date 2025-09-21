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

using System.Globalization;
using TobaccoManager.Models;

namespace TobaccoManager.Views.Dashboard.Components
{
    public partial class AddAgreement : Window
    {
        public QuotaAgreement? NewAgreement { get; private set; }

        private int _customerId;
        public AddAgreement(int customerId)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(customerId);
            InitializeComponent();
            StartDatePicker.SelectedDate = DateTime.Today;
            _customerId = customerId;
            QuotaBox.Focus();
        }

        private void AddAgreement_Click(object sender, RoutedEventArgs e)
        {
            // Validate quota
            if (!decimal.TryParse(QuotaBox.Text.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out decimal quota) || quota <= 0)
            {
                MessageBox.Show("Enter a valid positive number for quota.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (StartDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Start date is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var startDate = DateOnly.FromDateTime(StartDatePicker.SelectedDate.Value);
            DateOnly? endDate = null;

            if (EndDatePicker.SelectedDate.HasValue)
                endDate = DateOnly.FromDateTime(EndDatePicker.SelectedDate.Value);

            if (endDate.HasValue && endDate <= startDate)
            {
                MessageBox.Show("End date must be after start date.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var notes = string.IsNullOrWhiteSpace(NotesBox.Text) ? null : NotesBox.Text.Trim();

            // Create agreement
            NewAgreement = new QuotaAgreement(
                    customerId: _customerId,
                    maximumQuota: quota,
                    startDate: startDate,
                    endDate: endDate,
                    isActive: true,
                    notes: notes
                );

            // db connection
            using var db = new TobaccoManager.Contexts.AppDbContext();

            // checking for an existing agreement
            var existing = db.QuotaAgreements
                .FirstOrDefault(qa => qa.CustomerId == _customerId);

            if (existing != null)
            {
                MessageBox.Show(
                    $"An agreement already exists for customer ID {_customerId}.\n\nQuota: {existing.MaximumQuota}\nStart Date: {existing.StartDate}\nEnd Date: {existing.EndDate}\nNotes: {existing.Notes}", "Agreement Conflict",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                DialogResult = false;
                Close();
            }

            // Saving agreement
            db.QuotaAgreements.Add(NewAgreement);
            db.SaveChanges();

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
