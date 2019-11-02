﻿using System; //ienumerable
using System.Linq; //enumerable.repeat
using System.Collections.Generic;

namespace Algorithms{

    

    static public class Prim<T> where T: IComparable<T>{
        static int N;
        static Edge<T> edge;
        static PartialOrderedTree<Edge<T>> queue;
        static MazeGraph<T> g;
        static bool[] added;


        static public void initializeVariables(MazeGraph<T> G) {
            N = G.numVert();
            g = new MazeGraph<T>(N);
            added = new bool[N];
            added[0] = true; //first node;
            edge = new Edge<T>(0, 0, default(T));
            queue = new PartialOrderedTree<Edge<T>>(N * ((N - 1) / 2) - N + 2);
            
        }

        static List<Edge<T>> adjEdges(MazeGraph<T> G, int i) {
            List< MazeGraph<T>.vertexCost > adj = G.Adjacents(i);
            List< Edge<T> > adjEdges_ = new List<Edge<T>>(adj.Count);
            for(int j = 0; j < adj.Count; ++j) {
                adjEdges_.Add(new Edge<T>(i, adj[j].vertex, adj[j].cost));
            }
            return adjEdges_;
        }

        static void enqueueAdjEdges(MazeGraph<T> G, int i) {
            List<Edge<T>> adj = adjEdges(G, i);
            for (int j = 0; j < adj.Count; ++j) {
                if (!added[adj[j].dest]) {
                    queue.insert(adj[j]);
                }
            }
        }


        static public MazeGraph<T> Execute(MazeGraph<T> G) {
            initializeVariables(G);
            enqueueAdjEdges(G, 0);
            for (int i = 1; i < N; i++) {
                do {
                    edge = queue.top();
                    queue.delete();
                } while (added[edge.dest]);
                g.addEdge(edge.orig, edge.dest, edge.cost);
                g.addEdge(edge.dest, edge.orig, edge.cost);
                added[edge.dest] = true;
                enqueueAdjEdges(G, edge.dest);
            }
            return g;
        }
    }

    static public class Kruskall<T> where T : IComparable<T> {
        


        static int n;
        static MazeGraph<T> g;
        static Partition P;
        static PartialOrderedTree<Edge<T>> queue;

        static void initializeVariables(MazeGraph<T> G) {
            n = G.numVert();
            g = new MazeGraph<T>(n);
            P = new Partition(n);
            queue = new PartialOrderedTree<Edge<T>>(n * n);
            for (int i = 0; i < n; ++i) {
                List<MazeGraph<T>.vertexCost> adj = G.Adjacents(i);
                for (int j = 0; j < adj.Count; ++j) {
                    queue.insert(new Edge<T>(i, adj[j].vertex, adj[j].cost));
                }
            }
        }

        public static MazeGraph<T> Execute(MazeGraph<T> G) {
            initializeVariables(G);
            for (int count = 1; count < n;) {
                Edge<T> edge = queue.top();
                queue.delete();
                int leader1 = P.find(edge.orig);
                int leader2 = P.find(edge.dest);
                if (leader1 != leader2) {
                    P.join(leader1, leader2);
                    g.addEdge(edge.orig, edge.dest, edge.cost);
                    g.addEdge(edge.dest, edge.orig, edge.cost);
                    ++count;
                }
            }
            return g;
        }


    }

    static public class AldousBroder<T> where T : IComparable<T> {
        static List<int> unvisited;
        static MazeGraph<T> g;

        public static void InitializeVariables(MazeGraph<T> G) {
            unvisited = new List<int>(G.numVert());
            g = new MazeGraph<T>(G.numVert());
            for (int i = 0; i < G.numVert(); ++i) {
                unvisited.Add(i);
            }
        }

        public static MazeGraph<T> Execute(MazeGraph<T> G) {
            InitializeVariables(G);
            int idx = UnityEngine.Random.Range(0, unvisited.Count);
            int current = unvisited[idx];
            unvisited.Remove(idx);
            int count = 0;
            while (unvisited.Count > 0 && count < 100000) {
                MazeGraph<T>.vertexCost adj = G[current][UnityEngine.Random.Range(0, G[current].Count)];
                if(unvisited.IndexOf(adj.vertex) != -1) {
                    g.addEdge(current, adj.vertex, adj.cost);
                    g.addEdge(adj.vertex, current, adj.cost);
                    unvisited.Remove(adj.vertex);
                }
                current = adj.vertex;
                count++;
            }
            return g;
        }


    }

    static public class BinaryTree<T> where T : IComparable<T> {
        static MazeGraph<T> g;

        public static void InitializeVariables(MazeGraph<T> G) {
            g = new MazeGraph<T>(G.numVert());
        }

        public static MazeGraph<T> Execute(MazeGraph<T> G) {
            InitializeVariables(G);
            for(int i = 0; i < g.numVert(); ++i) {
                List<MazeGraph<T>.vertexCost> neighbors = new List<MazeGraph<T>.vertexCost>();
                List<MazeGraph<T>.vertexCost> adj = G[i];
                foreach(MazeGraph<T>.vertexCost v in adj) {
                    if( v.vertex < i) {
                        neighbors.Add(v);
                    }
                }
                if(neighbors.Count > 0) {
                    int idx = UnityEngine.Random.Range(0, neighbors.Count);
                    MazeGraph<T>.vertexCost neighbor = neighbors[idx];
                    g.addEdge(i, neighbor.vertex, neighbor.cost);
                    g.addEdge(neighbor.vertex, i, neighbor.cost);
                }

            }
            return g;
        }


    }

    static public class Sidewinder<T> where T : IComparable<T> {
        static MazeGraph<T> g;

        public static void InitializeVariables(MazeGraph<T> G) {
            g = new MazeGraph<T>(G.numVert());
        }

        public static MazeGraph<T> Execute(MazeGraph<T> G) {
            InitializeVariables(G);
            for (int i = 0; i < g.numVert(); ++i) {
                List<MazeGraph<T>.vertexCost> neighbors = new List<MazeGraph<T>.vertexCost>();
                List<MazeGraph<T>.vertexCost> adj = G[i];
                foreach (MazeGraph<T>.vertexCost v in adj) {
                    if (v.vertex < i) {
                        neighbors.Add(v);
                    }
                }
                if (neighbors.Count > 0) {
                    int idx = UnityEngine.Random.Range(0, neighbors.Count);
                    MazeGraph<T>.vertexCost neighbor = neighbors[idx];
                    g.addEdge(i, neighbor.vertex, neighbor.cost);
                    g.addEdge(neighbor.vertex, i, neighbor.cost);
                }

            }
            return g;
        }
    }

    static public class Wilson<T> where T : IComparable<T> {
        static MazeGraph<T> g;
        static List<int> unvisited;

        public static void InitializeVariables(MazeGraph<T> G) {
            g = new MazeGraph<T>(G.numVert());
            unvisited = new List<int>(G.numVert());
            for (int i = 0; i < G.numVert(); ++i) {
                unvisited.Add(i);
            }
        }

        public static MazeGraph<T> Execute(MazeGraph<T> G) {
            InitializeVariables(G);
            int count = 0;
            int idx = UnityEngine.Random.Range(0, unvisited.Count);
            int first = unvisited[idx];
            unvisited.RemoveAt(idx);
            while(unvisited.Count > 0 && count<500000) {
                int current = unvisited[UnityEngine.Random.Range(0, unvisited.Count)];
                List<int> path = new List<int>();
                path.Add(current);
                while (unvisited.IndexOf(current) != -1 && count < 500000) {
                    current = G.Adjacents(current)[UnityEngine.Random.Range(0, G.Adjacents(current).Count)].vertex;
                    int pos = path.IndexOf(current);
                    if (pos == -1) {
                        path.Add(current);
                    } else {
                        path = path.GetRange(0, pos + 1);
                    }
                    count++;
                }
                for (int i = 0; i < path.Count - 1; ++i) {
                    g.addEdge(path[i], path[i + 1], default(T));
                    g.addEdge(path[i + 1], path[i], default(T));                    
                    unvisited.Remove(path[i]);
                }

            }            
            return g;
        }
    }

    static public class RecursiveDivision<T> where T : IComparable<T> {
        static MazeGraph<T> g;
        static System.Random rand;

        //If function is called only with 1 parameter, it means you must find the number of row  
        //by find a cell that travels to the north,indicated by an adjacent vertex whose ID be greater than
        //current_vertex_ID+1 (this would mean "east" vertex), this algorithm requires a width and height of 
        //at least 2, otherwise it could cause unexpected behavior.
        public static void InitializeVariables(MazeGraph<T> G) {
            g = new MazeGraph<T>(G);
            rand = new System.Random();
        }

        public static MazeGraph<T> Execute(MazeGraph<T> G) {
            InitializeVariables(G);
            Divide(0, 0, g.rows, g.cols);
            return g;
        }

        public static void Divide(int row, int col, int height, int width) {
            if (height <= 1 || width <= 1) {
                return;
            } else {
                if (height > width) {
                    DivideHorizontally(row, col, height, width);
                } else if (width > height) {
                    DivideVertically(row, col, height, width);
                } else {
                    switch (rand.Next(2)) {
                        case 0: DivideVertically(row, col, height, width); break;
                        case 1: DivideHorizontally(row, col, height, width); break;
                    }
                }
            }
        }

        public static void DivideHorizontally(int row, int col, int height, int width) {
            int divideCell = rand.Next(height - 1);
            int passage_at = rand.Next(width);
            for (int i = 0; i < width; ++i) {
                if (i != passage_at) {
                    int r = row + divideCell;
                    int c = col + i;
                    int node = g.GetNode(r, c);
                    int southnode = g.GetSouth(r, c);
                    g.removeEdge(node, southnode);
                    g.removeEdge(southnode, node);
                }
            }
            Divide(row, col, divideCell + 1, width);
            Divide(row + divideCell + 1, col, height - divideCell - 1, width);
        }

        public static void DivideVertically(int row, int col, int height, int width) {
            int divideCell = rand.Next(width - 1);
            int passage_at = rand.Next(height);
            for (int i = 0; i < height; ++i) {
                if (i != passage_at) {
                    int r = row + i;
                    int c = col + divideCell;
                    int node = g.GetNode(r, c);
                    int eastnode = g.GetEast(r, c);
                    g.removeEdge(node, eastnode);
                    g.removeEdge(eastnode, node);
                }
            }
            Divide(row, col, height, divideCell + 1);
            Divide(row, col + divideCell + 1, height, width - divideCell - 1);
        }
    }

    static public class HuntAndKill<T> where T : IComparable<T> {
        static MazeGraph<T> g;
        static bool[] visited;
        static int counter;
        static Random rand;

        static public void initializeVariables(MazeGraph<T> G) {
            counter = G.numVert();
            g = new MazeGraph<T>(counter, G.rows, G.cols);
            visited = new bool[counter];
            rand = new Random();

        }

        static public MazeGraph<T> Execute(MazeGraph<T> G) {
            initializeVariables(G);
            int current = rand.Next(g.numVert());
            visited[current] = true;
            do {
                current = Kill(current);
                if (current == -1) {
                    current = Hunt();
                }
            } while (current != -1);
            return g;
        }

        private static int Kill(int current) {
            bool deadend = false;
            while (!deadend) {
                List<int> noAdj = g.UnconectedNeighbors(current / g.cols, current % g.cols);
                bool unconnected = false;
                while (0 < noAdj.Count && !unconnected) {
                    int idx = rand.Next(noAdj.Count);
                    if (!visited[noAdj[idx]]) {
                        g.addEdge(current, noAdj[idx], default(T));
                        g.addEdge(noAdj[idx], current, default(T));
                        current = noAdj[idx];
                        visited[current] = true;
                        unconnected = true;
                        --counter;
                    } else {
                        noAdj.RemoveAt(idx);
                    }
                }
                if (!unconnected) {
                    deadend = true;
                }
            }
            return -1;
        }

        private static int Hunt() {
            for (int i = 0; i < g.numVert(); ++i) {
                if (!visited[i]) {
                    List<int> neighbors = g.neighbors(i / g.cols, i % g.cols);
                    foreach (int neighbor in neighbors) {
                        if (visited[neighbor]) {
                            g.addEdge(i, neighbor, default(T));
                            g.addEdge(neighbor, i, default(T));
                            visited[i] = true;
                            return i;
                        }
                    }
                }
            }
            return -1;
        }
    }



    public class Partition {
        List<int> parent;

        public Partition(int n) {
            parent = new List<int>(Enumerable.Repeat(-1, n));
        }

        public void join(int a, int b) {
            if (parent[b] < parent[a])
                parent[a] = b;
            else {
                if (parent[a] == parent[b])
                    parent[a]--;
                parent[b] = a;
            }
        }

        public int find(int a) {
            int b, leader = a;
            while (parent[leader] > -1) {
                leader = parent[leader];
            }
            while (parent[a] > -1) {
                b = parent[a];
                parent[a] = leader;
                a = b;
            }
            return leader;
        }
    }

    /*random positions algorithms*/
    static public class RandPerm {
        static public int[] randperm(int n) {
            int[] indexes = new int[n];
            double[] rvalues = new double[n];
            System.Random rnd = new System.Random();
            for (int i = 0; i < n; i++) {
                indexes[i] = i;
                rvalues[i] = rnd.NextDouble();
            }
            Array.Sort(rvalues, indexes);
            return indexes;
        }
    }

    static public class Shuffle<T> {
        static public List<T> FisherYates(List<T> L) {
            List<T> newlist = L;
            System.Random rand = new System.Random();
            for(int i = L.Count-1; i >= 0; --i) {
                int j = rand.Next(i+1);
                T temp = newlist[j];
                newlist[j] = newlist[i];
                newlist[i] = temp;
            }
            return newlist;
        }

    }


}