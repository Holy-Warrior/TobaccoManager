using System;
using System.Globalization;
using System.Windows.Data;

namespace TobaccoManager.Converters
{
    /// <summary>
    /// Used by the Calendar page to highlight days that have at least one
    /// stock or quota-agreement event, by checking the shared
    /// Views.Dashboard.Calendar.EventDates set populated when the page loads.
    /// </summary>
    public class DateHasEventConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dateTime)
            {
                return Views.Dashboard.Calendar.EventDates.Contains(DateOnly.FromDateTime(dateTime));
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
