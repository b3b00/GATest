using System;
using System.Linq;
using System.Collections.Generic;
using GATest.Abstract;

namespace GATest.Generic
{
    public class PolynomePopulation : Population<List<PointD>>
    {

        private readonly long PolynomeSize = 10;

        public PolynomePopulation() : base()
        {
            InitRandomPopulation(PopulationSize);
        }

        public PolynomePopulation(long populationSize, long maxGeneration, long polynomeSize) : base(populationSize,maxGeneration)
        {
            PolynomeSize = polynomeSize;
            PopulationSize = populationSize;
            InitRandomPopulation(populationSize);
        }

        public new Polynome Best => (Polynome)Individuals.First();

        public sealed override void InitRandomPopulation(long populationSize)
        {
            for (long i = 0; i < populationSize; i++)
            {
                var poly = Polynome.RandomPolynome(PolynomeSize);
                Individuals.Add(poly);
            }
        }


        public new  Polynome Evolve(List<PointD> target, long populationSize, long maxGeneration)
        {
            
            Polynome best =  (Polynome)base.Evolve(target, populationSize, maxGeneration);
            
            var simplified = best.Simplify();

            Console.WriteLine($"exit with fitness [{best.Fitness}] : {simplified}");

            return simplified;
        }



    }
}

