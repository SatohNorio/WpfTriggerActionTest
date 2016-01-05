using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Threading;
using System.Threading;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Net.NetworkInformation;
using System.Diagnostics;
using System.Reflection;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace WpfTriggerActionTest
{
	public class MainWindowViewModel : ViewModel
	{
		// ------------------------------------------------------------------------------------------------------------
		#region コンストラクタ

		/// <summary>
		/// WpfTriggerActionTest.MainWindowViewModel クラスの新しいインスタンスを作成します。
		/// このコンストラクタはデザイン時、またはデバッグ時に使用します。
		/// </summary>
		public MainWindowViewModel()
		{
			this.CommandLogItems = new ObservableCollection<string>();
			this.LogItems = new ObservableCollection<string>();
			this.LogItems.CollectionChanged += (sender, e) =>
			{
				//var msg = "NewItems: " + e.NewItems.ToString() + ", NewStartingIndex: " + e.NewStartingIndex.ToString();
				//MessageBox.Show(msg);
			};
		}

		#endregion // コンストラクタ
		// ------------------------------------------------------------------------------------------------------------
		// ------------------------------------------------------------------------------------------------------------
		#region Binding 用プロパティ

		/// <summary>
		/// ログの一覧 を取得します。
		/// </summary>
		public ObservableCollection<string> LogItems { get; set; }

		private string FCommandText = "";

		/// <summary>
		/// 入力されたコマンドテキスト を取得または設定します。
		/// </summary>
		public string CommandText
		{
			get
			{
				return this.FCommandText;
			}
			set
			{
				if (this.FCommandText != value)
				{
					this.FCommandText = value;
					this.OnPropertyChanged("CommandText");
				}
			}
		}

		/// <summary>
		/// 入力されたコマンド履歴 を取得します。
		/// </summary>
		public ObservableCollection<string> CommandLogItems { get; private set; }

		#endregion // Binding 用プロパティ
		// ------------------------------------------------------------------------------------------------------------
		// ------------------------------------------------------------------------------------------------------------
		#region StringExecute コマンド

		/// <summary>
		/// 文字列で実行するコマンド を管理します。
		/// </summary>
		private ICommand FStringExecuteCommand;

		/// <summary>
		/// 文字列で実行するコマンド を取得します。
		/// </summary>
		public ICommand StringExecuteCommand
		{
			get
			{
				return this.FStringExecuteCommand = this.FStringExecuteCommand ?? new DelegateCommand<string>(this.StringExecuted, this.CanStringExecuted);
			}
		}

		/// <summary>
		/// 文字列で実行するコマンド を実行します。
		/// </summary>
		/// <param name="param">CommandParameter に指定された オブジェクトを指定します。</param>
		public void StringExecuted(string param)
		{
			if (!String.IsNullOrWhiteSpace(param))
			{
				var items = this.CommandLogItems;
				if (items.Contains(param))
				{
					items.Remove(param);
				}
				if (100 < items.Count)
				{
					items.RemoveAt(items.Count - 1);
				}
				items.Insert(0, param);

				var logs = this.LogItems;
				if (10 < logs.Count)
				{
					logs.RemoveAt(0);
				}
				logs.Add(param);
				this.CommandText = "";
			}
		}

		/// <summary>
		/// コマンドを実行可能かどうか判定します。
		/// </summary>
		/// <param name="param">CommandParameter に指定された オブジェクトを指定します。</param>
		/// <returns>コマンドが実行可能なら true を返します。</returns>
		public bool CanStringExecuted(string param)
		{
			return true;
		}

		#endregion // StringExecuteコマンド
		// ------------------------------------------------------------------------------------------------------------

		public void Hoge()
		{
			MessageBox.Show("Hoge!");
		}
	}
}
