using System;
using System.Collections.Generic;

namespace Assignment1
{
    abstract class Crossover
    {
        public abstract List<List<bool>> GenerateOffspring(List<List<bool>> population, Random random);
    }
}
