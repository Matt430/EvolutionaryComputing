using System.Collections.Generic;

namespace Assignment1
{
    class LinearOneCount : FitnessFunction
    {
        public override float Fitness(List<bool> bitstring)
        {
            float count = 0;
            for(int i = 0; i < bitstring.Count; i++)
            {
                if(bitstring[i])
                {
                    count += 1 + i;
                }
            }
            return count;
        }
    }
}
