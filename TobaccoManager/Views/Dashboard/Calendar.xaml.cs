using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

using Microsoft.EntityFrameworkCore;
using TobaccoManager.Contexts;

namespace TobaccoManager.Views.Dashboard
{
    /// <summary>
    /// Interaction logic for Calendar.xaml
    /// </summary>
    public partial class Calendar : Page
    {
        /// <summary>
        /// Dates that have at least one stock or quota-agreement event, used by
        /// DateHasEventConverter to highlight days on the calendar.
        /// </summary>
        public static HashSet<DateOnly> EventDates { get; private set; } = new();

        private readonly Dictionary<DateOnly, List<string>> _eventsByDate = new();

        public Calendar()
        {
            InitializeComponent();
            LoadEvents();

            EventCalendar.SelectedDate = DateTime.Today;
            ShowEventsFor(DateOnly.FromDateTime(DateTime.Today));
        }

        private void LoadEvents()
        {
            _eventsByDate.Clear();

            try
            {
                using var db = new AppDbContext();

                foreach (var stock in db.Stocks.ToList())
                {
                    AddEvent(stock.DateReceived, $"Stock #{stock.Id} received (price: {stock.Price:0.##})");
                    if (stock.DatePaid.HasValue)
                        AddEvent(stock.DatePaid.Value, $"Stock #{stock.Id} paid (price: {stock.Price:0.##})");
                }

                foreach (var agreement in db.QuotaAgreements.Include(a => a.Customer).ToList())
                {
                    var customerName = agreement.Customer?.Name ?? $"Customer #{agreement.CustomerId}";
                    AddEvent(agreement.StartDate, $"Quota agreement with {customerName} starts (max {agreement.MaximumQuota:0.##}kg)");
                    if (agreement.EndDate.HasValue)
                        AddEvent(agreement.EndDate.Value, $"Quota agreement with {customerName} ends");
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Failed to load calendar events: {ex.Message}", "Database Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }

            EventDates = new HashSet<DateOnly>(_eventsByDate.Keys);
        }

        private void AddEvent(DateOnly date, string description)
        {
            if (!_eventsByDate.TryGetValue(date, out var list))
            {
                list = new List<string>();
                _eventsByDate[date] = list;
            }
            list.Add(description);
        }

        private void EventCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EventCalendar.SelectedDate is DateTime selected)
            {
                ShowEventsFor(DateOnly.FromDateTime(selected));
            }
        }

        private void ShowEventsFor(DateOnly date)
        {
            SelectedDateHeader.Text = date.ToString("dddd, MMMM d, yyyy");

            EventsList.ItemsSource = _eventsByDate.TryGetValue(date, out var events)
                ? events
                : new List<string> { "No events on this date." };
        }
    }
}
