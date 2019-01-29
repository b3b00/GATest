using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace GATest.model
{

    public class Polynome
    {

        public const int Max = 10;

        public List<Monome> Monomes { get; set; }

        public int Count => Monomes.Count;

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
            Polynome random = new Polynome();
            int monomeCount = Max;
            for (int i = 0; i < monomeCount; i++)
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

        public int CompareTo(Polynome polynome)
        {
            double f1 = this.Fitness(Context.Target);
            double f2 = polynome.Fitness(Context.Target);
            return f1.CompareTo(f2);
        }


        public override string ToString()
        {
            return Monomes.Select(m => m.ToString()).Aggregate((m1, m2) => m1.ToString() + " + " + m2.ToString());
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

            Console.WriteLine($"mutate {mutationsCount} times");

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
            crossingCount = 1;

            List<Fragment> fragments = new List<Fragment>();

            for (int i = 0; i < crossingCount; i++)
            {
                Fragment fragment = RandomFragment(minLength);
                fragments.Add(fragment);
                Console.WriteLine($"{fragment}");
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

        public double Fitness(List<PointF> points)
        {
            double fitness = points.Select(p => Math.Abs(Compute(p.X) - p.Y)).Aggregate((double y1, double y2) => y1 + y2);
            return 0;
        }

        public double Compute(double x)
        {
            double result = Monomes.Select((Monome m) => m.Factor * x).Aggregate((double d1, double d2) => d1 + d2);
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


    }
}
