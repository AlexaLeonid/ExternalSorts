using System;

namespace Lab4
{
    internal class Program
    {
        private static void Main(string[] args)
        {
         //   NaturalMerge naturalMerge = new NaturalMerge("../../../../test.txt", 1);
         //   naturalMerge.Sort();
            MultipathMerge multipathMerge = new MultipathMerge("../../../../test.txt", 1);
            multipathMerge.Sort();
         //   DirectMerge MynaturalMerge = new DirectMerge("../../../../test.txt", 1);
         //   MynaturalMerge.Sort();
        }
    }
}

