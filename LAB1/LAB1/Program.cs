/*
    Лабораторна робота ЛР2 Варіант 23
    F: X = sort(d * B + Z * (MM * MX)) * min(B)
    P1: -
    P2: B, MX
    P3: -
    P4: X, MM, Z, d
    Рак Антон Вікторович ІМ-24
    Дата 05 03 2025
*/

using Data;
using System;
using System.Diagnostics;
using System.Threading;

namespace LAB2
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

            // Створення задач з ім'ям, приорітетом та функцією
            Thread thread1 = CreateTask("P1", ThreadPriority.Highest, () => Task.P1.Func1(N, pD));
            Thread thread2 = CreateTask("P2", ThreadPriority.Highest, () => Task.P2.Func2(N, pD));
            Thread thread3 = CreateTask("P3", ThreadPriority.Highest, () => Task.P3.Func3(N, pD));
            Thread thread4 = CreateTask("P4", ThreadPriority.Highest, () => Task.P4.Func4(N, pD));

            // щоб програма не завершувалася поки потоки не виконаються
            thread1.Join();
            thread2.Join();
            thread3.Join();
            thread4.Join();

            // зупинка таймеру часу
            stopwatch.Stop();

            // вивід всього часу програми
            Console.WriteLine($"Program End; N: {N}; Time: {stopwatch.ElapsedMilliseconds} ms");
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
