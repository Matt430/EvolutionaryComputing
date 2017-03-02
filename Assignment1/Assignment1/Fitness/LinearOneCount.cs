using System.Collections.Generic;

namespace Assignment1
{
    class LinearOneCount : FitnessFunction
    {
        public override float Fitness(List<bool> bitstring)
        {
            float fitness = base.Fitness(bitstring);
            if(fitness == -1)
            {
                for(int i = 0; i < bitstring.Count; i++)
                {
                    if(bitstring[i])
                    {
                        fitness += 1 + i;
                    }
                }

                tabooList.Add(bitstring, fitness);
            }
            return fitness;
        }
    }
}
