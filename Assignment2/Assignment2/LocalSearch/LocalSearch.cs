using System.Collections.Generic;

namespace Assignment2
{
    class LocalSearch
    {
        GraphBipartition fitnessFunc;

        public LocalSearch(GraphBipartition fitnessFunc)
        {
            this.fitnessFunc = fitnessFunc;
        }

        public List<bool> Search(List<bool> bitstring)
        {
            List<bool> previousSolution = bitstring;
            List<bool> currentSolution = BetterNeighbor(bitstring);
            while (previousSolution != currentSolution)
            {
                previousSolution = currentSolution;
                currentSolution = BetterNeighbor(currentSolution);
            }
            return currentSolution;
        }

        public List<bool> BetterNeighbor(List<bool> bitstring)
        {
            List<bool> original = bitstring;
            List<bool> current = new List<bool>(bitstring);
            int bestFitness = fitnessFunc.Fitness(bitstring);

            for (int i = 0; i < bitstring.Count - 1; i++)
                for (int j = i + 1; j < bitstring.Count; j++)
                {
                    if (current[i] != current[j])
                    {
                        current[i] = !current[i];
                        current[j] = !current[j];

                        int neighborFitness = fitnessFunc.FitnessSwap(current, original, i, j);
                        if (neighborFitness < bestFitness)
                        {
                            return current;
                        }

                        current[i] = !current[i];
                        current[j] = !current[j];
                    }
                }

            return original;
        }

        public List<bool> BestNeighbor(List<bool> bitstring)
        {
            List<bool> original = bitstring;
            List<bool> current = new List<bool>(bitstring);
            List<bool> bestNeighbor = bitstring;
            int bestSwap1 = 0, bestSwap2 = 0;
            int bestFitness = fitnessFunc.Fitness(bitstring);

            for (int i = 0; i < bitstring.Count - 1; i++)
                for (int j = i + 1; j < bitstring.Count; j++)
                {
                    if (current[i] != current[j])
                    {
                        current[i] = !current[i];
                        current[j] = !current[j];

                        int neighborFitness = fitnessFunc.FitnessSwap(current, original, i, j);
                        if (neighborFitness < bestFitness)
                        {
                            bestNeighbor = new List<bool>(current);
                            bestSwap1 = i;
                            bestSwap2 = j;
                            bestFitness = neighborFitness;
                        }

                        current[i] = !current[i];
                        current[j] = !current[j];
                    }
                }

            return bestNeighbor;
        }
    }
}
