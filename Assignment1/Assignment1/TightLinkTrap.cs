using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1
{
    class TightLinkTrap : FitnessFunction
    {
        private int k;
        private float d;

        public TightLinkTrap(int k, float d)
        {
            this.k = k;
            this.d = d;
        }

        public override float Fitness(List<bool> bitstring)
        {
            float count = 0;
            for (int i = 0; i < bitstring.Count; i += k)
            {
                int subCount = 0;
                for (int j = 0; j < k; j++)
                {
                    if (bitstring[i + j])
                    {
                        subCount += 1;
                    }
                }
                if (subCount == k)
                {
                    count += k;
                }
                else
                {
                    count += k - d - ((k - d) / (k - 1)) * subCount;
                }

            }
            return count;
        }
    }
}
