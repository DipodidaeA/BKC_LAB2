using System;
using System.Diagnostics;
using System.Threading;

namespace Task
{
    static class F1
    {
        public static void Func1(int N)
        {
            // таймер часу
            Stopwatch stopwatch = new Stopwatch();
            // запуст таймера часу
            stopwatch.Start();

            //Thread.Sleep(1000);

            // створення локальних змінних
            int[] E = new int[N];
            int[] A = new int[N];
            int[] B = new int[N];
            int[] C = new int[N];
            int[] D = new int[N];
            int[,] MA = new int[N,N];
            int[,] MD = new int[N,N];

            // якщо N менше рівне 4, вводимо з клавіатури
            if (N <= 4)
            {
                Console.WriteLine("Func1. Input num for fill: ");
                int num = int.Parse(Console.ReadLine());

                Data.FillParam.FillFunc1(N, num, A, B, C, D, MA, MD);
            }
            else 
            {
                Data.FillParam.FillFunc1(N, 1, A, B, C, D, MA, MD);
            }

            // F1: E = A + B + C + D * (MA * MD)
            // множення матриць (MA * MD)
            int[,] tempMatr =  Data.MathFucn.MulMatr(MA, MD);
            // множення вектора на матрицю D * |(MA * MD)|
            int[] tempVec1 = Data.MathFucn.MulVecMatr(D, tempMatr);
            // додавання векторів C + |D * (MA * MD)|
            int[] tempVec2 = Data.MathFucn.AddVector(tempVec1, C);
            // додавання векторів B + |C + D * (MA * MD)|
            int[] tempVec3 = Data.MathFucn.AddVector(tempVec2, B);
            // додавання векторів A + |B + C + D * (MA * MD)|
            E = Data.MathFucn.AddVector(tempVec3, A);

            // зупинка таймеру часу
            stopwatch.Stop();

            // якщо N менше рівне 4, виводимо результат обчислення
            if (N <= 4)
            {
                Console.WriteLine($"Thread end: F1; N: {N}; Time: {stopwatch.ElapsedMilliseconds} ms; " + Data.PrintText.PrintVector("E", E));
            }
            else
            {
                Console.WriteLine($"Thread end: F1; N: {N}; Time: {stopwatch.ElapsedMilliseconds} ms");
            }
        }
    }
}