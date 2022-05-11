using System;
using System.Collections.Generic;
using System.Windows.Data;
using System.Windows.Media;

namespace ColorCross.UI
{
	class IntToColorConverter : IValueConverter // ONLY if enough time
	{
		public List<Color> Colors { get; set; }
		// VM => UI
		// (int) 205 => 2m 5cm (string)
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			int val = (int)value;
			if (val < 0)
				return Brushes.Transparent;

			return new SolidColorBrush(Colors[val]);
		}

		// UI => VM
		// (string) 1m 85cm => 185 (int)
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
