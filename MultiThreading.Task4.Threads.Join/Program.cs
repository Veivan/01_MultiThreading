/*
 * 4.	Write a program which recursively creates 10 threads.
 * Each thread should be with the same body and receive a state with integer number, decrement it,
 * print and pass as a state into the newly created thread.
 * Use Thread class for this task and Join for waiting threads.
 * 
 * Implement all of the following options:
 * - a) Use Thread class for this task and Join for waiting threads.
 * - b) ThreadPool class for this task and Semaphore for waiting threads.
 */

using System;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
	class Program
	{
		static int number = 10;
		static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1);

		static void Main(string[] args)
		{
			Console.WriteLine("4.	Write a program which recursively creates 10 threads.");
			Console.WriteLine("Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread.");
			Console.WriteLine("Implement all of the following options:");
			Console.WriteLine();
			Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");
			Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");

			Console.WriteLine();

			UseThreads(number);
			Console.WriteLine();

			number = 10;
			UseThreadPool(number);

			Console.ReadLine();
		}

		#region Using Thread
		private static void UseThreads(int number)
		{
			var thread = new Thread(ExecThread)
			{
				Name = "Thread_" + number,
			};
			thread.Start(number);
			thread.Join();
		}

		private static void ExecThread(object state)
		{
			Thread.Sleep(500);
			int n = (int)state;
			if (n > 0)
			{
				n--;
				Console.WriteLine($"Thread {Thread.CurrentThread.Name} - {n}");
				UseThreads(n);
			}
		}
		#endregion

		#region Using ThreadPool
		private static void UseThreadPool(int number)
		{
			ThreadPool.QueueUserWorkItem(ExecTask, number);
		}

		private static void ExecTask(object state)
		{
			semaphoreSlim.Wait();
			Thread.Sleep(500);
			int n = (int)state;
			if (n > 0)
			{
				n--;
				Console.WriteLine($"Task {Thread.CurrentThread.Name} - {n}");
				UseThreadPool(n);
				semaphoreSlim.Release();
			}
		}

		#endregion
	}
}
