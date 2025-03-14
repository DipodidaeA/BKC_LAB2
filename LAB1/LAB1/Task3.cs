using Data;
using System;
using System.Diagnostics;
using System.Threading;

namespace Task
{
    static class T3
    {
        public static void Func3(int N, ProgData pD)
        {
            FillParam fP = new FillParam();
            PrintText pT = new PrintText();
            MathFucn mF = new MathFucn();

            // таймер часу
            Stopwatch stopwatch = new Stopwatch();
            // запуск таймеру часу
            stopwatch.Start();

            // 1) ЧЕКАЄ: введення даних B, MX в Т2	– W(3, 2)
            pD.sem1.WaitOne();

            // 2) ЧЕКАТИ: введення даних d, Z, MM в Т4   – W(3, 4)
            pD.sem2.WaitOne();

            // 3) обчислення 1. a3 = min(Bh)
            int a3 = mF.MinNumInVect(mF.GetPartVect(2, pD.H, pD.B));

            // 4) обчислення 2. a = min(a, a3) 
            pD.skd1.WaitOne();
                pD.a = mF.MinNum(pD.a, a3);  // – КД 1
            pD.skd1.Set();

            // 5) СИГНАЛИ: Т1,T2,T4 про обчислення  а	– S(1, 3), S(2, 3), S(4, 3)
            pD.sem5.Release(3);

            // 6) ЧЕКАТИ: обчислення a від Т1,Т2,Т4  	– W(3, 1), W(3, 2), W(3, 4)
            pD.sem3.WaitOne();
            pD.sem4.WaitOne();
            pD.sem6.WaitOne();

            // 7) копіюємо d у d3
            pD.skd2.WaitOne();
                int d3 = pD.d;          //  – КД 2
            pD.skd2.Set();

            // 8) обчислення 3. Rh = (d3 * Bh + Z * (MMн * MXh)) 
            int[] Rh = mF.AddVector(mF.MulScalVector(d3, mF.GetPartVect(2, pD.H, pD.B)), mF.MulVecMatr(pD.Z, mF.MulMatr(pD.MM, mF.GetPartMatrTab(2, pD.H, pD.MX))));

            // 9) обчислення 4. Qh = sort(Rh)
            int[] Q1hT3 = mF.SortVect(Rh);

            // 10) ЧЕКАТИ: обчислення Qh від Т4		– W(3, 4)
            pD.sem8.WaitOne();

            // 11) обчислення 5. Q2h = msort(Qh, Qh) 
            pD.Q2hT3 = mF.SortVectConcat(Q1hT3, pD.Q1hT4);

            // 12) СИГНАЛ: Т1 про обчислений Q2h		– S(1, 3)
            pD.sem9.Release();

            // 13) ЧЕКАТИ: обчислення Q від Т1 		– W(3, 1)
            pD.sem10.WaitOne();

            // копіюємо a у а3
            pD.skd3.WaitOne();
                a3 = pD.a;      // – КД 3
            pD.skd3.Set();

            // 15) обчислення 7. Xh = Qh * a3
            mF.ReturnPartVect(2, pD.H, pD.X, mF.MulScalVector(a3, mF.GetPartVect(2, pD.H, pD.Q)));

            // зупинка таймеру часу
            stopwatch.Stop();

            Console.WriteLine($"T3 end; H: {pD.H}; Time: {stopwatch.ElapsedMilliseconds} ms");

            // 16) СИГНАЛ: Т4 про закінчення обчислення	 	– S(4, 3)
            pD.sem11.Release();
        }
    }
}