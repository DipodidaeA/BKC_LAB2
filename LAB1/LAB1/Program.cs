/*
    Лабораторна робота ЛР2 Варіант 23
    F: X = sort(d * B + Z * (MM * MX)) * min(B)
    P1: B, MX
    P2: MM, X, Z, d
    Рак Антон Вікторович ІМ-24
    Дата 04 03 2025
*/

using Data;
using System;
using System.Diagnostics;
using System.Threading;

namespace LAB1
{
    internal class Program
    {
        // Точка початку програми
        static void Main(string[] args)
        {
            // ввід числа N
            Console.Write("Input N: ");
            int N = int.Parse(Console.ReadLine());

            ProgData pD = new ProgData(N);

            // таймер часу
            Stopwatch stopwatch = new Stopwatch();
            // запуст таймера часу
            stopwatch.Start();

            // Створення задач з ім'ям, приорітетом, номером ядра та функцією
            Thread thread1 = CreateTask("F1", ThreadPriority.Highest, () => Task.F1.Func1(N, pD));
            Thread thread2 = CreateTask("F2", ThreadPriority.Lowest, () => Task.F2.Func2(N, pD));

            // щоб програма не завершувалася поки потоки не виконаються
            thread1.Join();
            thread2.Join();

            // зупинка таймеру часу
            stopwatch.Stop();

            // вивід всього часу програми
            Console.WriteLine($"Full Time: {stopwatch.ElapsedMilliseconds} ms");
        }

        // Створення задая
        static Thread CreateTask(string name, ThreadPriority priority, Action task)
        {
            // створення потоку з виконуваною функцією та розміром стеку 1 МБ
            Thread thread = new Thread(task.Invoke, maxStackSize: 1024*1024);

            // задання Ім'я потоку
            thread.Name = name;
            // задання приорітету потоку
            thread.Priority = priority;
            // виконання у фоновому режимі
            thread.IsBackground = true;

            // запуску потоку
            thread.Start();
            Console.WriteLine($"Thread start: {thread.Name}; Priority: {thread.Priority}");

            return thread;
        }
    }
}
