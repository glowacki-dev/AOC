#include "Solver12.h"

class Graph {
    map<string, list<string>> adj;
public:
    set<string> nodes;
    set<string> all_paths;

    void add_edge(string v, string w) {
        nodes.emplace(v);
        nodes.emplace(w);
        adj[v].push_back(w);
    }

    int count_paths(string v, map<string, bool> visited) {
        if(v == "end")
            return 1;

        // make a copy of visited paths and mark small case one as visited
        map<string, bool> new_visited = visited;
        if(v[0] >= 'a' && v[0] <= 'z')
            new_visited[v] = true;

        int found = 0;
        list<string>::iterator i;
        for (i = adj[v].begin(); i != adj[v].end(); ++i)
            if (!new_visited[*i])
                found += count_paths(*i, new_visited);

        return found;
    }

    void count_paths_2(string v, map<string, bool> visited, string double_visit, string current_path) {
        if(v == "end")
        {
            all_paths.emplace(current_path);
            return;
        }

        // if a node is allowed as double visit then mark it (use one visit), otherwise fallback to visited list
        map<string, bool> new_visited = visited;
        string new_double_visit = double_visit;
        if(v[0] >= 'a' && v[0] <= 'z') {
            if(double_visit == v) {
                new_double_visit = "";
            } else {
                new_visited[v] = true;
            }
        }

        list<string>::iterator i;
        for (i = adj[v].begin(); i != adj[v].end(); ++i)
            if (!new_visited[*i])
                count_paths_2(*i, new_visited, new_double_visit, current_path + "," + *i);
    }
};

void Solver12::solve() {
    Graph g = Graph();
    for(string line: lines) {
        string tmp;
        stringstream ss(line);
        string first = "";
        while(getline(ss, tmp, '-')) {
            if(first == "") first = tmp;
            else {
                g.add_edge(first, tmp);
                g.add_edge(tmp, first);
                first = "";
            }
        }
    }
    cout << g.count_paths("start", map<string, bool>()) << endl;

    // This is definitely not optimal, but still resolves in a reasonable time...
    for(string node: g.nodes) {
        if(node[0] >= 'A' && node[0] <= 'Z') continue;
        if(node == "start" || node == "end") continue;
        g.count_paths_2("start", map<string, bool>(), node, "start");
    }
    g.count_paths_2("start", map<string, bool>(), "", "start");
    cout << g.all_paths.size() << endl;
}

