// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using Spreads;
using Microsoft;

unsafe
{
    Stopwatch sw1 = new Stopwatch();
    Stopwatch sw2 = new Stopwatch();
    Stopwatch sw3 = new Stopwatch();
    const int m = 2048;//2048
    long complexity = Convert.ToInt64(2 * Math.Pow(m, 3));

    Random rnd = new Random();
    double[,] matrix1 = new double[m, m];
    double[,] matrix2 = new double[m, m];
    double[,] matrixTrans = new double[m, m];
    double[,] matrixResult1 = new double[m, m];
    double[,] matrixResult2 = new double[m, m];
    double[,] matrixResult3 = new double[m, m];
    double alpha = 1.0;
    double beta = 0.0;


    
    for (int i = 0; i < m; i++)
    {
        for (int j = 0; j < m; j++)
        {
            matrix1[i, j] = rnd.NextDouble() * 10;
            matrix2[i, j] = rnd.NextDouble() * 10;
            matrixTrans[i, j] = matrix2[i, j];
        }
    }

    Console.ReadKey();

    //Способ из линейной алгебры - слишком долго(10000 лет выполнения)
    //sw1.Start();
    //for (int i = 0; i < m; i++)
    //    for (int j = 0; j < m; j++)
    //    {
    //        //matrixResult1[i, j] = 0.0;
    //        for (int k = 0; k < m; k++)
    //        {
    //            matrixResult1[i, j] += matrix1[i, k] * matrix2[k, j];

    //        }

    //    }
    //sw1.Stop();

    fixed (double* matrixA = new double[m * m], matrixB = new double[m * m], matrixC = new double[m * m])
    {
        int count = 0;
        int c = 0;
        double tmp;

        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < m; j++)
            {
                matrixA[count] = matrix1[i, j];
                matrixB[count] = matrix2[i, j];
                count++;
            }
        }

        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < i; j++)
            {
                tmp = matrixTrans[i, j];
                matrixTrans[i, j] = matrixTrans[j, i];
                matrixTrans[j, i] = tmp;
            }
        }

        sw2.Start();
        BLAS.CBLAS.Dgemm(BLAS.LAYOUT.RowMajor, BLAS.TRANSPOSE.NoTrans, BLAS.TRANSPOSE.NoTrans, m, m, m, alpha, matrixA, m, matrixB, m, beta, matrixC, m);
        sw2.Stop();

        //int leap = 0;

        //for (int i = 0; i < m; i++)
        //{
        //    for (int j = 0; j < m; j++)
        //    {
        //        Console.WriteLine(matrixResult1[i, j] + "   " + matrixC[leap] + "\n");
        //        leap++;
        //    }
        //}

        sw3.Start();

        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < m; j++)
            {
                for (int k = 0; k < m; k++) matrixResult3[i, j] += matrix1[i, k] *  matrixTrans[j, k];
            }
        }
        sw3.Stop();
    }

    TimeSpan ts1 = sw1.Elapsed;
    TimeSpan ts2 = sw2.Elapsed;
    TimeSpan ts3 = sw3.Elapsed;
    double t1 = ts1.Milliseconds + ts1.Seconds * 1000 + ts1.Minutes * 60000;
    double t2 = ts2.Milliseconds + ts2.Seconds * 1000 + ts2.Minutes * 60000;
    double t3 = ts3.Milliseconds + ts3.Seconds * 1000 + ts3.Minutes * 60000;
    double performance1 = complexity / t1 * Math.Pow(10, -3);
    double performance2 = complexity / t2 * Math.Pow(10, -3);
    double performance3 = complexity / t3 * Math.Pow(10, -3);
    Console.WriteLine($"Сложность задачи: {complexity}");
    Console.WriteLine($"Время по формуле из линейной алгебры: {ts1}");
    Console.WriteLine($"Время по результату работы библиотеки BLAS: {ts2}");
    Console.WriteLine($"Время по результату работы собственного алгоритма: {ts3}");
    Console.WriteLine($"Производительность первого варианта: {performance1.ToString()}");
    Console.WriteLine($"Производительность второго варианта: {performance2.ToString()}");
    Console.WriteLine($"Производительность третьего варианта: {performance3.ToString()}");
    Console.WriteLine("Работу выполнил: Блохин Валентин Владимирович\nГруппа: РПИа-о21");
    Console.ReadKey();
}
