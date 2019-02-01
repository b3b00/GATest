using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GATest.Abstract;

namespace GATest.Generic
{

    public class Polynome : IComparable, Individual<List<PointD>>
    {

        public const int Max = 10;

        public List<Monome> Monomes { get; set; }

        public int Count => Monomes.Count;

        public double Fitness { get; set; }

        public Polynome()
        {
            Monomes = new List<Monome>();
        }

        public Polynome(List<Monome> monomes)
        {
            Monomes = monomes;
        }

        public static Polynome RandomPolynome()
        {
            return RandomPolynome(Max);
        }

        public static Polynome RandomPolynome(long count)
        {
            Polynome random = new Polynome();
            for (long i = 0; i < count; i++)
            {
                random.AddMonome(Monome.RandomMonome());
            }
            return random;
        }

        public void AddMonome(Monome monome)
        {
            Monomes.Add(monome);
        }

        public void AddMonomes(List<Monome> monomes)
        {
            Monomes.AddRange(monomes);
        }

        public Polynome Simplify()
        {
            Polynome simplified = new Polynome();
            var groups = Monomes.GroupBy(m => m.Exponent).ToList();
            foreach (var group in groups)
            {

                var monomes = group.ToList();
                if (monomes != null && monomes.Any())
                {
                    int exponent = monomes[0].Exponent;
                    int factor = monomes.Select(m => m.Factor).ToList<int>().Sum();
                    if (factor != 0)
                    {
                        simplified.AddMonome(new Monome(factor, exponent));
                    }
                }

                simplified.Monomes = simplified.Monomes.OrderBy(m => m.Exponent).ToList();

            }
            ;
            return simplified;
        }

        public Monome this[int index]
        {
            get
            {
                return Monomes[index];
            }
            set
            {
                Monomes[index] = value;
            }
        }

        public override string ToString()
        {
            return "y = " + Monomes.Select(m => m.ToString()).Aggregate((m1, m2) => m1.ToString() + " + " + m2.ToString());
        }


        public Fragment CloneFragment(Fragment frag)
        {
            Fragment fragment = new Fragment(frag.Start, frag.End);
            fragment.Monomes = new List<Monome>();
            for (int i = frag.Start; i <= frag.End; i++)
            {
                fragment.Monomes.Add(Monomes[i].Clone());
            }
            return fragment;
        }

        public void PasteFragment(Fragment fragment)
        {
            for (int i = 0; i < fragment.Monomes.Count; i++)
            {
                Monomes[i + fragment.Start] = fragment.Monomes[i];
            }
        }

        public Polynome Mutate()
        {

            Polynome mutated = Clone();
            int mutationsCount = Context.Random.Next(mutated.Monomes.Count);

            // compute mutations positions and mutate gene
            List<int> mutationPositions = new List<int>();
            for (int i = 0; i < mutationsCount; i++)
            {
                int position = Context.Random.Next(mutationsCount);
                while (mutationPositions.Contains(position))
                {
                    position = Context.Random.Next(mutationsCount);
                }
                mutated[position] = Monomes[i].Mutate();
            }
            return mutated;
        }

        public Individual<List<PointD>> Crossover(Individual<List<PointD>> polynome)
        {
            return Crossover((Polynome)polynome);
        }


        private Fragment RandomFragment(int max)
        {
            int start = Context.Random.Next(max);
            int end = Context.Random.Next(max);
            while (end < start)
            {
                end = Context.Random.Next(max);
            }
            return new Fragment(start, end);
        }


        public Polynome Crossover(Polynome polynome)
        {


            int sourceIndex = Context.Random.Next(1);
            Polynome source = sourceIndex == 0 ? this : polynome;
            Polynome dest = sourceIndex == 0 ? polynome : this;


            Polynome child = dest.Clone();

            int minLength = Math.Min(Count, polynome.Count);

            int crossingCount = Context.Random.Next(minLength / 2) + 1;

            List<Fragment> fragments = new List<Fragment>();

            for (int i = 0; i < crossingCount; i++)
            {
                Fragment fragment = RandomFragment(minLength);
                fragments.Add(fragment);
            }

            int n = 0;
            foreach (Fragment fragment in fragments)
            {
                Fragment frag = source.CloneFragment(fragment);
                child.PasteFragment(frag);
                n++;
            }

            return child;

        }

        Individual<List<PointD>> Individual<List<PointD>>.Mutate()
        {
            return Mutate();
        }

        public double ComputeFitness(List<PointD> points)
        {
            double fitness = 0.0;
            foreach (var point in points)
            {
                double t = Math.Abs(Compute(point.X) - point.Y);
                fitness += t;
            }
            Fitness = fitness;
            return fitness;
        }

        public double Compute(double x)
        {
            double result = 0.0;

            foreach(Monome monome in Monomes) {
                double monoResult = monome.Compute(x);
                result += monoResult;
            }

            return result;
        }


        public Polynome Clone()
        {
            Polynome clone = new Polynome();
            foreach (Monome mono in Monomes)
            {
                clone.AddMonome(new Monome(mono.Factor, mono.Exponent));
            }
            return clone;
        }

        public int CompareTo(object obj)
        {
            if (obj != null)
            {
                if (obj is Polynome poly)
                {
                    return Fitness.CompareTo(poly.Fitness);
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return 1;
            }
        }
    }
}
