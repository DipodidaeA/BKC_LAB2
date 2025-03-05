using Data;
using System;
using System.Diagnostics;
using System.Threading;

namespace Task
{
    static class P3
    {
        public static void Func3(int N, ProgData pD)
        {
            Thread.Sleep(1000);

            FillParam fP = new FillParam();
            PrintText pT = new PrintText();
            MathFucn mF = new MathFucn();

            // таймер часу
            Stopwatch stopwatch = new Stopwatch();

            // ЧЕКАЄ: введення від Т2 -- W(3,2)
            pD.semaphore_P3.WaitOne();
            // ЧЕКАЄ: введення від Т4 -- W(3,4)
            pD.semaphore_P3.WaitOne();

            // запуск таймеру часу
            stopwatch.Start();

            // дія 1
            int[] Bh = mF.GetPartVect(2, pD.H, pD.B);
            int ai = mF.MinNumInVect(Bh);

            // ЧЕКАЄ: а є вільним         -- S(3,2)
            pD.semaphore_P3.WaitOne();

            // дія 2
            pD.a = ai;    // КД 1

            // СИГНАЛ: а є вільним         -- S(4,3)
            pD.semaphore_P4.Release();

            // ЧЕКАЄ: d є вільним         -- S(3,2)
            pD.semaphore_P3.WaitOne();

            int d1 = pD.d;         // КД 2

            // СИГНАЛ: d є вільним         -- S(4,3)
            pD.semaphore_P4.Release();

            // дія 3
            int[,] MXh = mF.GetPartMatrTab(2, pD.H, pD.MX);
            int[,] MR = mF.MulMatr(pD.MM, MXh);
            int[] U = mF.MulVecMatr(pD.Z, MR);
            int[] C = mF.MulScalVector(d1, Bh);

            int[] Rh = mF.AddVector(C, U);

            // дія 4
            int[] Q1hT3 = mF.SortVect(Rh);

            // ЧЕКАЄ: обчислення Q1hT4 від Т4	  -- W(3,4)
            pD.semaphore_P3.WaitOne();

            // дія 5
            pD.Q2hT3 = mF.SortVectConcat(Q1hT3, pD.Q1hT4);

            // СИГНАЛ: обчислення Q2hT3 для Т1	   -- W(1,3)
            pD.semaphore_P1.Release();

            // ЧЕКАЄ: а є вільним         -- S(3,2)
            pD.semaphore_P3.WaitOne();

            ai = pD.a;          // КД 3

            // СИГНАЛ: a є вільним         -- S(4,3)
            pD.semaphore_P4.Release();

            // ЧЕКАЄ: Т2 закінчив обчислення  		– W(3, 2)
            pD.semaphore_P3.WaitOne();

            // дія 7
            int[] Xh = mF.MulScalVector(ai, mF.GetPartVect(2, pD.H, pD.Q));
            pD.X = mF.VectConcat(pD.X, Xh);

            // СИГНАЛ: Т3 закінчив обчислення         -- S(4,3)
            pD.semaphore_P4.Release();

            // зупинка таймеру часу
            stopwatch.Stop();

            Console.WriteLine($"P3 end; H: {pD.H}; Time: {stopwatch.ElapsedMilliseconds} ms");

        }
    }
}