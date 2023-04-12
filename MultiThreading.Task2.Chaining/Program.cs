/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    class Program
    {
        const int ArrayLen = 10;
        static int[] intArray = new int[ArrayLen];
        static Random random = new Random();

        static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();

            Task task1 = new Task(Task1);

            Task task2 = task1.ContinueWith(Task2);
            Task task3 = task2.ContinueWith(Task3);
            Task task4 = task3.ContinueWith(Task4);

            task1.Start();
            task4.Wait();
            Console.ReadLine();
        }

        static void Task1()
        {
            const int taskNumber = 1;
            for (int i = 0; i < intArray.Length; i++)
            {
				intArray[i] = random.Next(1, 100);
            };
            PrintArray(taskNumber);
        }

        static void Task2(Task t)
        {
            const int taskNumber = 2;
            var mult = random.Next(2, 10);
            for (int i = 0; i < intArray.Length; i++)
            {
                intArray[i] *= mult;
            };
            PrintArray(taskNumber);
        }

        static void Task3(Task t)
        {
            const int taskNumber = 3;
            Array.Sort(intArray);
            PrintArray(taskNumber);
        }

        static void Task4(Task t)
        {
            const int taskNumber = 4;
            double total = 0;
            for (int i = 0; i < intArray.Length; i++)
            {
                total += intArray[i];
            };
            Console.WriteLine($"Task{taskNumber} result: {total/intArray.Length}");
        }

        static void PrintArray(int taskNumber)
        {
            Console.WriteLine($"Task{taskNumber} result: {string.Join(", ", intArray)}" );
        }
    }
}
