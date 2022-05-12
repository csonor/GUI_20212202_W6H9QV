using System;
using System.Collections.Generic;
using System.Windows.Data;
using System.Windows.Media;

namespace ColorCross.UI
{
	class IntToColorTextConverter : IValueConverter // ONLY if enough time
	{
		public List<Color> Colors { get; set; }

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			int val = (int)value;
			if (val < 0)
				return Brushes.Black;
			if (Colors[val].R < 128 && Colors[val].G < 128 && Colors[val].B < 128) return Brushes.White;
			return Brushes.Black;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
