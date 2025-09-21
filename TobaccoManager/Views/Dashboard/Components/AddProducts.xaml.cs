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
    /// Interaction logic for AddProducts.xaml
    /// </summary>
    public partial class AddProducts : Window
    {
        public Stock? NewStock { get; private set; }

        public AddProducts()
        {
            InitializeComponent();
        }

        private void AddStock_Click(object sender, RoutedEventArgs e)
        {
            // Validation: Price and DateReceived required
            if (!decimal.TryParse(PriceBox.Text.Trim(), out decimal price))
            {
                MessageBox.Show("Invalid price.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!DateOnly.TryParse(DateReceivedBox.Text.Trim(), out DateOnly dateReceived))
            {
                MessageBox.Show("Invalid date received.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            DateOnly? datePaid = null;
            if (!string.IsNullOrWhiteSpace(DatePaidBox.Text))
            {
                if (!DateOnly.TryParse(DatePaidBox.Text.Trim(), out var dp))
                {
                    MessageBox.Show("Invalid date paid.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                datePaid = dp;
            }

            // Create new stock object
            NewStock = new Stock(price, dateReceived, datePaid);

            // Signal that user submitted data
            this.DialogResult = true;
            this.Close();
        }
    }
}
