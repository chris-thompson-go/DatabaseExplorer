using ProcessFlow;
using ProcessFlow.SqlCommon;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace DatabaseExplorer
{
	public class ImageConverter : IValueConverter
	{
		public ImageConverter()
		{
		}

		public static BitmapImage GetImage(object value)
		{
			if (!initialized)
				Initialize();
			IElement e = value as IElement;

			if (e != null)
			{
				if (e.Name.In("Tables", "Procedures", "Views", "Columns", "Indexes"))
				{
					if (e.Status == "Expanded")
						return FolderOpenImage;
					else
						return FolderClosedImage;
				}
				if (e.Name.In("Connection", "Connect...", "Connections"))
					return ConnectImage;
				if (e.Name == "Databases")
					return DatabasesImage;
				if (e.Name == "Users")
					return UsersImage;
				if (e.Name == "Query")
					return QueryImage;
				if (e is ServerConnection)
					return ServerImage;
				if (e is IServer)
					return ServerImage;
				if (e is IDatabase)
					return DatabaseImage;
				if (e is Table)
					return TableImage;
				if (e is Procedure)
					return ProcedureImage;
				if (e is View)
					return ViewImage;
				if (e is Column)
					return TableImage;
				if (e is Index)
					return IndexImage;
				if (e is User)
					return UserImage;
			}
			return null;
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return GetImage(value);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		private static BitmapImage ConnectImage;
		private static BitmapImage DatabaseImage;
		private static BitmapImage DatabasesImage;
		private static BitmapImage FolderClosedImage;
		private static BitmapImage FolderImage;
		private static BitmapImage FolderOpenImage;
		private static BitmapImage IndexImage;
		private static bool initialized;
		private static BitmapImage ProcedureImage;
		private static BitmapImage QueryImage;
		private static BitmapImage ServerImage;
		private static BitmapImage TableImage;
		private static BitmapImage UserImage;
		private static BitmapImage UsersImage;
		private static BitmapImage ViewImage;

		private static void Initialize()
		{
			ConnectImage = new BitmapImage(new Uri("pack://application:,,,/Images/Connect.png"));
			DatabaseImage = new BitmapImage(new Uri("pack://application:,,,/Images/Database.png"));
			DatabasesImage = new BitmapImage(new Uri("pack://application:,,,/Images/Databases.png"));
			FolderImage = new BitmapImage(new Uri("pack://application:,,,/Images/Folder.png"));
			FolderClosedImage = new BitmapImage(new Uri("pack://application:,,,/Images/FolderClosed.png"));
			FolderOpenImage = new BitmapImage(new Uri("pack://application:,,,/Images/FolderOpen.png"));
			IndexImage = new BitmapImage(new Uri("pack://application:,,,/Images/Index.png"));
			ProcedureImage = new BitmapImage(new Uri("pack://application:,,,/Images/Procedure.png"));
			QueryImage = new BitmapImage(new Uri("pack://application:,,,/Images/Query.png"));
			ServerImage = new BitmapImage(new Uri("pack://application:,,,/Images/Server.png"));
			TableImage = new BitmapImage(new Uri("pack://application:,,,/Images/Table.png"));
			UserImage = new BitmapImage(new Uri("pack://application:,,,/Images/User.png"));
			UsersImage = new BitmapImage(new Uri("pack://application:,,,/Images/Users.png"));
			ViewImage = new BitmapImage(new Uri("pack://application:,,,/Images/View.png"));
			initialized = true;
		}
	}
}