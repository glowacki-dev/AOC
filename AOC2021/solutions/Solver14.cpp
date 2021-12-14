#include "Solver14.h"

class Polymer {
    map<pair<char, char>, char> mapping;
    map<pair<char, char>, long long> pairs;
    map<char, long long> counts;

public:
    Polymer(string data) {
        for(int i = 0; i < data.length() - 1; i++) {
            pairs[pair<char, char>{data[i], data[i+1]}] += 1;
        }
        for(char c: data) {
            counts[c] += 1;
        }
    }

    void add_mapping(pair<char, char> input, char output) {
        mapping[input] = output;
    }

    void expand() {
        map<pair<char, char>, long long> new_pairs(pairs);
        for(auto m: mapping) {
            long long count = pairs[m.first];
            new_pairs[m.first] -= count;
            new_pairs[pair<char, char>{m.first.first, m.second}] += count;
            new_pairs[pair<char, char>{m.second, m.first.second}] += count;
            counts[m.second] += count;
        }
        pairs = new_pairs;
    }

    long long get_score() {
        auto minmax = minmax_element(counts.begin(), counts.end(), [](pair<char, long long> a, pair<char, long long> b){ return a.second < b.second; });
        return minmax.second->second - minmax.first->second;
    }
};

void Solver14::solve() {
    Polymer p = Polymer(lines[0]);
    lines.erase(lines.begin());
    for(string line: lines) {
        p.add_mapping(pair<char, char>{line[0], line[1]}, line[6]);
    }
    for(int i = 0; i < 40; i++) {
        p.expand();
        cout << i << " " << p.get_score() << endl;
    }
    cout << p.get_score() << endl;
}

