using ProcessFlow.SqlCommon;
using System.Windows;
using System.Windows.Controls;
using System.Threading.Tasks;
using System;

namespace DatabaseExplorer
{
	/// <summary>
	/// Interaction logic for ConnectionDetail.xaml
	/// </summary>
	public partial class ConnectionDetail : UserControl
	{
		public ConnectionDetail()
		{
			InitializeComponent();
		}

		private ServerExplorer serverExplorer;

		private void authenticationComboBox_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			//if (authenticationComboBox.IsEnabled)
			//	authenticationComboBox.SelectedIndex = 0;
			//else
			//	authenticationComboBox.SelectedIndex = -1;
		}

		private async void connectButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (serverExplorer != null)
				{
					//await Task.Run(() => serverExplorer.Connect());
					//Dispatcher.Invoke(() => serverExplorer.Connect());
					await Task.Run(() => Dispatcher.Invoke(() => serverExplorer.Connect()));
					SaveConnections();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void connections_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ServerConnection sc = connections.SelectedItem as ServerConnection;
			if (sc != null)
			{
				serverExplorer.ServerConnection = sc;
				SaveConnections();
			}
		}

		private void removeAllConnections_Click(object sender, RoutedEventArgs e)
		{
			serverExplorer.Connections.Clear();
			SaveConnections();
		}

		private void removeConnection_Click(object sender, RoutedEventArgs e)
		{
			ServerConnection sc = connections.SelectedItem as ServerConnection;
			if (sc != null)
			{
				serverExplorer.Connections.Remove(sc);
				SaveConnections();
			}
		}

		private void SaveConnections()
		{
			Properties.Settings.Default.Connections = serverExplorer.SerializeConnections();
		}

		private void userControl_Loaded(object sender, RoutedEventArgs e)
		{
			serverExplorer = this.DataContext as ServerExplorer;
			string s = Properties.Settings.Default.Connections;
			if (serverExplorer != null)
			{
				serverExplorer.DeserializeConnections(s);
			}
		}
	}
}