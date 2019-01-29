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
            Context.Random = new Random();

            Polynome p1 = Polynome.RandomPolynome();
            // Console.WriteLine(p1);
            // Console.WriteLine($"p1(1) = {p1.Compute(1.0)}");
            Polynome mp = p1.Mutate();
            // Console.WriteLine(mp);
            // Console.WriteLine($"mp(1) = {mp.Compute(1.0)}");
            // Console.WriteLine();
            Polynome p2 = Polynome.RandomPolynome();
            // Console.WriteLine($"p2(1) = {p2.Compute(1.0)}");
            // Console.WriteLine(mp);
            // Console.WriteLine(p2);
            Polynome cp = mp.Crossover(p2);            
            // Console.WriteLine(cp);
            // Console.WriteLine($"cp(1) = {cp.Compute(1.0)}");


            List<PointF> target = new List<PointF>() {
                new PointF(1,1),
                new PointF(2,4),
                new PointF(3,9),
                new PointF(4,16),
                new PointF(5,25),
                new PointF(6,36),
                new PointF(7,49)
            };
            Population population = new Population();
            population.Evolve(target);
            var best = population.Best;
            foreach(var p in target) {
                Console.WriteLine($"x={p.X} expected={p.Y} found={best.Compute(p.X)}");
            }


        }
    }
}
