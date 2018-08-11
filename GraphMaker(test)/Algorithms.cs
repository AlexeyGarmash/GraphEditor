using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickGraph.Algorithms;
using QuickGraph;
using QuickGraph.Algorithms.Search;
using QuickGraph.Algorithms.ShortestPath;
using QuickGraph.Algorithms.Observers;
namespace GraphMaker_test_
{
    public static class Algorithms
    {
        private static double EdgeCostSearching(GraphVertex Source, GraphVertex Target, List<GraphEdge> listEdges)
        {
            foreach (var edge in listEdges)
            {
                if ((edge.StartVertex == Source && edge.EndVertex == Target) || (edge.StartVertex == Target && edge.EndVertex == Source))
                {
                    return edge.WeightEdge;
                }
            }
            return 0;
        }
        public static string ShortestWayDijsktraAlgorithmUnDirected(GraphVertex vertexD, List<GraphEdge> listEdge, List<GraphVertex> listVertex)
        {
                string s = "";
                UndirectedGraph<GraphVertex, UndirectedEdge<GraphVertex>> graph = new UndirectedGraph<GraphVertex, UndirectedEdge<GraphVertex>>();
                foreach (var vert in listVertex)
                {
                    graph.AddVertex(vert);
                }
                foreach (var edge in listEdge)
                {
                    graph.AddEdge(new UndirectedEdge<GraphVertex>(edge.StartVertex, edge.EndVertex));
                }
                Dictionary<UndirectedEdge<GraphVertex>, double> edgeCost = new Dictionary<UndirectedEdge<GraphVertex>, double>();

                int i = 0;
                foreach (var edge in graph.Edges)
                {
                    double eCost = EdgeCostSearching(edge.Source, edge.Target, listEdge);
                    edgeCost.Add(edge, eCost);
                    i++;
                }
                
                IEnumerable<UndirectedEdge<GraphVertex>> pathh;
                Func<UndirectedEdge<GraphVertex>, double> getW = edge => edgeCost[edge];
                UndirectedDijkstraShortestPathAlgorithm<GraphVertex, UndirectedEdge<GraphVertex>> diijkstra = new UndirectedDijkstraShortestPathAlgorithm<GraphVertex, UndirectedEdge<GraphVertex>>(graph, getW);
                UndirectedVertexDistanceRecorderObserver<GraphVertex, UndirectedEdge<GraphVertex>> distObs = new UndirectedVertexDistanceRecorderObserver<GraphVertex, UndirectedEdge<GraphVertex>>(getW);
                using (distObs.Attach(diijkstra))
                {
                    UndirectedVertexPredecessorRecorderObserver<GraphVertex, UndirectedEdge<GraphVertex>> predObs = new UndirectedVertexPredecessorRecorderObserver<GraphVertex, UndirectedEdge<GraphVertex>>();
                    using (predObs.Attach(diijkstra))
                    {
                        diijkstra.Compute(vertexD);
                        
                        foreach (KeyValuePair<GraphVertex, double> kvp in distObs.Distances)
                        {
                            s += "From " + vertexD.Name + " to " + kvp.Key.Name + " is " + kvp.Value + "         by ";
                            if (predObs.TryGetPath(kvp.Key, out pathh))
                            {
                                foreach (var t in pathh)
                                {
                                    s += "edge " + t.Source.Name + "<->" + t.Target.Name + " ";
                                }
                            }
                            s += System.Environment.NewLine;
                        }
                    }
                }
                return s;
        }
        public static string ShortestWayDijsktraAlgorithmDirected(GraphVertex vertexD, List<GraphEdge> listEdge, List<GraphVertex> listVertex)
        {
            string s = "";
            AdjacencyGraph<GraphVertex, Edge<GraphVertex>> graph = new AdjacencyGraph<GraphVertex, Edge<GraphVertex>>();
            foreach (var vert in listVertex)
            {
                graph.AddVertex(vert);
            }
            foreach (var edge in listEdge)
            {
                graph.AddEdge(new Edge<GraphVertex>(edge.StartVertex, edge.EndVertex));
            }
            Dictionary<Edge<GraphVertex>, double> edgeCost = new Dictionary<Edge<GraphVertex>, double>();
            int i = 0;
            foreach (var edge in graph.Edges)
            {
                double eCost = EdgeCostSearching(edge.Source, edge.Target, listEdge);
                edgeCost.Add(edge, eCost);
                i++;
            }
            Func<Edge<GraphVertex>, double> getW = edge => edgeCost[edge];
            DijkstraShortestPathAlgorithm<GraphVertex, Edge<GraphVertex>> diijkstra = new DijkstraShortestPathAlgorithm<GraphVertex, Edge<GraphVertex>>(graph, getW);
            VertexDistanceRecorderObserver<GraphVertex, Edge<GraphVertex>> distObs = new VertexDistanceRecorderObserver<GraphVertex, Edge<GraphVertex>>(getW);
            IEnumerable<Edge<GraphVertex>> pathh;
            using (distObs.Attach(diijkstra))
            {
                VertexPredecessorRecorderObserver<GraphVertex, Edge<GraphVertex>> predObs = new VertexPredecessorRecorderObserver<GraphVertex, Edge<GraphVertex>>();
                using (predObs.Attach(diijkstra))
                {
                    diijkstra.Compute(vertexD);
                    foreach (KeyValuePair<GraphVertex, double> kvp in distObs.Distances)
                    {
                        s += "From " + vertexD.Name + " to " + kvp.Key.Name + " is " + kvp.Value + "         by ";
                        if (predObs.TryGetPath(kvp.Key, out pathh))
                        {
                            foreach (var t in pathh)
                            {
                                s += "edge " + t.Source.Name + "<->" + t.Target.Name + " ";
                            }
                        }
                        s += System.Environment.NewLine;
                    }
                }
            }
            return s;
        }
        public static IEnumerable<UndirectedEdge<GraphVertex>> MinimumTreePrima(List<GraphVertex> listVertex, List<GraphEdge> listEdge)
        {
            UndirectedGraph<GraphVertex, UndirectedEdge<GraphVertex>> graph = new UndirectedGraph<GraphVertex, UndirectedEdge<GraphVertex>>();

            foreach (var vert in listVertex)
            {
                graph.AddVertex(vert);
            }
            foreach (var edge in listEdge)
            {
                graph.AddEdge(new UndirectedEdge<GraphVertex>(edge.StartVertex, edge.EndVertex));
            }
            Dictionary<UndirectedEdge<GraphVertex>, double> edgeCost = new Dictionary<UndirectedEdge<GraphVertex>, double>();

            int i = 0;
            foreach (var edge in graph.Edges)
            {
                double eCost = EdgeCostSearching(edge.Source, edge.Target, listEdge);
                edgeCost.Add(edge, eCost);
                i++;
            }
            Func<UndirectedEdge<GraphVertex>, double> getW = edge => edgeCost[edge];
            var PrimTree = graph.MinimumSpanningTreePrim(getW);
            return PrimTree;
        }

        public static IEnumerable<Edge<GraphVertex>> ShortestWayAstarAlgorithm(List<GraphVertex> listVertex, List<GraphEdge> listEdge, GraphVertex start, GraphVertex end)
        {
            AdjacencyGraph<GraphVertex, Edge<GraphVertex>> graph = new AdjacencyGraph<GraphVertex, Edge<GraphVertex>>();
            foreach (var vert in listVertex)
            {
                graph.AddVertex(vert);
            }
            foreach (var edge in listEdge)
            {
                graph.AddEdge(new Edge<GraphVertex>(edge.StartVertex, edge.EndVertex));
            }
            Dictionary<Edge<GraphVertex>, double> edgeCost = new Dictionary<Edge<GraphVertex>, double>();
            int i = 0;
            foreach (var edge in graph.Edges)
            {
                double eCost = EdgeCostSearching(edge.Source, edge.Target, listEdge);
                edgeCost.Add(edge, eCost);
                i++;
            }
            
            
            Func<Edge<GraphVertex>, double> getW = edge => edgeCost[edge];
            
            //---------------------------------
            IEnumerable<Edge<GraphVertex>> edgessAstar;
            AStarShortestPathAlgorithm<GraphVertex, Edge<GraphVertex>> astar = new AStarShortestPathAlgorithm<GraphVertex, Edge<GraphVertex>>(graph, getW, x => 0.0);
            VertexDistanceRecorderObserver<GraphVertex, Edge<GraphVertex>> distObsA = new VertexDistanceRecorderObserver<GraphVertex, Edge<GraphVertex>>(getW);
            using (distObsA.Attach(astar))
            {
                VertexPredecessorRecorderObserver<GraphVertex, Edge<GraphVertex>> predObs = new VertexPredecessorRecorderObserver<GraphVertex, Edge<GraphVertex>>();
                using (predObs.Attach(astar))
                {
                    astar.Compute(start);
                    if (predObs.TryGetPath(end, out edgessAstar))
                    {
                        return edgessAstar;
                    }
                }
            }
            return null;
        }
    }
}
