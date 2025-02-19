using System;
using System.Diagnostics;
using System.Threading;

namespace Task
{
    static class F2
    {
        public static void Func2(int N)
        {
            // таймер часу
            Stopwatch stopwatch = new Stopwatch();
            // запуст таймера часу
            stopwatch.Start();

            //Thread.Sleep(1000);

            // створення локальних змінних
            int[,] MG = new int[N, N];
            int[,] MF = new int[N, N];
            int[,] MH = new int[N, N];
            int[,] MK = new int[N, N];

            // якщо N менше рівне 4, вводимо з клавіатури
            if (N <= 4)
            {
                Console.WriteLine("Func2. Input num for fill: ");
                int num = int.Parse(Console.ReadLine());

                Data.FillParam.FillFunc2(N, num, MF, MH, MK);
            }
            else
            {
                Data.FillParam.FillFunc2(N, 2, MF, MH, MK);
            }

            // F2: MG = SORT(MF - MH * MK)
            // множення матриць MH * MK
            int[,] tempMatr1 = Data.MathFucn.MulMatr(MH, MK);
            // віднімання матриць MF - |MH * MK|
            int[,] tempMatr2 = Data.MathFucn.SubMatr(MF, tempMatr1);
            // розтування матриці по рядках SORT(|MF - MH * MK|)
            MG = Data.MathFucn.SortLineMatr(tempMatr2);

            // зупинка таймеру часу
            stopwatch.Stop();

            // якщо N менше рівне 4, виводимо результат обчислення
            if (N <= 4)
            {
                Console.WriteLine($"Thread end: F2; N: {N}; Time: {stopwatch.ElapsedMilliseconds} ms; " + Data.PrintText.PrintMATR("MG", MG));
            }
            else
            {
                Console.WriteLine($"Thread end: F2; N: {N}; Time: {stopwatch.ElapsedMilliseconds} ms");
            }
        }
    }
}