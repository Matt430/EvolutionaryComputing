using System.Collections.Generic;

namespace Assignment1
{
    abstract class FitnessFunction
    {
        //The fitness function, to be implemented for every class that inherits from this.
        public abstract float Fitness(List<bool> bitstring);

        //A comparison of two bitstrings, to be used when sorting populations.
        public int FitnessCompare(List<bool> bitstring1, List<bool> bitstring2)
        {
            if(Fitness(bitstring1) == Fitness(bitstring2))
            {
                return 0;
            }
            if(Fitness(bitstring1) < Fitness(bitstring2))
            {
                return -1;
            }
            return 1;
        }
    }
}
