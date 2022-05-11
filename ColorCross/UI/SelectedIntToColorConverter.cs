using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ColorCross.UI
{
	class SelectedIntToColorConverter : IValueConverter , IMultiValueConverter // ONLY if enough time
	{

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			int val = (int)value;
			int self = (int)parameter;
			if (val != self)
				return Brushes.Transparent;

			return Brushes.Aqua;
		}
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
			int val = (int)values[0];
			int self = (int)values[1];
			if (val != self)
				return Brushes.Transparent;

			return Brushes.Aqua;
		}

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
