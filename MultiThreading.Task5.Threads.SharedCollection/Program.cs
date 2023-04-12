/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task5.Threads.SharedCollection
{
	class Program
	{
		static List<int> sharedList = new List<int>();
		static AutoResetEvent waitHandler = new AutoResetEvent(true);
		static Random random = new Random();

		static void Main(string[] args)
		{
			Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
			Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
			Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
			Console.WriteLine();

			Task task1 = Task.Factory.StartNew(() => AddNumbers());
			Task task2 = Task.Factory.StartNew(() => PrintNumbers());

			Task.WaitAll(task1, task2);
			Console.ReadKey();
		}

		private static void AddNumbers()
		{
			waitHandler.WaitOne();
			for (int i = 0; i < 10; i++)
			{
				var x = random.Next(1, 100);
				sharedList.Add(x);
				Console.WriteLine($"{x} added to the list");
				waitHandler.Set();
				waitHandler.WaitOne();
			}
		}

		private static void PrintNumbers()
		{
			waitHandler.WaitOne();
			while (true)
			{
				if (sharedList.Count > 0)
				{
					Console.WriteLine($"List : {string.Join(", ", sharedList)}");
				}
				waitHandler.Set();
				waitHandler.WaitOne();
			}
		}

	}
}
