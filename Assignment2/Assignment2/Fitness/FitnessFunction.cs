using System.Collections.Generic;

namespace Assignment2
{
    abstract class FitnessFunction
    {
        private int fitnessCalls;
        protected Dictionary<List<bool>, int> tabooList;

        public FitnessFunction()
        {
            tabooList = new Dictionary<List<bool>, int>();
        }

        //The fitness function, to be implemented for every class that inherits from this.
        public virtual int Fitness(List<bool> bitstring)
        {
            if(tabooList.ContainsKey(bitstring))
                return tabooList[bitstring];

            ++fitnessCalls;

            return -1;
        }

        //A comparison of two bitstrings, to be used when sorting populations.
        public int FitnessCompare(List<bool> bitstring1, List<bool> bitstring2)
        {
            if(Fitness(bitstring1) == Fitness(bitstring2))
            {
                return 0;
            }
            if(Fitness(bitstring1) < Fitness(bitstring2))
            {
                return -1;
            }
            return 1;
        }

        public int FitnessCalls
        { get { return fitnessCalls; } }
    }
}
