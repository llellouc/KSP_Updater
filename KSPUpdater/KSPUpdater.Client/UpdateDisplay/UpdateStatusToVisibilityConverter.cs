using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace KSPUpdater.Client.UpdateDisplay
{
    public class UpdateStatusToVisibilityConverter : IValueConverter
    {
        public object Convert(object valueObj, Type targetType, object parameterObj, CultureInfo culture)
        {
            if(valueObj == null)
                throw new ArgumentNullException(nameof(valueObj));
            if(parameterObj == null)
                throw new ArgumentNullException(nameof(parameterObj));

            var value = (UpdateStatus) valueObj;
            var parameter = Enum.Parse<UpdateStatus>((string) parameterObj);

            if(value == parameter)
                return Visibility.Visible;
            //else
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
