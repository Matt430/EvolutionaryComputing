using System.Collections.Generic;

namespace Assignment1
{
    class TightLinkTrap : FitnessFunction
    {
        private int k;
        private float d;

        public TightLinkTrap(int k, float d) : base()
        {
            this.k = k;
            this.d = d;
        }

        public override float Fitness(List<bool> bitstring)
        {
            float fitness = base.Fitness(bitstring);
            if(fitness == -1)
            {
                for(int i = 0; i < bitstring.Count; i += k)
                {
                    int subCount = 0;
                    for(int j = 0; j < k; j++)
                    {
                        if(bitstring[i + j])
                        {
                            subCount += 1;
                        }
                    }

                    if(subCount == k)
                    {
                        fitness += k;
                    }
                    else
                    {
                        fitness += k - d - ((k - d) / (k - 1)) * subCount;
                    }
                }

                tabooList.Add(bitstring, fitness);
            }
            return fitness;
        }
    }
}
