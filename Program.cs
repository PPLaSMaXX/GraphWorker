namespace GraphWorker
{
    public class Program
    {
        public static void Main()
        {
            List<Graph> graphs = new();

            Console.WriteLine(  "##========================================================##\n" +
                                "## Welcome to GraphWorker v1.0 type help to get more info ##\n" +
                                "##========================================================##\n");

            while (true)
            {
                string? input = Console.ReadLine();

                if (input == null) continue;
                switch (input)
                {
                    case "help": Help(); break;
                    case "exit": return;
                    case "clear": Console.Clear(); break;
                }

                string[] inputSplitted = input.Split(' ');

                switch (inputSplitted[0])
                {
                    case "add": graphs = Add(graphs, inputSplitted); break;
                    case "delete": graphs = Delete(graphs, inputSplitted); break;
                    case "show": Show(graphs, inputSplitted); break;
                }
            }
        }

        static void Help()
        {
            Console.WriteLine(
                "\nadd\n\t" +
                    "graph {name}\n\t" +
                    "node {graphName} {Value}\n\t" +
                    "edge {graphName} {from} {to} {is directed (false or true)}\n" +
                "delete\n\t" +
                    "graph {name}\n\t" +
                    "node {graphName} {position}\n\t" +
                    "edge {graphName} {position}\n" +
                "show\n\t" +
                    "matrix {graphName}\n\t" +
                    "graphs\n\t" +
                    "edges {graphName}\n\t" +
                    "nodes {graphName}\n\t" +
                    "graphType {graphName}\n\t" +
                    "isIsomorphic {graphName} {graphName}\n" +
                "clear\tclears the console\n"+
                "exit\texits program");
        }

        private static List<Graph> Add(List<Graph> graphs, string[] input)
        {
            if (input == null || graphs == null) return null!;

            try
            {
                switch (input[1])
                {
                    case "graph":
                        {
                            if (graphs.Any(x => x.Label == input[2])) { Console.WriteLine("\nThis name is alredy taken\n"); break; }
                            graphs.Add(new Graph(input[2]));
                            Console.WriteLine($"\nAdded graph {input[2]}\n");
                            break;
                        }

                    case "node": graphs.Find(x => x.Label == input[2])!.AddNode(Convert.ToInt32(input[3])); Console.WriteLine($"\nAdded node to {input[2]} with value {input[3]}\n"); break;
                    case "edge": graphs.Find(x => x.Label == input[2])!.AddEdge(Convert.ToInt32(input[3]) - 1, Convert.ToInt32(input[4]) - 1, Convert.ToBoolean(input[5])); Console.WriteLine($"\nAdded edge to {input[2]} from {input[3]} to {input[4]}\n"); break;
                }
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("\nSome index is out of range\n");
            }
            catch
            {
                Console.WriteLine("\nSomething went wrong, please re-check your input!\n");
                Help();
            }

            return graphs;
        }

        private static List<Graph> Delete(List<Graph> graphs, string[] input)
        {
            if (input == null || graphs == null) return null!;

            try
            {
                switch (input[1])
                {
                    case "graph":
                        {
                            if (!graphs.Any(x => x.Label == input[2]))
                            {
                                Console.WriteLine("\nThis name doesn't exist\n");
                                break;
                            }
                            else
                            {
                                graphs.RemoveAt(graphs.FindIndex(x => x.Label == input[2]));
                                Console.WriteLine($"\nDeleted graph {input[2]}\n");
                            }
                            break;
                        }
                    case "node": graphs.Find(x => x.Label == input[2])!.RemoveNode(Convert.ToInt32(input[3]) - 1); Console.WriteLine($"\nDeleted node in {input[2]}\n"); break;
                    case "edge": graphs.Find(x => x.Label == input[2])!.RemoveEdge(Convert.ToInt32(input[3]) - 1); Console.WriteLine($"\nDeleted edge in {input[2]}\n"); break;
                }
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("\nSome index is out of range\n");
            }
            catch
            {
                Console.WriteLine("\nSomething went wrong, please re-check your input!\n");
                Help();
            }

            return graphs;
        }

        private static void Show(List<Graph> graphs, string[] input)
        {
            if (input == null || graphs == null) return;

            Console.WriteLine();

            try
            {
                switch (input[1])
                {
                    case "matrix": graphs.Find(x => x.Label == input[2])!.ShowAdjacencyMatrix(); break;
                    case "graphs": graphs.ForEach(x => Console.WriteLine(x.Label)); break;
                    case "nodes": graphs.Find(x => x.Label == input[2])!.ShowNodes(); break;
                    case "edges": graphs.Find(x => x.Label == input[2])!.ShowEdges(); break;
                    case "graphType": Console.WriteLine(graphs.Find(x => x.Label == input[2])!.GetGraphType()); break;
                    case "isIsomorphic": Console.WriteLine(graphs.Find(x => x.Label == input[2])!.IsIsomorphicTo(graphs.Find(x => x.Label == input[3])!)); break;
                }
            }
            catch
            {
                Console.WriteLine("\nSomething went wrong, please re-check your input!\n");
                Help();
            }
            finally
            {
                Console.WriteLine();
            }
        }
    }
}