namespace GraphWorker
{
    public class Graph
    {
        public string Label;

        public List<Node> Nodes = new();
        public List<Edge> Edges = new();

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

                Edges.Add(new Edge(Nodes[start], Nodes[end], directed));

            else throw new IndexOutOfRangeException();
        }

        public void RemoveNode(int position)
        {
            if (position >= 0 && position < Nodes.Count)
            {

                List<Edge> edgesToRemove = Edges.FindAll(x => x.fromNode == Nodes[position] || x.toNode == Nodes[position]);
                foreach (Edge e in edgesToRemove)
                {
                    Edges.Remove(e);
                }
                Nodes.RemoveAt(position);
            }

            else throw new IndexOutOfRangeException();
        }

        public void RemoveEdge(int position)
        {
            if (position >= 0 && position < Edges.Count)
            {
                Edges.RemoveAt(position);
            }
            else throw new IndexOutOfRangeException();
        }

        public void ShowNodes()
        {
            foreach (Node node in Nodes)
            {
                Console.WriteLine(Label + " has a node with value " + node.Value);
            }
        }

        public void ShowEdges()
        {
            foreach (Edge edge in Edges)
            {
                Console.WriteLine(Label + " has edge with start in " + (Nodes.IndexOf(edge.fromNode) + 1) + " and end in " + (Nodes.IndexOf(edge.toNode) + 1));
            }
        }

        public int[,] GetAdjacencyMatrix()
        {
            int[,] adjacencyMatrix = new int[Nodes.Count, Nodes.Count];

            for (int i = 0; i < adjacencyMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < adjacencyMatrix.GetLength(1); j++)
                {
                    if (Edges.Any(x => Nodes.IndexOf(x.fromNode) == i && Nodes.IndexOf(x.toNode) == j) && Edges.Count != 0)
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

        public int[,] PutColumns(int[,] fromMatrix, List<int> permutation)
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

        public int[,] PutRows(int[,] fromMatrix, List<int> permutation)
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

        public List<List<int>> GetAllPermutations(int num)
        {
            List<int> adresses = new();

            for (int i = 0; i < num; i++)
            {
                adresses.Add(i);
            }

            List<List<int>> list = new();

            return DoPermute(adresses.ToArray(), 0, adresses.ToArray().Length - 1, list);
        }

        List<List<int>> DoPermute(int[] nums, int start, int end, List<List<int>> list)
        {
            if (start == end)
            {
                list.Add(new List<int>(nums));
            }
            else
            {
                for (var i = start; i <= end; i++)
                {
                    (nums[start], nums[i]) = (nums[i], nums[start]);
                    DoPermute(nums, start + 1, end, list);
                    (nums[start], nums[i]) = (nums[i], nums[start]);
                }
            }

            return list;
        }

        public int[] GetColumn(int[,] matrix, int columnNumber)
        {
            return Enumerable.Range(0, matrix.GetLength(0))
                    .Select(x => matrix[x, columnNumber])
                    .ToArray();
        }

        public int[] GetRow(int[,] matrix, int rowNumber)
        {
            return Enumerable.Range(0, matrix.GetLength(1))
                    .Select(x => matrix[rowNumber, x])
                    .ToArray();
        }

        public bool Compare(int[,] firstMatrix, int[,] secondMatrix)
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
                if (!Edges.Contains(new Edge(comparerEdge.fromNode, comparerEdge.toNode)) || Edges.Contains(new Edge(comparerEdge.toNode, comparerEdge.fromNode)))
                {
                    if (!Edges.Contains(new Edge(comparerEdge.fromNode, comparerEdge.toNode, true)) || Edges.Contains(new Edge(comparerEdge.toNode, comparerEdge.fromNode, true)))
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

            bool isMultigraph = false;

            foreach (Edge edge in Edges)
            {
                if (isMultigraph) break;

                foreach (Edge comparerEdge in Edges)
                {
                    if (edge.fromNode == comparerEdge.toNode && edge.toNode == comparerEdge.fromNode)
                    {
                        isMultigraph = true;
                        break;
                    }
                    else if (edge != comparerEdge && edge.fromNode == comparerEdge.fromNode && edge.toNode == comparerEdge.toNode)
                    {
                        isMultigraph = true;
                        break;
                    }
                }
            }

            if (isMultigraph) stringBuilder += "multigraph";
            else if (Edges.Any(x => x.fromNode == x.toNode)) stringBuilder += "multigraph";
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
                        if (edges.Contains(new Edge(Nodes[j], Nodes[i]))) continue;
                        Edge edge = new(Nodes[i], Nodes[j]);

                        edges.Add(edge);
                    }
                }
            }

            return edges;
        }
    }

    public struct Node
    {
        public int Value;
        private int ID = 0;

        Random rnd = new();

        public Node(int value)
        {
            Value = value;
            ID = rnd.Next(int.MinValue, int.MaxValue);
        }

        public int GetID()
        {
            return ID;
        }

        public static bool operator ==(Node node0, Node node1)
        {
            return node0.GetHashCode() == node1.GetHashCode();
        }

        public static bool operator !=(Node node0, Node node1)
        {
            return node0.GetHashCode() != node1.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            return obj is Node node &&
                   Value == node.Value &&
                   ID == node.GetID();
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value, ID);
        }
    }

    public struct Edge
    {
        public Node fromNode;
        public Node toNode;
        public bool isDirected = false;
        private int ID = 0;

        Random rnd = new();

        public Edge(Node from, Node to, bool directed)
        {
            fromNode = from;
            toNode = to;
            isDirected = directed;
            ID = rnd.Next(int.MinValue, int.MaxValue);
        }
        public Edge(Node from, Node to)
        {
            fromNode = from;
            toNode = to;
            ID = rnd.Next(int.MinValue, int.MaxValue);
        }

        public int GetID()
        {
            return ID;
        }

        public static bool operator ==(Edge edge0, Edge edge1)
        {
            return edge0.GetHashCode() == edge1.GetHashCode();
        }

        public static bool operator !=(Edge edge0, Edge edge1)
        {
            return edge0.GetHashCode() != edge1.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            return obj is Edge edge &&
                   EqualityComparer<Node>.Default.Equals(fromNode, edge.fromNode) &&
                   EqualityComparer<Node>.Default.Equals(toNode, edge.toNode) &&
                   isDirected == edge.isDirected &&
                   ID == edge.GetID();
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(fromNode, toNode, isDirected, ID);
        }
    }
}