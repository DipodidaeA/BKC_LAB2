using System;
using System.Text;

namespace Data
{
    // клас математичних функцій
    static class MathFucn
    {
        // додає два вектори, повертає вектор
        public static int[] AddVector(int[] A, int[] B)
        {
            int N = A.Length;
            int[] R = new int[N];

            for (int i = 0; i < N; i++)
            {
                R[i] = A[i] + B[i];
            }

            return R;
        }

        // Множення вектора на матриць, повертає вектор
        public static int[] MulVecMatr(int[] A, int[,] B)
        {
            int N = A.Length;
            int[] R = new int[N];

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    R[i] += A[j] * B[j, i];
                }
            }

            return R;
        }

        // віднімання матриць, повертає матрицю
        public static int[,] SubMatr(int[,] A, int[,] B)
        {
            int N = A.GetLength(0);
            int[,] R = new int[N, N];

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    R[i, j] = A[i, j] - B[i, j];
                }
            }

            return R;
        }

        // множення матриць, повертає матрицю
        public static int[,] MulMatr(int[,] A, int[,] B)
        {
            int N = A.GetLength(0);
            int[,] R = new int[N, N];

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    for (int k = 0; k < N; k++)
                    {
                        R[i, j] += A[i, k] * B[k, j];
                    }
                }
            }

            return R;
        }

        // сортування рядків матриць, повертає відсортовану за рядками матрицю
        public static int[,] SortLineMatr(int[,] A)
        {
            int N = A.GetLength(0);
            // створення масив результат
            int[,] R = new int[N, N];

            for (int i = 0; i < N; i++)
            {
                // тимчасовий масив для радка що сортується
                int[] tempArr = new int[N];

                // Копіюємо елементи рядка матриці у тимчасовий масив
                for (int j = 0; j < N; j++)
                {
                    tempArr[j] = A[i, j];
                }

                // Сортуємо тимчасовий масив
                Array.Sort(tempArr);

                // Записуємо відсортовані елементи у масив результат
                for (int j = 0; j < N; j++)
                {
                    R[i, j] = tempArr[j];
                }
            }

            return R;
        }
    }

    // клас для заповенння векторів та матриць для функцій
    static class FillParam
    {
        // заповнення для функції 1
        public static void FillFunc1(int N, int num, int[] A, int[] B, int[] C, int[] D, int[,] MA, int[,] MD)
        {
            for (int i = 0; i < N; i++)
            {
                A[i] = num;
                B[i] = num;
                C[i] = num;
                D[i] = num;
                for (int j = 0; j < N; j++)
                {
                    MA[i, j] = num;
                    MD[i, j] = num;
                }
            }
        }

        // заповнення для функції 2
        public static void FillFunc2(int N, int num, int[,] MF, int[,] MH, int[,] MK)
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    MF[i, j] = num;
                    MH[i, j] = num;
                    MK[i, j] = num;
                }
            }
        }

        // випадкове заповнення для функції 2, для перевірки сортування
        /*
        public static void FillRandomFunc2(int N, int num, int[,] MF, int[,] MH, int[,] MK)
        {
            Random random = new Random();

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    MF[i, j] = random.Next(1, 10);
                    MH[i, j] = random.Next(1, 10);
                    MK[i, j] = random.Next(1, 10);
                }
            }
        }
        */

        // заповнення для функції 3
        public static void FillFunc3(int N, int num, int[] O, int[] P, int[] V, int[,] MR, int[,] MS)
        {
            for (int i = 0; i < N; i++)
            {
                O[i] = num;
                P[i] = num;
                V[i] = num;
                for (int j = 0; j < N; j++)
                {
                    MR[i, j] = num;
                    MS[i, j] = num;
                }
            }
        }
    }

    // клас для виводу векторів та матриць
    static class PrintText
    {
        // для виводу вектора
        public static string PrintVector(string NameV, int[] V)
        {
            StringBuilder result = new StringBuilder();
            result.Append(NameV + ": ");

            for (int i = 0; i < V.Length; i++)
            {
                result.Append(V[i]);
                result.Append(" ");
            }

            return result.ToString();
        }

        // для виводу матириці
        public static string PrintMATR(string NameMA, int[,] MA)
        {
            StringBuilder result = new StringBuilder();
            result.Append(NameMA + ": ");

            for (int i = 0; i < MA.GetLength(0); i++)
            {
                for (int j = 0; j < MA.GetLength(1); j++)
                {
                    result.Append(MA[i, j]);
                    result.Append(" ");
                }
                result.AppendLine();
            }

            return result.ToString();
        }
    }
}