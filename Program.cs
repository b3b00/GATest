using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using GATest.model;

namespace GATest
{

    
    class Program
    {
        static void Main(string[] args)
        {

            try
            {

                Context.Random = new Random();

                List<PointD> target = new List<PointD>();



                Func<double, Func<double, double>> power = (double pow) => (x) => (double)Math.Pow(x, pow);
                Func<double, double> pow5 = power(5);
                Func<double, double> pow2 = power(2);
                Func<double, double> cos = (double x) => (double)Math.Cos(x);
                // y = x + 2x^2 +5x^5
                Func<double, double> poly = (double x) => x + 2 * pow2(x) + 5 * pow5(x);

                Polynome targetPoly = Polynome.RandomPolynome(10).Simplify();
                Console.WriteLine($"looking for {targetPoly}");
                

                Func<double,double> polypoly = (double x) => (double)targetPoly.Compute(x);

                for (int i = 1; i <= 10; i++)
                {
                    target.Add(new PointD(i, polypoly(i)));
                }


                Population population = new Population();
                
                var best = population.Evolve(target,50000,1000);
                Console.WriteLine($"lokked for : {targetPoly}");
                foreach (var p in target)
                {
                    Console.WriteLine($"x={p.X} expected={p.Y} found={best.Compute(p.X)}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message} : \n {e.StackTrace}");
                ;
            }

        }
    }
}
