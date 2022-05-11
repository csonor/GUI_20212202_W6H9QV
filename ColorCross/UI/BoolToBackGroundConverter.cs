using System;
using System.Collections.Generic;
using System.Windows.Data;
using System.Windows.Media;

namespace ColorCross.UI
{
	class BoolToBackGroundConverter : IValueConverter // ONLY if enough time
	{
		
		// VM => UI
		// (int) 205 => 2m 5cm (string)
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			bool val = (bool)value;
			if (!val)
				return Brushes.Transparent;

			return Brushes.Green;
		}

		// UI => VM
		// (string) 1m 85cm => 185 (int)
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
