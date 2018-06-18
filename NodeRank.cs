using System;
using System.Collections.Generic;

namespace STEMGames
{
    class NodeRank
    {
        static void Main(string[] args)
        {
            var s = Console.ReadLine();
            var t = s.Split(' ');
            int n = Convert.ToInt32(t[0]); // broj čvorova u usmjerenom grafu [1, 10^5]
            double beta = Convert.ToDouble(t[1]); // vjerojatnost ne-teleportiranja
            
            var rankCache = new List<List<decimal>> { FillList(n, decimal.Divide(1m, n)) };
            // add initial value for node rank in 1st iteration

            var nodes = new List<Node>(n);

            for (int i = 0; i < n; i++)
            {
                var node = new Node(); // init nodes as well while we're here
                                       // brid u grafu
                s = Console.ReadLine();
                var lstS = s.Split(' ');
                foreach (var j in lstS)
                {
                    var temp = Convert.ToInt32(j); // [0, n-1]
                    node.Neighbors.Add(temp);
                }
                //var lstI = lstS.Select(o => Convert.ToInt32(o));
                //node.Neighbors.AddRange(lstI);

                nodes.Add(node);
            }

            var q = Convert.ToInt32(Console.ReadLine()); // broj upita

            for (int query = 0; query < q; query++)
            {
                s = Console.ReadLine();
                var lstS = s.Split(' ');
                var queryNode = Convert.ToInt32(lstS[0]); // index čvora [0, n-1]
                var iteration = Convert.ToInt32(lstS[1]); // redni broj iteracije algoritma [1, 100]

                if (iteration >= rankCache.Count)
                {
                    var value = decimal.Divide(decimal.Subtract(1, Convert.ToDecimal(beta)), n); // we need MORE PRECISION MORE POWER!
                    for (int i = rankCache.Count; i <= iteration; i++)
                    {
                        rankCache.Add(FillList(n, value));

                        for (int nodeIndex = 0; nodeIndex < nodes.Count; nodeIndex++)
                        {
                            var node = nodes[nodeIndex];
                            var addValue = // MORE POWER!!!!!!!!!!
                                decimal.Divide(
                                    decimal.Multiply(Convert.ToDecimal(beta), rankCache[i - 1][nodeIndex]),
                                    node.Neighbors.Count);
                            foreach (var neighbor in node.Neighbors)
                            {
                                rankCache[i][neighbor] += addValue;
                            }
                        }
                    }
                }

                var rank = rankCache[iteration][queryNode];
                // vrijednost ranga traženog čvora n u traženoj iteraciji t
                Console.WriteLine("{0:0.0000000000}", rank);
            }
            Console.ReadLine();
        }

        private static List<decimal> FillList(int n, decimal value)
        {
            var x = new List<decimal>();
            for (int i = 0; i < n; i++)
            {
                x.Add(value);
            }

            return x;
        }
    }

    public class Node
    {
        public List<int> Neighbors { get; set; }

        public Node()
        {
            this.Neighbors = new List<int>();
        }
    }


}
