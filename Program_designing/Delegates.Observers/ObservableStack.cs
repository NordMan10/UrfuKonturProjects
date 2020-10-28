using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace Delegates.Observers
{
	public class StackOperationsLogger
	{
		public StringBuilder Log = new StringBuilder();

		public void SubscribeOn<T>(ObservableStack<T> stack)
		{
			stack.Notify += HandleEvent;
		}

		public string GetLog()
		{
			return Log.ToString();
		}

		public void HandleEvent<T>(object sender, ObservableStackEventArgs<T> e)
        {
			Log.Append(e.Data);
        }
	}

	public class ObservableStackEventArgs<T> : EventArgs
    {
		public ObservableStackEventArgs(StackEventData<T> data)
        {
			Data = data;
        }

		public StackEventData<T> Data { get; }
	}

	public delegate void ObservableStackEventHandler<T>(object sender, ObservableStackEventArgs<T> e);

	public class ObservableStack<T>
	{
        private readonly List<T> data = new List<T>();

		public event ObservableStackEventHandler<T> Notify;

		protected virtual void OnNotify(ObservableStackEventArgs<T> args)
        {
			Notify?.Invoke(this, args);
		}

		public void Push(T obj)
		{
			data.Add(obj);
			OnNotify(new ObservableStackEventArgs<T>(new StackEventData<T> { IsPushed = true, Value = obj }));
		}

		public T Pop()
		{
			if (data.Count == 0)
				throw new InvalidOperationException();

			var result = data[data.Count - 1];
			data.Remove(result);

			OnNotify(new ObservableStackEventArgs<T>(new StackEventData<T> { IsPushed = false, Value = result }));

			return result;
		}
	}
}
