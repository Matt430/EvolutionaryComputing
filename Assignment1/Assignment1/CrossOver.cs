using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1
{
    abstract class Crossover
    {
        public abstract List<List<bool>> GenerateOffspring(List<List<bool>> population, Random random);
    }
}
