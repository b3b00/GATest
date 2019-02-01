using System;
using System.Linq;
using System.Collections.Generic;

namespace GATest.Abstract
{
    public abstract class Population<Target> 
    {

        protected double TOLERANCE { get; set;  } = 0.0001;

        protected long MaxGeneration { get; set; } = 1000;

        protected long PopulationSize { get; set; } = 500;

        public List<Individual<Target>> Individuals { get; set; }
        
         

        public Population() 
        {
            InitRandomPopulation(PopulationSize);
        }

        public Population(long populationSize, long maxGeneration)
        {
            PopulationSize = populationSize;
            MaxGeneration = maxGeneration;
            InitRandomPopulation(populationSize);
        }

        public virtual Individual<Target> Best => Individuals.First();

        public abstract void InitRandomPopulation(long populationSize);


        public Individual<Target> Evolve(Target target)
        {
            return Evolve(target, PopulationSize, MaxGeneration);
        }


        public List<Tuple<double, double>> fitnesses = new List<Tuple<double, double>>();

       

        public Individual<Target> Evolve(Target target, long populationSize, long maxGeneration)
        {
            List<Individual<Target>> newGeneration = new List<Individual<Target>>();

            long step = maxGeneration / 10;
            long gen = 0;
            double fit = double.MaxValue;
            Individual<Target> ind = Individuals[0];
            //ind.
            Individuals.ForEach( p => p.ComputeFitness(target));
            Individuals.Sort();
            fit = Best.ComputeFitness(target);
            while (gen < maxGeneration && fit > 0)
            {
                // TODO : mutate polynomes
                foreach (var poly in Individuals)
                {
                    poly.Mutate();
                    poly.ComputeFitness(target);
                }
                Individuals.Sort();
                // TODO cross first polynomes
                for (var i = 0; i < PopulationSize / 2; i++)
                {
                    var firstParent = Individuals[2 * i];
                    var secondParent = Individuals[2 * i + 1];
                    var child = firstParent.Crossover(secondParent);
                    child.ComputeFitness(target);

                    if (child.CompareTo(secondParent) <= 0)
                    {
                        Individuals[2 * i + 1] = child;
                    }
                }
                Individuals.ForEach(p => p.ComputeFitness(target));
                Individuals.Sort();

                if (gen % step == 0)
                {
                    Console.WriteLine();
                    fit = Best.Fitness;
                    var fits = Individuals.Select(p => p.Fitness).ToList();
                    double averageFit = fits.Average();
                    Console.WriteLine($"generation #{gen} : best={fit,0:0,00.0}, average={averageFit,0:0,00.0}");

                    if (fitnesses.Count > 3)
                    {

                        var previous = fitnesses.Last();
                        var prevpreious = fitnesses[fitnesses.Count - 2];

                        if (Math.Abs(previous.Item2 - prevpreious.Item2) < TOLERANCE && averageFit == previous.Item2)
                        {
                            ;
                        }

                    }

                    fitnesses.Add(new Tuple<double, double>(fit, averageFit));

                }
                gen++;
            }

            return Best;

        }

        
    }
}

