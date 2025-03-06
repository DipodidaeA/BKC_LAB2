using Data;
using System;
using System.Diagnostics;
using System.Threading;
using System.Xml.Linq;

namespace Task
{
    static class P4
    {
        public static void Func4(int N, ProgData pD)
        {
            Thread.Sleep(1000);

            FillParam fP = new FillParam();
            PrintText pT = new PrintText();
            MathFucn mF = new MathFucn();
            // таймер часу
            Stopwatch stopwatch = new Stopwatch();

            // ЧЕКАЄ: введення даних B, MX  		– W(4, 2)
            pD.semaphore_P4.WaitOne();

            // якщо N менше рівне 4, вводимо з клавіатури
            Console.WriteLine("Func4. Input num...");
            if (N <= 4)
            {
                fP.FillMatr("MM", pD.MM);
                fP.FillVect("Z", pD.Z);
                Console.Write("d: ");
                pD.d = int.Parse(Console.ReadLine());

                Console.WriteLine();

            }
            else
            {
                fP.FillMatrAuto(pD.MM);
                fP.FillVectAuto(pD.Z);
                fP.FillNumAuto(pD.d);
            }

            // СИГНАЛ: MM, Z, d введено	– S(1, 4), S(2, 4), S(3, 4)
            pD.semaphore_P1.Release();
            pD.semaphore_P2.Release();
            pD.semaphore_P3.Release();
            
            // запуст таймера часу
            stopwatch.Start();

            // дія 1
            int[] Bh = mF.GetPartVect(3, pD.H, pD.B);
            int ai = mF.MinNumInVect(Bh);

            // ЧЕКАЄ: а є вільним  		– W(4, 3)
            pD.semaphore_P4.WaitOne();

            // дія 2
            pD.a = ai;    // КД 1

            // ЧЕКАЄ: d є вільним  		– W(4, 3)
            pD.semaphore_P4.WaitOne();

            int d1 = pD.d;         // КД 2

            // дія 3
            int[,] MXh = mF.GetPartMatrTab(3, pD.H, pD.MX);
            int[,] MR = mF.MulMatr(pD.MM, MXh);
            int[] U = mF.MulVecMatr(pD.Z, MR);
            int[] C = mF.MulScalVector(d1, Bh);

            int[] Rh = mF.AddVector(C, U);

            // дія 4
            pD.Q1hT4 = mF.SortVect(Rh);

            // СИГНАЛ: Q1hT3 обчислений для Т3         -- S(3,4)
            pD.semaphore_P3.Release();

            // ЧЕКАЄ: а є вільним  		– W(4, 3)
            pD.semaphore_P4.WaitOne();

            ai = pD.a;          // КД 3

            // ЧЕКАЄ: Т3 закінчив обчислення  		– W(4, 3)
            pD.semaphore_P4.WaitOne();

            // дія 7
            int[] Xh = mF.MulScalVector(ai, mF.GetPartVect(3, pD.H, pD.Q));
            pD.X = mF.VectConcat(pD.X, Xh);

            // якщо N менше рівне 4, виводимо результат обчислення
            if (N <= 4)
            {
                Console.WriteLine($"P4 end; H: {pD.H}; Time: {stopwatch.ElapsedMilliseconds} ms");
                Console.WriteLine("Res: ");
                pT.PrintVect("X", pD.X);
            }
            else
            {
                Console.WriteLine($"P4 end; H: {pD.H}; Time: {stopwatch.ElapsedMilliseconds} ms");
                Console.WriteLine("Res: ");
                Console.WriteLine("X[0]: " + pD.X[0]);
            }
        }
    }
}