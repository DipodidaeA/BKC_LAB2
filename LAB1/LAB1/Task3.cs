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

            // 3) обчислення 1.  a1 = min(Bh)
            int ai = mF.MinNumInVect(mF.GetPartVect(2, pD.H, pD.B));

            // якщо а хтось вже використовує, чекаємо
            pD.skd1.WaitOne();

            // 4) обчислення 2.  a = min(a, a1)   – КД 1
            pD.a = mF.MinNum(pD.a, ai);

            // збільшуємо кількість потоків що обчилили а
            pD.c++;

            // 5) СИГНАЛИ: Т1,T2,T4 про обчислення  а	– S(1, 3), S(2, 3), S(4, 3)
            pD.skd1.Release();

            // перевіряємо чи всі потоки обчислили а
            pD.isCheckCalc(4, pD.sem3);

            // 6) ЧЕКАТИ: обчислення a від Т1,Т2,Т4  	– W(3, 1), W(3, 2), W(3, 4)
            pD.sem3.WaitOne();

            // якщо d хтось вже використовує, чекаємо
            pD.skd2.WaitOne();

            // 7) копіювати d1 = d	 – КД 2
            int d1 = pD.d;

            // дозволяємо використовувати d
            pD.skd2.Release();

            // 8) обчислення 3. Rh = (d1 * Bh + Z * (MMн * MXh))
            int[] Rh = mF.AddVector(mF.MulScalVector(d1, mF.GetPartVect(2, pD.H, pD.B)), mF.MulVecMatr(pD.Z, mF.MulMatr(pD.MM, mF.GetPartMatrTab(2, pD.H, pD.MX))));

            // 9) обчислення 4. Q1h = sort(Rh)
            int[] Q1hT3 = mF.SortVect(Rh);

            // 10) ЧЕКАТИ: обчислення Q1h від Т4	– W(3, 4)
            pD.sem8.WaitOne();

            // 11) обчислення 5. Q2h = msort(Q1h, Q1h) 
            pD.Q2hT3 = mF.SortVectConcat(Q1hT3, pD.Q1hT4);

            // 12) СИГНАЛ: Q2h обчислений для Т1 	– S(1, 3)
            pD.sem5.Release();

            // 13) ЧЕКАТИ: обчислення Q від Т1 		– W(3, 1)
            pD.sem6.WaitOne();

            // якщо a хтось вже використовує, чекаємо
            pD.skd3.WaitOne();

            // 14) копіювати а1 = a		– КД 3
            ai = pD.a;

            // дозволяємо використовувати a
            pD.skd3.Release();

            // 15) обчислення 7. Xh = Qh * a1
            mF.ReturnPartVect(2, pD.H, pD.X, mF.MulScalVector(ai, mF.GetPartVect(2, pD.H, pD.Q)));

            // зупинка таймеру часу
            stopwatch.Stop();

            Console.WriteLine($"T3 end; H: {pD.H}; Time: {stopwatch.ElapsedMilliseconds} ms");

            // 16) СИГНАЛ: Т4 про Т3 закінчив обчислення	– S(4, 3)
            pD.c++;
            pD.isCheckCalc(3, pD.sem7);
        }
    }
}