using System;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;

namespace GATest.model
{
    public class Population
    {

        public const int MaxGeneration = 1000;

        public const int PopulationSize = 500;

        public List<Polynome> Polynomes { get; set; }

        public Population()
        {
            InitRandomPopulation(PopulationSize);
        }

        public Polynome Best => Polynomes.First();

        public void InitRandomPopulation(int populationSize)
        {
            Polynomes = new List<Polynome>();
            for (int i = 0; i < populationSize; i++)
            {
                Polynomes.Add(Polynome.RandomPolynome());
            }
        }

        public void Evolve(List<PointF> target)
        {
            Context.Target = target;
            int gen = 0;
            double fit = double.MaxValue;
            Polynomes.ForEach(p => p.ComputeFitness(target));
            Polynomes.Sort();
            fit = Polynomes.Last().ComputeFitness(target);
            while (gen < MaxGeneration && fit > 0.1)
            {
                // TODO : mutate polynomes
                foreach (Polynome poly in Polynomes)
                {
                    poly.Mutate();
                    poly.ComputeFitness(target);
                }
                Polynomes.Sort();
                // TODO cross first polynomes
                for (int i = 0; i < PopulationSize / 2; i++)
                {
                    var firstParent = Polynomes[2*i];
                    var secondParent = Polynomes[2*i+1];
                    var child = firstParent.Crossover(secondParent);
                    child.ComputeFitness(target);
                    
                    if (child.CompareTo(secondParent) <= 0)
                    {
                        //Console.WriteLine($"child beats parent {child.Fitness} <= {secondParent.Fitness}");
                        Polynomes[2*i+1] = child;
                    }
                }
                Polynomes.ForEach(p => p.ComputeFitness(target));
                Polynomes.Sort();
                fit = Polynomes.First().Fitness;
                var fits = Polynomes.Select(p => p.Fitness).ToList();
                Console.WriteLine($"generation #{gen} : fitness={fit}");
                var ffstr = fits.Select(f => f.ToString()).ToList();
                var ff = ffstr.Aggregate((string f1, string f2) => f1 + ", " + f2);
                // Console.WriteLine($"[ {ff} ]");
                //.ToList().Aggregate((double f1, double f2) => f1+", "+f2);
                gen++;
            }
            var best = Polynomes.First();
            Console.WriteLine(best);
            var simplified = best.Simplify();

            Console.WriteLine($"exit with fitness [{Polynomes.First().Fitness}] : {simplified}");
            ;
        }

    }
}

