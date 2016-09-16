using ProcessFlow.SqlCommon;
using System.Windows.Controls;

namespace DatabaseExplorer
{
	/// <summary>
	/// Interaction logic for ProcedureDetail.xaml
	/// </summary>
	public partial class ProcedureDetail : UserControl
	{
		public ProcedureDetail()
		{
			InitializeComponent();
		}

		private void userControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			Procedure p = this.DataContext as Procedure;
			if (p != null)
				sqlTextBox.AppendText(p.Text);
		}
	}
}