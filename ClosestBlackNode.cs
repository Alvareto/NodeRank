using System;
using System.Collections.Generic;
using System.Linq;

namespace STEMGames
{
    class ClosestBlackNode
    {
        /// <summary>
        /// Distance from nearest black node that is considered invalid for our use case.
        /// </summary>
        private static int INVALID = -1;
        /// <summary>
        /// Max distance from black node after which we find it invalid.
        /// </summary>
        private static int MAX_DISTANCE_BEFORE_INVALID = 10;
        /// <summary>
        /// Black node is always 0 distance away from closest black node.
        /// </summary>
        private static int DEFAULT_BLACK_NODE_DISTANCE = 0;

        static void Main(string[] args)
        {
            string line = Console.ReadLine();
            var splitLine = line?.Split(' ');
            int n = Convert.ToInt32(splitLine?[0]); // broj čvorova [1, 10^5]
            int e = Convert.ToInt32(splitLine?[1]); // broj bridova <= 2.5 * n

            var nodes = new List<Node>();
            // N čvovora w/ tip čvora t = {0, 1}, 1 if black
            for (int i = 0; i < n; i++)
            {
                int t = Convert.ToInt32(Console.ReadLine());
                nodes.Add(new Node(i, t));
            }

            // s & d predstavljaju indekse čvorova 
            // između kojih postoji brid u neusmjerenom grafu
            for (int i = 0; i < e; i++)
            {
                line = Console.ReadLine();
                splitLine = line?.Split(' ');
                int s = Convert.ToInt32(splitLine?[0]);
                int d = Convert.ToInt32(splitLine?[1]);

                nodes[s].Neighbors.Add(d);
                nodes[d].Neighbors.Add(s);
            }

            //Console.WriteLine();

            for (int i = 0; i < n; i++)
            {
                var result = new ClosestNode { Distance = DEFAULT_BLACK_NODE_DISTANCE, Node = i }; // for  black node
                if (!nodes[i].IsBlack)
                {
                    result = Search(nodes, i);
                }
                Console.WriteLine("{0} {1}", result.Node, result.Distance);
            }

            //Console.ReadLine();
        }


        /// <summary>
        /// Breadth-first search for closest black node to given root node.
        /// </summary>
        /// <param name="nodes">List of nodes with neighbors</param>
        /// <param name="root">Root node</param>
        /// <returns></returns>
        private static ClosestNode Search(IReadOnlyList<Node> nodes, int root)
        {
            var closest = new ClosestNode();

            var visited = new List<bool>();
            for (var i = 0; i < nodes.Count; i++)
            {
                visited.Add(i == root); // true if root, else false (not visited)
            }
            //Debug.Assert(visited[root], nameof(visited) + "[root] == true"); // assert that root is true.

            var currentLevel = new Queue<int>();
            currentLevel.Enqueue(root);

            var distance = 0;
            while (distance <= MAX_DISTANCE_BEFORE_INVALID && closest.Distance == INVALID)
            {
                var nextLevel = new Queue<int>();

                var levelResults = new List<ClosestNode>();
                while (currentLevel.Any())
                {
                    var current = currentLevel.Dequeue();

                    if (nodes[current].IsBlack)
                    {
                        var cn = new ClosestNode { Node = current, Distance = distance };
                        levelResults.Add(cn);
                    }

                    foreach (var n in nodes[current].Neighbors)
                    {
                        if (visited[n])
                        {
                            continue;
                        }
                        visited[n] = true;
                        nextLevel.Enqueue(n);
                    }
                }
                ++distance;

                currentLevel = nextLevel;
                foreach (var res in levelResults)
                {
                    if (res.Distance < closest.Distance || (res.Distance == closest.Distance && res.Node < closest.Node) ||
                        closest.Distance == INVALID)
                    {
                        closest = res;
                    }
                }
            }

            return closest;
        }
    }


    public class ClosestNode
    {
        public int Node { get; set; }
        public int Distance { get; set; }

        public ClosestNode()
        {
            Node = -1;
            Distance = -1;
        }
    }

    public enum Type
    {
        White = 0,
        Black = 1
    }

    public class Node
    {
        public List<int> Neighbors { get; }
        public bool IsBlack => Type == Type.Black;
        private Type Type { get; }

        public Node(int i, int t)
        {
            Type = (Type)t; // TODO: check if this works
            Neighbors = new List<int>();
        }
    }
}
