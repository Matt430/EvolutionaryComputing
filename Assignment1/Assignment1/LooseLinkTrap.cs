using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1
{
    class LooseLinkTrap : FitnessFunction
    {
        private int k;
        private float d;
        private int[] permutation;

        public LooseLinkTrap(int k, float d, int stringLength, Random random)
        {
            this.k = k;
            this.d = d;
            permutation = new int[stringLength];
            for (int i = 0; i < stringLength; i++)
            {
                permutation[i] = i;
            }
            int n = stringLength;
            while (n > 1)
            {
                n--;
                int s = random.Next(n + 1);
                int value = permutation[s];
                permutation[s] = permutation[n];
                permutation[n] = value;
            }
        }

        public override float Fitness(List<bool> bitstring)
        {
            float count = 0;
            for (int i = 0; i < bitstring.Count; i += k)
            {
                int subCount = 0;
                for (int j = 0; j < k; j++)
                {
                    if (bitstring[permutation[i + j]])
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
