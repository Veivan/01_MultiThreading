/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
			Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
			Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
			Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
			Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
			Console.WriteLine("Demonstrate the work of the each case with console utility.");
			Console.WriteLine();

			CaseA();
			CaseB(false);
			CaseB(true);
			CaseC(false); 
			CaseC(true); 
			CaseD(false);
			CaseD(true);

			Console.ReadLine();
		}

		#region Case A

		private static void CaseA()
		{
			Task<int> taskA1 = new Task<int>(() =>
			{
				Random random = new Random();
				return random.Next(1, 100);
			});
			Task taskA2 = taskA1.ContinueWith(task => SecondTaskA(task.Result));

			taskA1.Start();
			taskA2.Wait();
		}

		private static void SecondTaskA(int result)
		{
			const int bar = 50;
			if (result > bar)
				Console.WriteLine($"Case A: Result {result} more {bar}");
			else
				Console.WriteLine($"Case A: Result {result} less {bar}");
		}
		#endregion

		#region Case B

		private static void CaseB(bool execSecond)
		{
			Task taskB1 = new Task(() =>
			{
				if (execSecond)
					throw new Exception("Failed");
				else
					Console.WriteLine("Case B: First task - Success. Second task hasn't started");

			});
			Task taskB2 = taskB1.ContinueWith(task => Console.WriteLine($"Case B: First task - {task.Exception.Message}; Second task executed"),
				TaskContinuationOptions.OnlyOnFaulted);

			taskB1.Start();
			try
			{
				taskB1.Wait();
			}
			catch
			{
			}
		}

		#endregion

		#region Case C

		private static void CaseC(bool execSecond)
		{
			CancellationTokenSource tokenSource = new CancellationTokenSource();
			CancellationToken token = tokenSource.Token;

			Task taskC1 = new Task(() =>
			{
				Thread.Sleep(1000);
				if (!execSecond)
				{
					Console.WriteLine("Case C: First task - Success. Second task hasn't started");
					tokenSource.Cancel();
				}
				else
				{
					throw new Exception("Failed");
				}
			});
			Task taskC2 = taskC1.ContinueWith(task =>
			{
				Console.WriteLine("Case C: First task - failed. Second task executed");
			},
			token, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Current);

			taskC1.Start();
			try
			{
				taskC1.Wait();
			}
			catch { }
			finally
			{
				tokenSource.Dispose();
			}
		}
		#endregion

		#region Case D

		private static void CaseD(bool execSecond)
		{
			CancellationTokenSource tokenSource = new CancellationTokenSource();
			CancellationToken token = tokenSource.Token;

			CancellationTokenSource tokenSource2 = new CancellationTokenSource();
			CancellationToken token2 = tokenSource2.Token;

			Task taskD1 = new Task(() =>
			{
				Thread.Sleep(1000);
				if (!execSecond)
				{
					Console.WriteLine("Case D: First task - Success. Second task hasn't started");
					tokenSource2.Cancel();
				}
			}, token);
			Task taskD2 = taskD1.ContinueWith(task =>
			{
				Console.WriteLine("Case D: First task - Cancelled. Second task executed");
			},
			token2, TaskContinuationOptions.RunContinuationsAsynchronously, TaskScheduler.Current);

			taskD1.Start();
			if (execSecond)
				tokenSource.Cancel();

			try
			{
				taskD1.Wait();
			}
			catch
			{
			}
			finally
			{
				tokenSource.Dispose();
			}

			try
			{
				if (taskD1.IsCanceled)
					taskD2.Wait();
			}
			finally
			{
				tokenSource2.Dispose();
			}
		}

		#endregion
	}
}
