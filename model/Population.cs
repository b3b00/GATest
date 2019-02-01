using System;
using System.Linq;
using System.Collections.Generic;

namespace GATest.model
{
    public class Population
    {

        private int MaxGeneration { get; set; } = 1000;

        private int PopulationSize { get; set; } = 500;

        public List<Polynome> Polynomes { get; set; }

        public Population()
        {
            InitRandomPopulation(PopulationSize, Polynome.Max);
        }

        public Population(int populationSize, int polynomeSize)
        {
            PopulationSize = populationSize;
            InitRandomPopulation(populationSize, polynomeSize);
        }

        public Polynome Best => Polynomes.First();

        public void InitRandomPopulation(int populationSize, int polynomeSize)
        {
            Polynomes = new List<Polynome>();
            for (int i = 0; i < populationSize; i++)
            {
                var poly = Polynome.RandomPolynome(polynomeSize);
                Polynomes.Add(poly);
            }
        }


        public Polynome Evolve(List<PointD> target)
        {
            return Evolve(target, PopulationSize, MaxGeneration);
        }


        public List<Tuple<double, double>> fitnesses = new List<Tuple<double, double>>();
        public Polynome Evolve(List<PointD> target, int populationSize, int maxGeneration)
        {
            List<Polynome> newGeneration = new List<Polynome>();

            int step = maxGeneration / 10;
            Context.Target = target;
            int gen = 0;
            double fit = double.MaxValue;
            Polynomes.ForEach(p => p.ComputeFitness(target));
            Polynomes.Sort();
            fit = Polynomes.First().ComputeFitness(target);
            while (gen < maxGeneration && fit > 0)
            {
                // TODO : mutate polynomes
                foreach (var poly in Polynomes)
                {
                    poly.Mutate();
                    poly.ComputeFitness(target);
                }
                Polynomes.Sort();
                // TODO cross first polynomes
                for (var i = 0; i < PopulationSize / 2; i++)
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

                if (gen % step == 0)
                {
                    Console.WriteLine();
                    fit = Polynomes.First().Fitness;
                    var fits = Polynomes.Select(p => p.Fitness).ToList();
                    double averageFit = fits.Average();
                    Console.WriteLine($"generation #{gen} : best={fit,0:0,00.0}, average={averageFit,0:0,00.0}");

                    if (fitnesses.Count > 3)
                    {

                        var previous = fitnesses.Last();
                        var prevpreious = fitnesses[fitnesses.Count - 2];

                        if (previous.Item2 == prevpreious.Item2 && averageFit == previous.Item2)
                        {
                            ;
                        }

                    }

                    fitnesses.Add(new Tuple<double, double>(fit, averageFit));

                }
                gen++;
            }
            var best = Polynomes.First();
            var simplified = best.Simplify();

            Console.WriteLine($"exit with fitness [{Polynomes.First().Fitness}] : {simplified}");

            return simplified;
        }

    }
}

