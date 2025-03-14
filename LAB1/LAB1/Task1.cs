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

            // 1) ЧЕКАЄ: введення даних B, MX в Т2	– W(1, 2)
            pD.sem1.WaitOne();

            // 2) ЧЕКАТИ: введення даних d, Z, MM в Т4   – W(1, 4)
            pD.sem2.WaitOne();

            // 3) обчислення 1.  a1 = min(Bh)
            int ai = mF.MinNumInVect(mF.GetPartVect(0, pD.H, pD.B));

            // якщо а хтось вже використовує, чекаємо
            pD.skd1.WaitOne();

            // 4) обчислення 2.  a = min(a, a1)   – КД 1
            pD.a = mF.MinNum(pD.a, ai);

            // збільшуємо кількість потоків що обчилили а
            pD.c++;

            // 5) СИГНАЛИ: Т2-T4 про обчислення  а	– S(2, 1), S(3, 1), S(4, 1)
            pD.skd1.Release();

            // перевіряємо чи всі потоки обчислили а
            pD.isCheckCalc(4, pD.sem3);

            // 6) ЧЕКАТИ: обчислення а  від Т2-Т4  – W(1, 2), W(1, 3), W(1, 4)
            pD.sem3.WaitOne();

            // якщо d хтось вже використовує, чекаємо
            pD.skd2.WaitOne();

            // 7) копіювати d1 = d	 – КД 2
            int d1 = pD.d; 

            // дозволяємо використовувати d
            pD.skd2.Release();

            // 8) обчислення 3. Rh = (d1 * Bh + Z * (MMн * MXh))
            int[] Rh = mF.AddVector(mF.MulScalVector(d1, mF.GetPartVect(0, pD.H, pD.B)), mF.MulVecMatr(pD.Z, mF.MulMatr(pD.MM, mF.GetPartMatrTab(0, pD.H, pD.MX))));

            // 9) обчислення 4. Q1h = sort(Rh)
            int[] Q1hT1 = mF.SortVect(Rh);

            // 10) ЧЕКАТИ: обчислення Q1h від Т2    – W(1, 2)
            pD.sem4.WaitOne();

            // 11) обчислення 5. Q2h = msort(Q1h, Q1h)  
            int[] Q2hT1 = mF.SortVectConcat(Q1hT1, pD.Q1hT2);

            // 12) ЧЕКАТИ: обчислення Q2h від Т3	– W(1, 3)
            pD.sem5.WaitOne();

            // 13) обчислення 6. Q = msort(Q2h, Q2h)
            pD.Q = mF.SortVectConcat(Q2hT1, pD.Q2hT3);

            // 14) СИГНАЛИ: Т2-Т4 про обчислення Q
            pD.sem6.Release(3);

            // якщо a хтось вже використовує, чекаємо
            pD.skd3.WaitOne();

            // 15) копіювати а1 = a		– КД 3
            ai = pD.a;

            // дозволяємо використовувати a
            pD.skd3.Release();

            // 16) обчислення 7. Xh = Qh * a1
            mF.ReturnPartVect(0, pD.H, pD.X, mF.MulScalVector(ai, mF.GetPartVect(0, pD.H, pD.Q)));

            // зупинка таймеру часу
            stopwatch.Stop();

            Console.WriteLine($"T1 end; H: {pD.H}; Time: {stopwatch.ElapsedMilliseconds} ms");

            // 17) СИГНАЛ: Т4 про Т1 закінчив обчислення	– S(4, 1)
            pD.c++;
            pD.isCheckCalc(3, pD.sem7);
        }
    }
}