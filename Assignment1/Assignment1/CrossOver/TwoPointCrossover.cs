using System;
using System.Collections.Generic;

namespace Assignment1
{
    class TwoPointCrossover : Crossover
    {
        public override List<List<bool>> GenerateOffspring(List<List<bool>> population, Random random)
        {
            int length = population.Count;
            int stringLength = population[0].Count;

            for(int i = 0; i + 1 < length; i += 2)
            {
                // Choose a random start and end location
                int x = 1 + random.Next(length - 1);
                int y = 1 + random.Next(length - 1);
                while(x == y)
                {
                    x = 1 + random.Next(length - 1);
                    y = 1 + random.Next(length - 1);
                }
                if(x > y)
                {
                    int temp = x;
                    x = y;
                    y = temp;
                }

                // Create the children
                List<bool> child1 = new List<bool>(), child2 = new List<bool>();
                for(int j = 0; j < stringLength; j++)
                {
                    if(j < x || j >= y)
                    {
                        child1.Add(population[i][j]);
                        child2.Add(population[i + 1][j]);
                    }
                    else
                    {
                        child1.Add(population[i + 1][j]);
                        child2.Add(population[i][j]);
                    }
                }
                population.Add(child1);
                population.Add(child2);
            }
            return (population);
        }
    }
}
