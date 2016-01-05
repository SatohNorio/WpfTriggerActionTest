using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfTriggerActionTest
{
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			//listBox.SourceUpdated += (sender, e) => MessageBox.Show("SourceUpdated");
			//listBox.TargetUpdated += (sender, e) => MessageBox.Show("TargetUpdated");
			//this.scrollViewer.ScrollChanged += (sender, e) =>
			//{
			//	var lb = e.Source as ListBox;
			//	if (lb != null)
			//	{
			//		MessageBox.Show("ScrollChanged");
			//	}
			//};
		}
	}
}
