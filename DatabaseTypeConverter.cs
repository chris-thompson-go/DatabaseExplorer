using System;
using System.Globalization;
using System.Windows.Data;

namespace DatabaseExplorer
{
	public class DatabaseTypeConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			//DatabaseType databaseType = value as DatabaseType;
			//if (databaseType != null)
			//	return databaseType.ToDescriptiveString();

			//return DatabaseTypeExtension.GetEnumValue(value.ToString());

			//if (value.GetType() == typeof(DatabaseType))
			//	return ((DatabaseType)value).ToDescriptiveString();

			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			//if (targetType == typeof(DatabaseType))
			//	return DatabaseTypeExtension.GetEnumValue(value.ToString());
			return value;
		}
	}
}