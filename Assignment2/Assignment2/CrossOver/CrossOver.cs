using System;
using System.Collections.Generic;

namespace Assignment2
{
    abstract class Crossover
    {
        public abstract List<List<bool>> GenerateOffspring(List<List<bool>> population, Random random);
    }
}
