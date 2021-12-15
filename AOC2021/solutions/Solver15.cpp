#include "Solver15.h"

// iPair ==> Integer Pair
typedef pair<int, int> iPair;

// To add an edge
void addEdge(vector <pair<int, int> > adj[], int u,
                                     int v, int wt)
{
    adj[u].push_back(make_pair(v, wt));
}


// Prints shortest paths from src to all other vertices
void shortestPath(vector<pair<int,int> > adj[], int V, int src)
{
    // Create a priority queue to store vertices that
    // are being preprocessed. This is weird syntax in C++.
    // Refer below link for details of this syntax
    // http://geeksquiz.com/implement-min-heap-using-stl/
    priority_queue< iPair, vector <iPair> , greater<iPair> > pq;

    // Create a vector for distances and initialize all
    // distances as infinite (INF)
    vector<int> dist(V, INT_MAX);

    // Insert source itself in priority queue and initialize
    // its distance as 0.
    pq.push(make_pair(0, src));
    dist[src] = 0;

    /* Looping till priority queue becomes empty (or all
    distances are not finalized) */
    while (!pq.empty())
    {
        // The first vertex in pair is the minimum distance
        // vertex, extract it from priority queue.
        // vertex label is stored in second of pair (it
        // has to be done this way to keep the vertices
        // sorted distance (distance must be first item
        // in pair)
        int u = pq.top().second;
        pq.pop();

        // Get all adjacent of u.
        for (auto x : adj[u])
        {
            // Get vertex label and weight of current adjacent
            // of u.
            int v = x.first;
            int weight = x.second;

            // If there is shorted path to v through u.
            if (dist[v] > dist[u] + weight)
            {
                // Updating distance of v
                dist[v] = dist[u] + weight;
                pq.push(make_pair(dist[v], v));
            }
        }
    }

    // Print shortest distances stored in dist[]
    printf("Vertex Distance from Source\n");
    for (int i = 0; i < V; ++i)
        printf("%d \t\t %d\n", i, dist[i]);
}
void Solver15::solve() {
    int V = (int)(5 * lines[0].length()) * (int)(5 * lines.size());
    vector<iPair> adj[V + 1];
    int i = 0;
    for(int u = 0; u < 5; u++) {
        for(string line: lines) {
            for(int v = 0; v < 5; v++) {
                for(char c: line) {
                    int weight = (u + v + c - '0');
                    if(weight > 9) weight = weight % 9;
                    if(i % (5 * lines[0].length()) > 0) {
                        addEdge(adj, i - 1, i, weight);
                    }
                    if(i >= 5 * lines[0].length()) {
                        addEdge(adj, i - (int)(5 * lines[0].length()), i, weight);
                    }
                    if(i % (5 * lines[0].length()) < (5 * lines[0].length()) - 1) {
                        addEdge(adj, i + 1, i, weight);
                    }
                    if(i + (5 * lines[0].length()) < V) {
                        addEdge(adj, i + (int)(5 * lines[0].length()), i, weight);
                    }
                    i += 1;
                }
            }
        }
    }
    addEdge(adj, i, 0, 0);
    shortestPath(adj, V, i);
}

