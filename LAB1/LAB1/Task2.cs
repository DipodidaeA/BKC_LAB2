using Data;
using System;
using System.Diagnostics;
using System.Threading;

namespace Task
{
    static class P2
    {
        public static void Func2(int N, ProgData pD)
        {
            Thread.Sleep(1000);

            FillParam fP = new FillParam();
            PrintText pT = new PrintText();
            MathFucn mF = new MathFucn();

            // таймер часу
            Stopwatch stopwatch = new Stopwatch();


            // якщо N менше рівне 4, вводимо з клавіатури
            Console.WriteLine("Func2. Input num...");
            if (N <= 4)
            {
                fP.FillMatr("MX", pD.MX);
                fP.FillVect("B", pD.B);
            }
            else
            {
                fP.FillMatrRand(pD.MX);
                fP.FillVectRand(pD.B);
            }

            // СИГНАЛ: B, MX введено	– S(1, 2), S(3, 2), S(4, 2)
            pD.semaphore_P1.Release();
            pD.semaphore_P3.Release();
            pD.semaphore_P4.Release();

            // ЧЕКАЄ: введення даних d, Z, MM  		– W(2, 4)
            pD.semaphore_P2.WaitOne();

            // запуст таймера часу
            stopwatch.Start();

            // дія 1
            int[] Bh = mF.GetPartVect(1, pD.H, pD.B);
            int ai = mF.MinNumInVect(Bh);

            // ЧЕКАЄ: а є вільним  		– W(2, 1)
            pD.semaphore_P2.WaitOne();

            // дія 2
            pD.a = ai;    // КД 1

            // СИГНАЛ: а є вільним         -- S(3,2)
            pD.semaphore_P3.Release();

            // ЧЕКАЄ: d є вільним  		– W(2, 1)
            pD.semaphore_P2.WaitOne();

            int d1 = pD.d;         // КД 2

            // СИГНАЛ: d є вільним         -- S(3,2)
            pD.semaphore_P3.Release();

            // дія 3
            int[,] MXh = mF.GetPartMatrTab(1, pD.H, pD.MX);
            int[,] MR = mF.MulMatr(pD.MM, MXh);
            int[] U = mF.MulVecMatr(pD.Z, MR);
            int[] C = mF.MulScalVector(d1, Bh);

            int[] Rh = mF.AddVector(C, U);

            // дія 4
            pD.Q1hT2 = mF.SortVect(Rh);

            // СИГНАЛ: Q1h обчислений для Т1         -- S(1,2)
            pD.semaphore_P1.Release();

            // ЧЕКАЄ: а є вільним  		– W(2, 1)
            pD.semaphore_P2.WaitOne();

            ai = pD.a;          // КД 3

            // СИГНАЛ: a є вільним         -- S(3,2)
            pD.semaphore_P3.Release();

            // ЧЕКАЄ: Т1 закінчив обчислення  		– W(2, 1)
            pD.semaphore_P2.WaitOne();

            // дія 7
            int[] Xh = mF.MulScalVector(ai, mF.GetPartVect(1, pD.H, pD.Q));
            pD.X = mF.VectConcat(pD.X, Xh);

            // СИГНАЛ: Т2 закінчив обчислення         -- S(3,2)
            pD.semaphore_P3.Release();

            // зупинка таймеру часу
            stopwatch.Stop();

            Console.WriteLine($"P2 end; H: {pD.H}; Time: {stopwatch.ElapsedMilliseconds} ms");
        }
    }
}