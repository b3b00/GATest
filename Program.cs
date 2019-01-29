using System;
using GATest.model;

namespace GATest
{
    class Program
    {
        static void Main(string[] args)
        {
            Context.Random = new Random();

            Polynome p1 = Polynome.RandomPolynome();
            Console.WriteLine(p1);
            Console.WriteLine($"p1(1) = {p1.Compute(1.0)}");
            Polynome mp = p1.Mutate();
            Console.WriteLine(mp);
            Console.WriteLine($"mp(1) = {mp.Compute(1.0)}");
            Console.WriteLine();
            Polynome p2 = Polynome.RandomPolynome();
            Console.WriteLine($"p2(1) = {p2.Compute(1.0)}");
            Console.WriteLine(mp);
            Console.WriteLine(p2);
            Polynome cp = mp.Crossover(p2);            
            Console.WriteLine(cp);
            Console.WriteLine($"cp(1) = {cp.Compute(1.0)}");


        }
    }
}
