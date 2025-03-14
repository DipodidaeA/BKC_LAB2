using Data;
using System;
using System.Diagnostics;
using System.Threading;

namespace Task
{
    static class T1
    {
        public static void Func1(int N, ProgData pD)
        {
            FillParam fP = new FillParam();
            PrintText pT = new PrintText();
            MathFucn mF = new MathFucn();

            // таймер часу
            Stopwatch stopwatch = new Stopwatch();
            // запуск таймеру часу
            stopwatch.Start();

            // 1) ЧЕКАТИ: введення даних B, MX в Т2		– W(1, 2)
            pD.sem1.WaitOne();

            // 2) ЧЕКАТИ: введення даних d, Z, MM  	в Т4      	– W(1, 4)
            pD.sem2.WaitOne();

            // 3) обчислення 1.  a1 = min(Bh)
            int a1 = mF.MinNumInVect(mF.GetPartVect(0, pD.H, pD.B));

            // 4) обчислення 2.  a = min(a, a1)
            pD.skd1.WaitOne();
                pD.a = mF.MinNum(pD.a, a1); // – КД 1
            pD.skd1.Set();

            // 5) СИГНАЛИ: Т2-T4 про обчислення  а	– S(2, 1), S(3, 1), S(4, 1)
            pD.sem3.Release(3);


            // 6) ЧЕКАТИ: обчислення  а  від Т2-Т4      – W(1, 2), W(1, 3), W(1, 4)
            pD.sem4.WaitOne();
            pD.sem5.WaitOne();
            pD.sem6.WaitOne();

            // 7) копіюємо d у d1
            pD.skd2.WaitOne();
                int d1 = pD.d;  //  – КД 2
            pD.skd2.Set();

            // 8) обчислення 3. Rh = (d1 * Bh + Z * (MMн *  MXh)) 
            int[] Rh = mF.AddVector(mF.MulScalVector(d1, mF.GetPartVect(0, pD.H, pD.B)), mF.MulVecMatr(pD.Z, mF.MulMatr(pD.MM, mF.GetPartMatrTab(0, pD.H, pD.MX))));

            // 9) обчислення 4. Qh = sort(Rh)
            int[] Q1hT1 = mF.SortVect(Rh);

            // 10) ЧЕКАТИ: обчислення Qh від Т2		– W(1, 2)
            pD.sem7.WaitOne();

            // 11) обчислення 5. Q2h = msort(Qh, Qh)  
            int[] Q2hT1 = mF.SortVectConcat(Q1hT1, pD.Q1hT2);

            // 12) ЧЕКАТИ: обчислення Q2h від Т3			– W(1, 3)
            pD.sem9.WaitOne();

            // 13) обчислення 6. Q = msort(Q2h, Q2h)
            pD.Q = mF.SortVectConcat(Q2hT1, pD.Q2hT3);

            // 14) СИГНАЛИ: Т2-Т4 про обчислення Q
            pD.sem10.Release(3);

            // 15) копіюємо а y a1	
            pD.skd3.WaitOne();
                a1 = pD.a;  // – КД 3
            pD.skd3.Set();

            // 16) обчислення 7. Xh = Qh * a1
            mF.ReturnPartVect(0, pD.H, pD.X, mF.MulScalVector(a1, mF.GetPartVect(0, pD.H, pD.Q)));

            // зупинка таймеру часу
            stopwatch.Stop();

            Console.WriteLine($"T1 end; H: {pD.H}; Time: {stopwatch.ElapsedMilliseconds} ms");

            // 17) СИГНАЛ: Т4 про закінчення обчислення	   – S(4, 1)
            pD.sem11.Release();
        }
    }
}