using Data;
using System;
using System.Diagnostics;
using System.Threading;

namespace Task
{
    static class P1
    {
        public static void Func1(int N, ProgData pD)
        {
            Thread.Sleep(1000);

            FillParam fP = new FillParam();
            PrintText pT = new PrintText();
            MathFucn mF = new MathFucn();

            // таймер часу
            Stopwatch stopwatch = new Stopwatch();

            // ЧЕКАЄ: введення від Т2 -- W(1,2)
            pD.semaphore_P1.WaitOne();
            // ЧЕКАЄ: введення від Т4 -- W(1,4)
            pD.semaphore_P1.WaitOne();

            // запуск таймеру часу
            stopwatch.Start();

            // дія 1
            int[] Bh = mF.GetPartVect(0, pD.H, pD.B);
            int ai = mF.MinNumInVect(Bh);

            // дія 2
            pD.a = ai;    // КД 1

            // СИГНАЛ: а є вільним         -- S(2,1)
            pD.semaphore_P2.Release();

            int d1 = pD.d;         // КД 2

            // СИГНАЛ: d є вільним         -- S(2,1)
            pD.semaphore_P2.Release();

            // дія 3
            int[,] MXh = mF.GetPartMatrTab(0, pD.H, pD.MX);
            int[,] MR = mF.MulMatr(pD.MM, MXh);
            int[] U = mF.MulVecMatr(pD.Z, MR);
            int[] C = mF.MulScalVector(d1, Bh);

            int[] Rh = mF.AddVector(C, U);

            // дія 4
            int[] Q1hT1 = mF.SortVect(Rh);

            // ЧЕКАЄ: обчислення Q1hT2 від Т2	  -- W(1,2)
            pD.semaphore_P1.WaitOne();

            // дія 5
            int[] Q2hT1 = mF.SortVectConcat(Q1hT1, pD.Q1hT2);

            // ЧЕКАЄ: обчислення Q2hT3 від Т3	   -- W(1,3)
            pD.semaphore_P1.WaitOne();

            // дія 6
            pD.Q = mF.SortVectConcat(Q2hT1, pD.Q2hT3);
            ai = pD.a;          // КД 3

            // СИГНАЛ: a є вільним         -- S(2,1)
            pD.semaphore_P2.Release();

            // дія 7
            pD.X = mF.MulScalVector(ai, mF.GetPartVect(0, pD.H, pD.Q));

            // СИГНАЛ: Т1 закінчив обчислення         -- S(2,1)
            pD.semaphore_P2.Release();

            // зупинка таймеру часу
            stopwatch.Stop();

            Console.WriteLine($"P1 end; H: {pD.H}; Time: {stopwatch.ElapsedMilliseconds} ms");
        }
    }
}