using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1
{
    abstract class FitnessFunction
    {
        //The fitness function, to be implemented for every class that inherits from this.
        public abstract int Fitness(List<bool> bitstring);

        //A comparison of two bitstrings, to be used when sorting populations.
        public int FitnessCompare(List<bool> bitstring1, List<bool> bitstring2)
        {
            return (Fitness(bitstring1) - Fitness(bitstring2));
        }
    }
}
