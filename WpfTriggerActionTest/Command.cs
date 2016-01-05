using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace WpfTriggerActionTest
{
	// ------------------------------------------------------------------------------------------------------------
	#region No parameter DelegateCommand

	/// <summary>
	/// 汎用コマンド
	/// </summary>
	/// <remarks>
	/// このクラスは下記サイトのものをコピペして作成した。
	/// かずきのBlog@hatena
	/// http://okazuki.hatenablog.com/entry/20100223/1266897125
	/// </remarks>
	public sealed class DelegateCommand : ICommand
	{
		/// <summary>
		/// 実行する処理を格納するオブジェクトを定義します。
		/// </summary>
		private Action _execute;

		/// <summary>
		/// 実行可能かどうかを判定する処理を格納するオブジェクトを定義します。
		/// </summary>
		private Func<bool> _canExecute;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="execute">実行する処理を指定します。</param>
		public DelegateCommand(Action execute) : this(execute, () => true)
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="execute">実行する処理を指定します。</param>
		/// <param name="canExecute">実行可能かどうかを判定する処理を指定します。省略した場合は常にtrueを返します。</param>
		public DelegateCommand(Action execute, Func<bool> canExecute)
		{
			_execute = execute;
			_canExecute = canExecute;
		}

		/// <summary>
		/// コマンドが実行可能かどうか判定し、実行可能ならばtrueを返します。
		/// </summary>
		/// <returns></returns>
		public bool CanExecute()
		{
			return _canExecute();
		}

		/// <summary>
		/// コマンドを実行します。
		/// </summary>
		public void Execute()
		{
			_execute();
		}

		/// <summary>
		/// コマンドの実行可能状態の変化を通知するイベントハンドラを定義します。
		/// </summary>
		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		// 明示的なインターフェースの実装
		#region ICommand
		bool ICommand.CanExecute(object parameter)
		{
			return CanExecute();
		}
		void ICommand.Execute(object parameter)
		{
			Execute();
		}
		#endregion
	}
	#endregion
	// ------------------------------------------------------------------------------------------------------------
	// ------------------------------------------------------------------------------------------------------------
	#region Parameter DelegateCommand

	/// <summary>
	/// 引数がある場合の汎用コマンド
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public sealed class DelegateCommand<T> : ICommand
	{
		/// <summary>
		/// 実行する処理を格納するオブジェクトを定義します。
		/// </summary>
		private Action<T> _execute;

		/// <summary>
		/// 実行可能かどうかを判定する処理を格納するオブジェクトを定義します。
		/// </summary>
		private Func<T, bool> _canExecute;

		/// <summary>
		/// 引数の型が値型ならtrueを保持します。
		/// </summary>
		private static readonly bool IS_VALUE_TYPE;

		/// <summary>
		/// 静的コンストラクタ
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
		static DelegateCommand()
		{
			IS_VALUE_TYPE = typeof(T).IsValueType;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="execute">実行する処理を指定します。</param>
		public DelegateCommand(Action<T> execute) : this(execute, o => true)
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="execute">実行する処理を指定します。</param>
		/// <param name="canExecute">実行可能かどうかを判定する処理を指定します。省略した場合は常にtrueを返します。</param>
		public DelegateCommand(Action<T> execute, Func<T, bool> canExecute)
		{
			_execute = execute;
			_canExecute = canExecute;
		}

		/// <summary>
		/// コマンドが実行可能かどうか判定し、実行可能ならばtrueを返します。
		/// </summary>
		/// <param name="parameter">任意の型の引数を定義します。</param>
		/// <returns></returns>
		public bool CanExecute(T parameter)
		{
			return _canExecute(parameter);
		}

		/// <summary>
		/// コマンドを実行します。
		/// </summary>
		/// <param name="parameter">任意の型の引数を定義します。</param>
		public void Execute(T parameter)
		{
			_execute(parameter);
		}

		/// <summary>
		/// コマンドの実行可能状態の変化を通知するイベントハンドラを定義します。
		/// </summary>
		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		// 明示的なインターフェースの実装
		#region ICommand
		bool ICommand.CanExecute(object parameter)
		{
			return CanExecute(Cast(parameter));
		}

		void ICommand.Execute(object parameter)
		{
			Execute(Cast(parameter));
		}
		#endregion

		/// <summary>
		/// convert parameter value
		/// </summary>
		/// <param name="parameter"></param>
		/// <returns></returns>
		private static T Cast(object parameter)
		{
			if (parameter == null && IS_VALUE_TYPE)
			{
				return default(T);
			}
			return (T)parameter;
		}
	}

	#endregion
	// ------------------------------------------------------------------------------------------------------------
}
