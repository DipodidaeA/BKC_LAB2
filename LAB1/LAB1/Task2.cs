using Data;
using System;
using System.Diagnostics;
using System.Threading;

namespace Task
{
    static class F2
    {
        //ПОТІК 1:              ||B, MX || K = d * B    || Y = Z * MK || Q = K + Y || Q = sort(Q) || X = Q * m  ||
        //ПОТІК 2: MM, X, Z, d  ||      || MK = MM * MX || m = min(B) ||                                        || Console(X)
        public static void Func2(int N, ProgData pD)
        {
            FillParam fP = new FillParam();
            PrintText pT = new PrintText();
            MathFucn mF = new MathFucn();
            // таймер часу
            Stopwatch stopwatch = new Stopwatch();

            //чекає дозволу на роботу
            Console.WriteLine("<<<P2 Blocked>>>");
            pD.semaphore_P2.WaitOne();
            Console.WriteLine("<<<Program UnBlocked P2>>>");

            // якщо N менше рівне 4, вводимо з клавіатури
            Console.WriteLine("Func2. Input num...");
            if (N <= 4)
            {
                fP.FillMatr("MM", pD.MM);
                fP.FillVect("Z", pD.Z);
                fP.FillNum("d", pD.d);

                // запуст таймера часу
                stopwatch.Start();
            }
            else
            {
                // запуст таймера часу
                stopwatch.Start();

                fP.FillMatrRand(pD.MM);
                fP.FillVectRand(pD.Z);
                fP.FillNumRand(pD.d);
            }

            // розблоковує потік 1
            Console.WriteLine("<<<P2 UnBlocking P1>>>");
            pD.semaphore_P1.Release();

            //чекає дозволу на роботу від потоку 1
            Console.WriteLine("<<<P2 Blocked>>>");
            pD.semaphore_P2.WaitOne();
            Console.WriteLine("<<<P2 UnBlocked>>>");

            // MK = MM * MX
            Console.WriteLine("((( P2 Calculate: MK = MM * MX )))");
            pD.MK = mF.MulMatr(pD.MM, pD.MX);

            // розблоковує потік 1
            Console.WriteLine("<<<P2 UnBlocking P1>>>");
            pD.semaphore_P1.Release();

            // m = min(B)
            Console.WriteLine("((( P2 Calculate: m = min(B) )))");
            pD.m = mF.MinNumInVect(pD.B);

            // розблоковує потік 1
            Console.WriteLine("<<<P2 UnBlocking P1>>>");
            pD.semaphore_P1.Release();

            // зупинка таймеру часу
            stopwatch.Stop();

            //чекає дозволу на роботу від потоку 1
            Console.WriteLine("<<<P2 Blocked>>>");
            pD.semaphore_P2.WaitOne();
            Console.WriteLine("<<<P2 UnBlocked>>>");

            // якщо N менше рівне 4, виводимо результат обчислення
            if (N <= 4)
            {
                Console.Write($"P2 end; N: {N}; Time: {stopwatch.ElapsedMilliseconds} ms; Res: ");
                pT.PrintVect("X", pD.X);
            }
            else
            {
                Console.WriteLine($"P2 end; N: {N}; Time: {stopwatch.ElapsedMilliseconds} ms");
            }
        }
    }
}