using System;
using System.Collections.Generic;
using GATest.Generic;
using GATest.model;
using Context = GATest.model.Context;
using Polynome = GATest.model.Polynome;

namespace GATest
{

    
    class Program
    {
        static void Main(string[] args)
        {
//            InitialTest();
            GenericTest();
        }

        private static void InitialTest()
        {
            try
            {
                int popSize = 5000;
                int polySize = 5;
                int generationMax = 100;

                Context.Random = new Random();

                var po = Polynome.RandomPolynome();
                Console.WriteLine(po);
                var rr = po.Compute(1);

                List<PointD> target = new List<PointD>();


                Func<double, Func<double, double>> power = (double pow) => (x) => (double) Math.Pow(x, pow);
                Func<double, double> pow5 = power(5);
                Func<double, double> pow2 = power(2);
                Func<double, double> cos = (double x) => (double) Math.Cos(x);
                // y = x + 2x^2 +5x^5
                Func<double, double> poly = (double x) => x + 2 * pow2(x) + 5 * pow5(x);

                Polynome targetPoly = Polynome.RandomPolynome(polySize).Simplify();
                Console.WriteLine($"looking for {targetPoly}");


                Func<double, double> polypoly = (double x) => (double) targetPoly.Compute(x);

                for (int x = 1; x <= 10; x++)
                {
                    target.Add(new PointD(x, polypoly(x)));
                }


                Population population = new Population(popSize, polySize);

                var best = population.Evolve(target, popSize, generationMax);
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
        
        private static void GenericTest()
        {
            try
            {
                int popSize = 5000;
                int polySize = 5;
                int maxGeneration = 100;

                GATest.Generic.Context.Random = new Random();

                var po = Polynome.RandomPolynome();
                Console.WriteLine(po);
                var rr = po.Compute(1);

                List<PointD> target = new List<PointD>();


                Func<double, Func<double, double>> power = (double pow) => (x) => (double) Math.Pow(x, pow);
                Func<double, double> pow5 = power(5);
                Func<double, double> pow2 = power(2);
                Func<double, double> cos = (double x) => (double) Math.Cos(x);
                // y = x + 2x^2 +5x^5
                Func<double, double> poly = (double x) => x + 2 * pow2(x) + 5 * pow5(x);

                Polynome targetPoly = Polynome.RandomPolynome(polySize).Simplify();
                Console.WriteLine($"looking for {targetPoly}");


                Func<double, double> polypoly = (double x) => (double) targetPoly.Compute(x);

                for (int x = 1; x <= 10; x++)
                {
                    target.Add(new PointD(x, polypoly(x)));
                }


                PolynomePopulation population = new PolynomePopulation(popSize, maxGeneration,polySize);

                GATest.Generic.Polynome best = population.Evolve(target, popSize, maxGeneration);
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
