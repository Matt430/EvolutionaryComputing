using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1
{
    class LinearOneCount : FitnessFunction
    {
        public override float Fitness(List<bool> bitstring)
        {
            float count = 0;
            for (int i = 0; i < bitstring.Count; i++ )
            {
                if (bitstring[i])
                {
                    count += 1 + i;
                }
            }
            return count;
        }
    }
}
