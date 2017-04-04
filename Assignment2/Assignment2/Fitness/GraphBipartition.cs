using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2
{
    class GraphBipartition : FitnessFunction
    {
        int[][] fullGraph;
        int[][] halfGraph;

        public GraphBipartition(int[][] graph)
        {
            fullGraph = graph;
            //This changes the graph into a halfGraph, so each edge only gets checked once.
            halfGraph = new int[graph.Length][];
            for (int i = 0; i < graph.Length; i++)
            {
                int k = 0;
                for (int j = 0; j < graph[i].Length; j++)
                {
                    if (graph[i][j] > i)
                        k++;
                }
                halfGraph[i] = new int[k];
                int l = 0;
                for (int j = 0; j < graph[i].Length; j++)
                {
                    if (graph[i][j] > i)
                    {
                        halfGraph[i][l] = graph[i][j];
                        l++;
                    }
                }
            }
        }

        public override int Fitness(List<bool> bitstring)
        {
            
            int fitness = base.Fitness(bitstring);
            if (fitness == -1)
            {
                //Check for every edge if both vertices are in the same partition.
                fitness = 0;
                for (int i = 0; i < halfGraph.Length; i++ )
                {
                    for (int j = 0; j < halfGraph[i].Length; j++ )
                        if (bitstring[i] != bitstring[halfGraph[i][j]])
                        {
                            fitness++;
                        }
                }

                tabooList.Add(bitstring, fitness);
            }
            return fitness;
        }

        public int FitnessSwap(List<bool> bitstring, List<bool> originalString, int swap1, int swap2)
        {
            int fitness = base.Fitness(bitstring);
            if (fitness == -1)
            {
                //Check for the swapped vertices if the vertices connected to it are in the same partition.
                fitness = Fitness(originalString);
                for (int j = 0; j < fullGraph[swap1].Length; j++)
                    if (fullGraph[swap1][j] != swap2)
                    {
                        if (bitstring[swap1] != bitstring[fullGraph[swap1][j]])
                        {
                            fitness++;
                        }
                        else
                        {
                            fitness--;
                        }
                    }
                for (int j = 0; j < fullGraph[swap2].Length; j++)
                    if (fullGraph[swap2][j] != swap1)
                    {
                        if (bitstring[swap2] != bitstring[fullGraph[swap2][j]])
                        {
                            fitness++;
                        }
                        else
                        {
                            fitness--;
                        }
                    }

            }
            return fitness;
        }
    }
}
