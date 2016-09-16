using ProcessFlow.SqlCommon;
using System.Windows.Controls;

namespace DatabaseExplorer
{
	/// <summary>
	/// Interaction logic for ViewDetail.xaml
	/// </summary>
	public partial class ViewDetail : UserControl
	{
		public ViewDetail()
		{
			InitializeComponent();
		}

		private void userControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			View v = this.DataContext as View;
			if (v != null)
				sqlTextBox.AppendText(v.Text);
		}
	}
}