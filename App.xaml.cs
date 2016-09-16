using System.Windows;

namespace DatabaseExplorer
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private void Application_Exit(object sender, ExitEventArgs e)
		{
			DatabaseExplorer.Properties.Settings.Default.Save();
		}
	}
}