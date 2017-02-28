using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1
{
    class UniformOneCount : FitnessFunction
    {
        //This fitness function simply counts how many 1s are in this string.
        public override int Fitness(List<bool> bitstring)
        {
            int count = 0;
            foreach (bool bit in bitstring)
            {
                if (bit)
                {
                    count++;
                }
            }
            return count;
        }
    }
}
