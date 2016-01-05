using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Interactivity;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace WpfTriggerActionTest
{
	// ================================================================================================================
	#region DragMoveBehavior

	/// <summary>
	/// Windowの任意の場所を掴んでドラッグできるようになります。
	/// </summary>
	public class DragMoveBehavior : Behavior<Window>
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public DragMoveBehavior()
		{
		}

		/// <summary>
		/// 要素にアタッチされた時に実行する処理
		/// </summary>
		protected override void OnAttached()
		{
			base.OnAttached();
			this.AssociatedObject.MouseLeftButtonDown += AssociatedObjectMouseLeftButtonDown;
		}

		/// <summary>
		/// Windowの任意の場所を掴んでドラッグする
		/// </summary>
		/// <param name="sender">イベントを送信したオブジェクトを指定します。</param>
		/// <param name="e">イベント引数を指定します。</param>
		void AssociatedObjectMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			this.AssociatedObject.DragMove();
		}

		/// <summary>
		/// 要素にデタッチされた時に実行する処理
		/// </summary>
		protected override void OnDetaching()
		{
			base.OnDetaching();
			this.AssociatedObject.MouseLeftButtonDown -= AssociatedObjectMouseLeftButtonDown;
		}
	}

	#endregion
	// ================================================================================================================
	#region EnterKeyUpBehavior

	/// <summary>
	/// エンターキー押下を検出し、割り当てられたコマンドを実行するクラスを定義します。
	/// </summary>
	/// <remarks>
	/// コンボボックス、テキストボックスに対応しています。入力された文字列をコマンドパラメータとして送信します。
	/// </remarks>
	public class EnterKeyUpBehavior : Behavior<FrameworkElement>
	{
		// ------------------------------------------------------------------------------------------------------------
		#region コンストラクタ

		/// <summary>
		/// Okaden.Common.Wpf.Interactivity.EnterKeyUpBehavior クラスの新しいインスタンスを作成します。
		/// </summary>
		public EnterKeyUpBehavior()
		{
			this.FActionDictionary.Add(typeof(ComboBox), this.ComboboxAction);
			this.FActionDictionary.Add(typeof(TextBox), this.TextBoxAction);
		}

		#endregion // コンストラクタ
		// ------------------------------------------------------------------------------------------------------------

		/// <summary>
		/// 要素にアタッチされた時に実行する処理を定義します。
		/// </summary>
		protected override void OnAttached()
		{
			base.OnAttached();
			this.AssociatedObject.KeyUp += AssociatedObjectKeyUp;
		}

		/// <summary>
		/// 要素からデタッチされる時に実行する処理を定義します。
		/// </summary>
		protected override void OnDetaching()
		{
			base.OnDetaching();
			this.AssociatedObject.KeyUp -= AssociatedObjectKeyUp;
		}

		/// <summary>
		/// エンターキー押下時 のイベントを処理します。
		/// </summary>
		/// <param name="sender">イベントを送信したオブジェクトを指定します。</param>
		/// <param name="e">キー情報を含む、イベント引数を指定します。</param>
		private void AssociatedObjectKeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				var type = this.AssociatedObject.GetType();
				var action = this.FActionDictionary[type];
				if (action != null)
				{
					action();
				}
			}
		}

		/// <summary>
		/// 型によって実行する処理のディクショナリを管理します。
		/// </summary>
		private Dictionary<Type, Action> FActionDictionary = new Dictionary<Type, Action>();

		// ------------------------------------------------------------------------------------------------------------
		#region Command プロパティ（依存関係プロパティ）

		/// <summary>
		/// 実行するコマンド を取得または設定します。
		/// </summary>
		public ICommand Command
		{
			get { return (ICommand)GetValue(CommandProperty); }
			set { SetValue(CommandProperty, value); }
		}

		/// <summary>
		/// 実行するコマンド を管理する依存関係プロパティ
		/// </summary>
		public static readonly DependencyProperty CommandProperty =
			DependencyProperty.Register("Command", typeof(ICommand), typeof(EnterKeyUpBehavior), new PropertyMetadata(null));

		#endregion // Command プロパティ
		// ------------------------------------------------------------------------------------------------------------

		/// <summary>
		/// 割り当てられたコマンドを実行します。
		/// このメソッドは関連付けられたコントロールがコンボボックスの時に実行します。
		/// </summary>
		private void ComboboxAction()
		{
			var ctl = this.AssociatedObject as ComboBox;
			if (ctl != null)
			{
				var cmd = this.Command;
				var param = ctl.Text;
				if (cmd != null && cmd.CanExecute(param))
				{
					cmd.Execute(param);
				}
			}
		}

		/// <summary>
		/// 割り当てられたコマンドを実行します。
		/// このメソッドは関連付けられたコントロールがテキストボックスの時に実行します。
		/// </summary>
		private void TextBoxAction()
		{
			var ctl = this.AssociatedObject as TextBox;
			if (ctl != null)
			{
				var cmd = this.Command;
				var param = ctl.Text;
				if (cmd != null && cmd.CanExecute(param))
				{
					cmd.Execute(param);
				}
			}
		}
	}

	#endregion
	// ================================================================================================================
}
