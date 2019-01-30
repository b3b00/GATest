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

                List<PointF> target = new List<PointF>();



                Func<float, Func<float, float>> mono = (float pow) => (x) => (float)Math.Pow(x, pow);
                Func<float, float> pow5 = mono(5);
                Func<float, float> cos = (float x) => (float)Math.Cos(x);
                // y = x + 2x^2 +5x^5
                Func<float, float> poly = (float x) => x + 2 * mono(2)(x) + 5 * mono(5)(x);

                for (int i = 1; i <= 10; i++)
                {
                    target.Add(new PointF(i, poly(i)));
                }


                Population population = new Population();
                population.Evolve(target);
                var best = population.Best;
                foreach (var p in target)
                {
                    Console.WriteLine($"x={p.X} expected={p.Y} found={best.Compute(p.X)}");
                }
            }
            catch (Exception e)
            {
                ;
            }

        }
    }
}
