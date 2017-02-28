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
            //Assign how large the population should be.
            int populationCount = 250;
            //Assign string length.
            int stringLength = 100;
            //Assign length of subfunctions.
            int k = 4;
            //Assign deceptiveness. 1 = deceptive, 2.5 is non-deceptive.
            float d = 2.5f;
            //Assign which fitness function and crossover must be used.
            FitnessFunction fitnessFunction = new LooseLinkTrap(k, d, stringLength, random);
            Crossover crossover = new UniformCrossover();

            //Generate a random population
            List<List<bool>> population;
            population = GenerateRandomPopulation(populationCount, 100);

            //Main loop
            while (PrintString(population[0]) != PrintString(population[populationCount - 1]))
            {
                //Make sure the ordering is random.
                population = ShufflePopulation(population);
                //Generate new offsring using the choses crossover method.
                population = crossover.GenerateOffspring(population, random);
                //Sort the list and remove the worst half.
                population.Sort(fitnessFunction.FitnessCompare);
                population.RemoveRange(0, population.Count / 2);
            //}
                //Writeline for debug.
                Console.WriteLine(PrintString(population[populationCount - 1]));
                
            }
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

        //Randomizes the order of the population list.
        static List<List<bool>> ShufflePopulation(List<List<bool>> population)
        {
            int n = population.Count;  
            while (n > 1) 
            {  
                n--;  
                int k = random.Next(n + 1);  
                List<bool> value = population[k];  
                population[k] = population[n];  
                population[n] = value;  
            }
            return(population);
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
