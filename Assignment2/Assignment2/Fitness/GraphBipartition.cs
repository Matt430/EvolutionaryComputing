using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2
{
    class GraphBipartition : FitnessFunction
    {
        int[][] halfGraph;

        public GraphBipartition(int[][] graph)
        {
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

        public override float Fitness(List<bool> bitstring)
        {
            //Check for every edge if both vertices are in the same partition.
            float fitness = base.Fitness(bitstring);
            if (fitness == -1)
            {
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
    }
}
