using System;

namespace GATest.Abstract
{
    public interface Individual<Target> : IComparable
    {
            double Fitness { get; set; }

            Individual<Target> Mutate();

            Individual<Target> Crossover(Individual<Target> polynome);

            double ComputeFitness(Target target);
    }
}
