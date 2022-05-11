using System;
using System.Windows.Data;
using System.Windows.Media;

namespace ColorCross.UI
{
	class BoolToBackGroundConverter : IValueConverter // ONLY if enough time
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			bool val = (bool)value;
			if (!val)
				return Brushes.Transparent;

			return Brushes.Green;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
