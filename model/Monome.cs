using System;

namespace GATest.model
{
    public class Monome
    {

        public const int Max = 10;

        public int Factor;
        public int Exponent;

        public Monome(int factor, int exponent)
        {
            Factor = factor;
            Exponent = exponent;
        }

        public static Monome RandomMonome()
        {
            Random rnd = new Random();
            int factor = rnd.Next(Max);
            int exponent = rnd.Next(Max);
            return new Monome(factor, exponent);
        }



        public Monome Clone()
        {
            return new Monome(Factor, Exponent);
        }

        public override string ToString()
        {
            return $"{Factor} x ^{Exponent}";
        }

        public Monome Mutate()
        {
            Monome mutated = null;
            if (Context.Random.Next(1) == 1)
            {
                int factor = Factor;
                if (Context.Random.Next(1) == 1)
                {
                    factor = Context.Random.Next(Max);
                }
                int exponent = Exponent;
                if (Context.Random.Next(1) == 1)
                {
                    exponent = Context.Random.Next(Max);
                }
                mutated = new Monome(factor, exponent);
                Console.WriteLine($"mutationt {this} -> {mutated}");

            }
            else {
                mutated = Clone();
            }
            return mutated;
        }

    }
}
