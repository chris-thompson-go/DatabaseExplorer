using ProcessFlow;
using ProcessFlow.SqlCommon;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace DatabaseExplorer
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		// TODO: Hide expander for empty nodes
		// TODO: Lazy loading
		// TODO: Call the refresh methods
		// TODO: async, status
		public MainWindow()
		{
			InitializeComponent();
		}

		private ConnectionDetail connectionDetail;

		private Dictionary<string, DatabaseDetail> databaseDetails = new Dictionary<string, DatabaseDetail>();

		private DatabasesDetail databasesDetail;

		private ProcedureDetail procedureDetail;

		private Dictionary<string, ProceduresDetail> proceduresDetails = new Dictionary<string, ProceduresDetail>();

		private QueryEditor queryEditor;
		private ServerDetail serverDetail;

		private ServerExplorer serverExplorer;

		private object serverLock = new object();

		private TableDetail tableDetail;

		private Dictionary<string, TablesDetail> tablesDetails = new Dictionary<string, TablesDetail>();

		private UserDetail userDetail;

		private UsersDetail usersDetail;

		private ViewDetail viewDetail;

		private Dictionary<string, ViewsDetail> viewsDetails = new Dictionary<string, ViewsDetail>();

		private Visual GetDescendantByType(Visual element, Type type)
		{
			if (element == null) return null;
			if (element.GetType() == type) return element;
			Visual foundElement = null;
			if (element is FrameworkElement)
				(element as FrameworkElement).ApplyTemplate();
			for (int i = 0;
				i < VisualTreeHelper.GetChildrenCount(element); i++)
			{
				Visual visual = VisualTreeHelper.GetChild(element, i) as Visual;
				foundElement = GetDescendantByType(visual, type);
				if (foundElement != null)
					break;
			}
			return foundElement;
		}

		private void RefreshImage(TreeViewItem tvi)
		{
			if (tvi != null)
			{
				Image i = GetDescendantByType(tvi, typeof(Image)) as Image;
				if (i != null)
					i.Source = ImageConverter.GetImage(treeView.Items[0]);
			}
		}

		private void serverExplorer_Connected(object sender, EventArgs e)
		{
			if (treeView.Items.Count > 0)
			{
				//TreeViewItem tvi = treeView.ItemContainerGenerator.ContainerFromItem(treeView.Items[0]) as TreeViewItem;
				//RefreshImage(tvi);
			}
			databaseDetails.Clear();
			databasesDetail = null;
			procedureDetail = null;
			proceduresDetails.Clear();
			serverDetail = null;
			tableDetail = null;
			tablesDetails.Clear();
			userDetail = null;
			usersDetail = null;
			viewDetail = null;
			viewsDetails.Clear();
	}

		private void treeView_Collapsed(object sender, RoutedEventArgs e)
		{
			TreeViewItem tvi = e.OriginalSource as TreeViewItem;
			if (tvi != null)
			{
				IElement element = tvi.DataContext as IElement;
				if (element != null)
					element.Status = "";
			}
		}

		private void treeView_Expanded(object sender, RoutedEventArgs e)
		{
			TreeViewItem tvi = e.OriginalSource as TreeViewItem;
			if (tvi != null)
			{
				IElement element = tvi.DataContext as IElement;
				if (element != null)
					element.Status = "Expanded";
			}
		}

		private void treeView_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			// TODO: This is not working for sub-nodes
			if (e.Key == System.Windows.Input.Key.Enter)
			{
				TreeViewItem tvi = treeView.ItemContainerGenerator.ContainerFromItem(treeView.SelectedItem) as TreeViewItem;
				if (tvi != null)
					tvi.IsExpanded = true;
			}
		}

		private void treeView_LayoutUpdated(object sender, EventArgs e)
		{
			if (treeView.Items.Count > 0)
			{
				TreeViewItem tvi = treeView.ItemContainerGenerator.ContainerFromItem(treeView.Items[0]) as TreeViewItem;
				if (tvi != null && tvi.Margin.Left == 0)
					tvi.Margin = new Thickness(-15, 0, 0, 0);
			}
		}

		private void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			// TODO: I can assign the panel and DataContext generically
			IElement node = e.NewValue as IElement;
			if (node != null)
			{
				if (node.Name == "Connections")
				{
					if (connectionDetail == null)
					{
						connectionDetail = new ConnectionDetail();
						connectionDetail.DataContext = serverExplorer;
					}
					DetailPanel.Content = connectionDetail;
				}
				else if (node.TypeName == "Server")
				{
					if (serverDetail == null)
					{
						serverDetail = new ServerDetail();
						serverDetail.DataContext = node;
					}
					DetailPanel.Content = serverDetail;
				}
				else if (node.TypeName == nameof(Query))
				{
					if (queryEditor == null)
					{
						queryEditor = new QueryEditor();
						queryEditor.DataContext = node;
					}
					DetailPanel.Content = queryEditor;
				}
				else if (node.Name == "Databases")
				{
					if (databasesDetail == null)
					{
						databasesDetail = new DatabasesDetail();
						databasesDetail.DataContext = node;
					}
					DetailPanel.Content = databasesDetail;
				}
				else if (node.Name == "Users")
				{
					if (usersDetail == null)
					{
						usersDetail = new UsersDetail();
						usersDetail.DataContext = node;
					}
					DetailPanel.Content = usersDetail;
				}
				else if (node.TypeName == "Database")
				{
					if (!databaseDetails.ContainsKey(node.Name))
					{
						DatabaseDetail dd = new DatabaseDetail();
						dd.DataContext = node;
						databaseDetails.Add(node.Name, dd);
					}
					DetailPanel.Content = databaseDetails[node.Name];
				}
				else if (node.Name == "Tables")
				{
					if (!tablesDetails.ContainsKey(node.Parent.Name))
					{
						TablesDetail td = new TablesDetail();
						td.DataContext = node;
						tablesDetails.Add(node.Parent.Name, td);
					}
					DetailPanel.Content = tablesDetails[node.Parent.Name];
				}
				else if (node.Name == "Procedures")
				{
					if (!proceduresDetails.ContainsKey(node.Parent.Name))
					{
						ProceduresDetail pd = new ProceduresDetail();
						pd.DataContext = node;
						proceduresDetails.Add(node.Parent.Name, pd);
					}
					DetailPanel.Content = proceduresDetails[node.Parent.Name];
				}
				else if (node.Name == "Views")
				{
					if (!viewsDetails.ContainsKey(node.Parent.Name))
					{
						ViewsDetail vd = new ViewsDetail();
						vd.DataContext = node;
						viewsDetails.Add(node.Parent.Name, vd);
					}
					DetailPanel.Content = viewsDetails[node.Parent.Name];
				}
				else if (node.TypeName == nameof(Table))
				{
					if (tableDetail == null)
						tableDetail = new TableDetail();
					Table t = node as Table;
					IDatabase d = (IDatabase)t.Parent;
					Task task = new Task(() => { Dispatcher.Invoke(() => tableDetail.DataContext = d.GetTable(node.Name)); });
					task.Start();
					DetailPanel.Content = tableDetail;
				}
				else if (node.TypeName == nameof(Procedure))
				{
					if (procedureDetail == null)
						procedureDetail = new ProcedureDetail();
					Procedure p = node as Procedure;
					IDatabase d = (IDatabase)p.Parent;
					Task task = new Task(() => { Dispatcher.Invoke(() => procedureDetail.DataContext = d.GetProcedure(node.Name)); });
					task.Start();
					DetailPanel.Content = procedureDetail;
				}
				else if (node.TypeName == nameof(View))
				{
					if (viewDetail == null)
						viewDetail = new ViewDetail();
					View v = node as View;
					IDatabase d = (IDatabase)v.Parent;
					Task task = new Task(() => { Dispatcher.Invoke(() => viewDetail.DataContext = d.GetView(node.Name)); });
					task.Start();
					DetailPanel.Content = viewDetail;
				}
				else if (node.TypeName == nameof(User))
				{
					if (userDetail == null)
						userDetail = new UserDetail();
					DetailPanel.Content = userDetail;
				}
			}
		}

		private void window_Loaded(object sender, RoutedEventArgs e)
		{
			serverExplorer = new ServerExplorer();
			serverExplorer.Connected += serverExplorer_Connected;
			treeView.ItemsSource = serverExplorer;
			BindingOperations.EnableCollectionSynchronization(serverExplorer, serverLock);

			//	Height = Properties.Settings.Default.WindowHeight;
			//	Width =
			//		= "{Binding WindowHeight, Source={x:Static Properties:Settings.Default}, Mode=TwoWay}"

			//Width = "{Binding WindowWidth, Source={x:Static Properties:Settings.Default}, Mode=TwoWay}"

			//Left = "{Binding WindowLeft, Source={x:Static Properties:Settings.Default}, Mode=TwoWay}"

			//Top = "{Binding WindowTop, Source={x:Static Properties:Settings.Default}, Mode=TwoWay}"
		}
	}
}