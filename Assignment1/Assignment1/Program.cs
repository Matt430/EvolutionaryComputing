using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1
{
    class Program
    {
        static Random random = new Random();

        static void Main(string[] args)
        {
            //Assign which fitness function must be used
            FitnessFunction fitnessFunction = new UniformOneCount();

            //Generate a random population
            List<List<bool>> population;
            population = GenerateRandomPopulation(100, 100);

            population.Sort(fitnessFunction.FitnessCompare);

            Console.WriteLine(fitnessFunction.Fitness(population[0]));
            Console.ReadLine();
        }

        //This method generates a list of random bitstrings with a population size of size, where each string has a length of stringlength
        static List<List<bool>> GenerateRandomPopulation(int size, int stringlength)
        {
            List<List<bool>> population = new List<List<bool>>();
            for (int i = 0; i < size; i++)
            {
                List<bool> bitString = new List<bool>();
                for (int j = 0; j < stringlength; j++)
                {
                    bitString.Add(random.NextDouble() >= 0.5);
                }
                population.Add(bitString);
            }
            return population;
        }

        //This methods converts a list of bits to a printable string.
        static string PrintString(List<bool> bitstring)
        {
            string bits = "";
            foreach (bool bit in bitstring)
            {
                if (bit)
                    bits += "1";
                else
                    bits += "0";
            }
            return bits;
        }
    }
}
