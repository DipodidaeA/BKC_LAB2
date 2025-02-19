using System;
using System.Diagnostics;
using System.Threading;

namespace Task
{
    static class F3
    {
        public static void Func3(int N)
        {
            // таймер часу
            Stopwatch stopwatch = new Stopwatch();
            // запуст таймера часу
            stopwatch.Start();

            //Thread.Sleep(1000);

            // створення локальних змінних
            int[] S = new int[N];
            int[] O = new int[N];
            int[] P = new int[N];
            int[] V = new int[N];
            int[,] MR = new int[N, N];
            int[,] MS = new int[N, N];

            // якщо N менше рівне 4, вводимо з клавіатури
            if (N <= 4)
            {
                Console.WriteLine("Func3. Input num for fill: ");
                int num = int.Parse(Console.ReadLine());

                Data.FillParam.FillFunc3(N, num, O, P, V, MR, MS);
            }
            else
            {
                Data.FillParam.FillFunc3(N, 3, O, P, V, MR, MS);
            }

            // F3: S = (O + P + V) * (MR * MS)
            // додавання векторів O + P
            int[] tempVec1 = Data.MathFucn.AddVector(O, P);
            // додавання векторів |O + P| + V
            int[] tempVec2 = Data.MathFucn.AddVector(tempVec1, V);
            // множення  матриць MR * MS
            int[,] tempMatr = Data.MathFucn.MulMatr(MR, MS);
            // множення вектора на матрицю |(O + P + V)| * |(MR * MS)|
            S = Data.MathFucn.MulVecMatr(tempVec2, tempMatr);

            // зупинка таймеру часу
            stopwatch.Stop();

            // якщо N менше рівне 4, виводимо результат обчислення
            if (N <= 4)
            {
                Console.WriteLine($"Thread end: F3; N: {N}; Time: {stopwatch.ElapsedMilliseconds} ms; " + Data.PrintText.PrintVector("S", S));
            }
            else
            {
                Console.WriteLine($"Thread end: F3; N: {N}; Time: {stopwatch.ElapsedMilliseconds} ms");
            }
        }
    }
}