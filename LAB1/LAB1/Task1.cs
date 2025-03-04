using Data;
using System;
using System.Diagnostics;
using System.Threading;

namespace Task
{
    static class F1
    {

        //ПОТІК 1:              ||B, MX || K = d * B    || Y = Z * MK || Q = K + Y || Q = sort(Q) || X = Q * m || 
        //ПОТІК 2: MM, X, Z, d  ||      || MK = MM * MX || m = min(B) ||                                       || Console(X)
        public static void Func1(int N, ProgData pD)
        {
            FillParam fP = new FillParam();
            PrintText pT = new PrintText();
            MathFucn mF = new MathFucn();

            // таймер часу
            Stopwatch stopwatch = new Stopwatch();

            // чекаємо на дозвіл працювати від потоку 2
            Console.WriteLine("<<<P1 Blocked>>>");
            pD.semaphore_P1.WaitOne();
            Console.WriteLine("<<<P1 UnBlocked>>>");

            // якщо N менше рівне 4, вводимо з клавіатури
            Console.WriteLine("Func1. Input num...");
            if (N <= 4)
            {
                fP.FillVect("B", pD.B);
                fP.FillMatr("MX", pD.MX);

                // запуст таймера часу
                stopwatch.Start();
            }
            else 
            {
                // запуст таймера часу
                stopwatch.Start();

                fP.FillVectRand(pD.B);
                fP.FillMatrRand(pD.MX);
            }

            // розблоковує потік 2
            Console.WriteLine("<<<P1 UnBlocking P2>>>");
            pD.semaphore_P2.Release();

            // K = d * B
            Console.WriteLine("((( P1 Calculate: K = d * B )))");
            pD.K = mF.MulScalVector(pD.d, pD.B);

            // чекаємо на дозвіл працювати від потоку 2
            Console.WriteLine("<<<P1 Blocked>>>");
            pD.semaphore_P1.WaitOne();
            Console.WriteLine("<<<P1 UnBlocked>>>");

            // Y = Z * MK
            Console.WriteLine("((( P1 Calculate: Y = Z * MK )))");
            pD.Y = mF.MulVecMatr(pD.Z, pD.MK);
            // Q = K + Y
            Console.WriteLine("((( P1 Calculate: Q = K + Y )))");
            pD.Q = mF.AddVector(pD.K, pD.Y);
            // Q = sort(Q)
            Console.WriteLine("((( P1 Calculate: Q = sort(Q) )))");
            mF.SortVect(pD.Q);

            // чекаємо на дозвіл працювати від потоку 2
            Console.WriteLine("<<<P1 Blocked>>>");
            pD.semaphore_P1.WaitOne();
            Console.WriteLine("<<<P1 UnBlocked>>>");

            // X = Q * m
            Console.WriteLine("((( P1 Calculate: X = Q * m )))");
            pD.X = mF.MulScalVector(pD.m, pD.Q);

            // зупинка таймеру часу
            stopwatch.Stop();

            Console.WriteLine($"P1 end; N: {N}; Time: {stopwatch.ElapsedMilliseconds} ms");

            // розблоковуємо потік 2
            Console.WriteLine("<<<P1 UnBlocking P2>>>");
            pD.semaphore_P2.Release();
        }
    }
}