#include "Solver12.h"

class Graph {
    map<string, list<string>> adj;
public:
    set<string> nodes;

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

    void count_paths_2(string v, map<string, bool> visited, string double_visit, int &score) {
        if(v == "end")
        {
            score++;
            return;
        }

        map<string, bool> new_visited = visited;
        if(v[0] >= 'a' && v[0] <= 'z')
            new_visited[v] = true;

        list<string>::iterator i;
        for (i = adj[v].begin(); i != adj[v].end(); ++i)
            if (!new_visited[*i])
                count_paths_2(*i, new_visited, double_visit, score);
            else if (double_visit == "" && *i != "start" && *i != "end")
                count_paths_2(*i, new_visited, *i, score);
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

    int score = 0;
    g.count_paths_2("start", map<string, bool>(), "", score);
    cout << score << endl;
}

