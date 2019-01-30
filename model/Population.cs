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


        public Polynome Evolve(List<PointD> target)
        {
            return Evolve(target, PopulationSize, MaxGeneration);
        }

        public Polynome Evolve(List<PointD> target, int populationSize, int maxGeneration)
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
                    var firstParent = Polynomes[2 * i];
                    var secondParent = Polynomes[2 * i + 1];
                    var child = firstParent.Crossover(secondParent);
                    child.ComputeFitness(target);

                    if (child.CompareTo(secondParent) <= 0)
                    {
                        Polynomes[2 * i + 1] = child;
                    }
                }
                Polynomes.ForEach(p => p.ComputeFitness(target));
                Polynomes.Sort();

                if (gen % 100 == 0)
                {
                    fit = Polynomes.First().Fitness;
                    var fits = Polynomes.Select(p => p.Fitness).ToList();
                    double averageFit = fits.Average();
                    Console.WriteLine($"generation #{gen} : best={fit}, average={averageFit}");
                }
                gen++;
            }
            var best = Polynomes.First();
            Console.WriteLine(best);
            var simplified = best.Simplify();

            Console.WriteLine($"exit with fitness [{Polynomes.First().Fitness}] : {simplified}");

            return simplified;
        }

    }
}

