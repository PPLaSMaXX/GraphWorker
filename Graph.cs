
namespace GraphWorker
{
    public class Graph
    {
        public string Label;

        private List<Node> Nodes = new();
        private List<Edge> Edges = new();

        public Graph(string label)
        {
            Label = label;
        }

        public void AddNode(int value)
        {
            Nodes.Add(new Node(value));
        }

        public void AddEdge(int start, int end, bool directed)
        {
            if (start >= 0 && start < Nodes.Count && end < Nodes.Count && end >= 0)

                Edges.Add(new Edge(start, end, directed));

            else throw new IndexOutOfRangeException();
        }

        public void RemoveNode(int position)
        {
            if (position >= 0 && position < Nodes.Count)
                Nodes.RemoveAt(position);
            else throw new IndexOutOfRangeException();
        }

        public void RemoveEdge(int start, int end)
        {
            if (start >= 0 && start < Nodes.Count && end < Nodes.Count && end >= 0)
                Edges.Remove(new Edge(start, end));
            else throw new IndexOutOfRangeException();
        }

        public void ShowNodes()
        {
            foreach (Node node in Nodes)
            {
                Console.WriteLine(Label + " has a node with value " + node.value);
            }
        }

        public void ShowEdges()
        {
            foreach (Edge edge in Edges)
            {
                Console.WriteLine(Label + " has edge with start in " + (edge.start + 1) + " and end on " + (edge.end + 1));
            }
        }

        public int[,] GetAdjacencyMatrix()
        {
            int[,] adjacencyMatrix = new int[Nodes.Count, Nodes.Count];

            for (int i = 0; i < adjacencyMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < adjacencyMatrix.GetLength(1); j++)
                {
                    if (Edges.Any(x => x.start == i && x.end == j) && Edges.Count != 0)
                    {
                        adjacencyMatrix[i, j] = 1;
                        adjacencyMatrix[j, i] = 1;
                    }
                }
            }

            return adjacencyMatrix;
        }

        public void ShowAdjacencyMatrix()
        {
            Console.WriteLine();
            int[,] adjacencyMatrix = GetAdjacencyMatrix();

            for (int i = 0; i < adjacencyMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < adjacencyMatrix.GetLength(1); j++)
                {
                    Console.Write(adjacencyMatrix[i, j] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public bool IsIsomorphicTo(Graph second)
        {
            int[,] mainMatrix = GetAdjacencyMatrix();
            int[,] comparedMatrix = second.GetAdjacencyMatrix();

            if (mainMatrix.Length != comparedMatrix.Length) return false;

            List<List<int>> Permutations = GetAllPermutations(mainMatrix.GetLength(0));

            foreach (List<int> permutationRow in Permutations)
            {
                foreach (List<int> permutationColumn in Permutations)
                {
                    int[,] matrix = mainMatrix;
                    matrix = PutRows(matrix, permutationRow);
                    matrix = PutColumns(matrix, permutationColumn);

                    if (Compare(matrix, comparedMatrix)) return true;
                }
            }

            return false;
        }

        public static int[,] PutColumns(int[,] fromMatrix, List<int> permutation)
        {
            int[,] toMatrix = (int[,])fromMatrix.Clone();

            for (int x = 0; x < permutation.Count; x++)
            {
                int[] column = GetColumn(fromMatrix, permutation[x]);

                for (int i = 0; i < toMatrix.GetLength(0); i++)
                {
                    for (int j = 0; j < toMatrix.GetLength(1); j++)
                    {
                        if (j == x) toMatrix[i, j] = column[i];
                    }
                }
            }

            return toMatrix;
        }

        public static int[,] PutRows(int[,] fromMatrix, List<int> permutation)
        {
            int[,] toMatrix = (int[,])fromMatrix.Clone();

            for (int x = 0; x < permutation.Count; x++)
            {
                int[] row = GetRow(fromMatrix, permutation[x]);

                for (int i = 0; i < toMatrix.GetLength(0); i++)
                {
                    for (int j = 0; j < toMatrix.GetLength(1); j++)
                    {
                        if (i == x) toMatrix[i, j] = row[j];
                    }
                }
            }
            return toMatrix;
        }

        public static List<List<int>> GetAllPermutations(int num)
        {
            List<int> adresses = new();

            for (int i = 0; i < num; i++)
            {
                adresses.Add(i);
            }

            return Permute(adresses.ToArray());
        }

        static List<List<int>> Permute(int[] nums)
        {
            var list = new List<List<int>>();
            return DoPermute(nums, 0, nums.Length - 1, list);
        }

        static List<List<int>> DoPermute(int[] nums, int start, int end, List<List<int>> list)
        {
            if (start == end)
            {
                list.Add(new List<int>(nums));
            }
            else
            {
                for (var i = start; i <= end; i++)
                {
                    Swap(ref nums[start], ref nums[i]);
                    DoPermute(nums, start + 1, end, list);
                    Swap(ref nums[start], ref nums[i]);
                }
            }

            return list;
        }

        static void Swap(ref int a, ref int b)
        {
            (b, a) = (a, b);
        }

        public static int[] GetColumn(int[,] matrix, int columnNumber)
        {
            return Enumerable.Range(0, matrix.GetLength(0))
                    .Select(x => matrix[x, columnNumber])
                    .ToArray();
        }

        public static int[] GetRow(int[,] matrix, int rowNumber)
        {
            return Enumerable.Range(0, matrix.GetLength(1))
                    .Select(x => matrix[rowNumber, x])
                    .ToArray();
        }

        public static bool Compare(int[,] firstMatrix, int[,] secondMatrix)
        {
            if (firstMatrix.Length != secondMatrix.Length) return false;

            for (int i = 0; i < firstMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < firstMatrix.GetLength(1); j++)
                {
                    if (firstMatrix[i, j] != secondMatrix[i, j]) return false;
                }
            }

            return true;
        }

        public string GetGraphType()
        {
            return DefineType();
        }

        private string DefineType()
        {
            string stringBuilder = "";

            bool existing = true;

            List<Edge> possibleEdges = PossibleEdges();

            foreach (Edge comparerEdge in possibleEdges)
            {
                if (!Edges.Contains(new Edge(comparerEdge.start, comparerEdge.end)) || Edges.Contains(new Edge(comparerEdge.end, comparerEdge.start)))
                {
                    if (!Edges.Contains(new Edge(comparerEdge.start, comparerEdge.end, true)) || Edges.Contains(new Edge(comparerEdge.end, comparerEdge.start, true)))
                    {
                        existing = false;
                        break;
                    }
                }
            }

            if (existing) stringBuilder += "complete ";

            if (Edges.Count == 0) stringBuilder += "null ";
            else if (Edges.All(x => x.isDirected)) stringBuilder += "directed ";
            else if (Edges.Any(x => x.isDirected)) stringBuilder += "mixed ";
            else stringBuilder += "simple ";

            List<Edge> comparerEdges = new List<Edge>(Edges);

            bool isMultigraph = false;

            foreach(Edge edge in Edges)
            {
                foreach(Edge comparerEdge in comparerEdges)
                {
                    if (edge.start == comparerEdge.end && edge.end == comparerEdge.start) isMultigraph = true;
                }
            }

            if (isMultigraph) stringBuilder += "multigraph";
            else if (Edges.Any(x => x.start == x.end)) stringBuilder += "multigraph";
            else stringBuilder += "graph";

            return stringBuilder;
        }

        public List<Edge> PossibleEdges()
        {
            List<Edge> edges = new();

            for (int i = 0; i < Nodes.Count; i++)
            {
                for (int j = 0; j < Nodes.Count; j++)
                {
                    if (i != j)
                    {
                        if (edges.Contains(new Edge(j, i))) continue;
                        Edge edge = new(i, j);

                        edges.Add(edge);
                    }
                }
            }

            return edges;
        }
    }

    public struct Node
    {
        public int value;

        public Node(int value)
        {
            this.value = value;
        }
    }

    public struct Edge
    {
        public int start;
        public int end;
        public bool isDirected = false;

        public Edge(int start, int end, bool directed)
        {
            this.start = start;
            this.end = end;
            this.isDirected = directed;
        }
        public Edge(int start, int end)
        {
            this.start = start;
            this.end = end;
        }
    }
}