using System.Collections.Generic;

namespace Assignment1
{
    class UniformOneCount : FitnessFunction
    {
        //This fitness function simply counts how many 1s are in this string.
        public override float Fitness(List<bool> bitstring)
        {
            float fitness = base.Fitness(bitstring);
            if(fitness == -1)
            {
                foreach(bool bit in bitstring)
                {
                    if(bit)
                    {
                        fitness++;
                    }
                }

                tabooList.Add(bitstring, fitness);
            }
            return fitness;
        }
    }
}
