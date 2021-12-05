#include "Solver05.h"

class Line {
    int x1, x2, y1, y2;
public:
    Line(string _x1, string _y1, string _x2, string _y2) {
        x1 = stoi(_x1);
        y1 = stoi(_y1);
        x2 = stoi(_x2);
        y2 = stoi(_y2);
    }

    bool is_straight() {
        return x1 == x2 || y1 == y2;
    }

    vector<pair<int, int>> points() {
        vector<pair<int, int>> _points;
        if(x1 == x2) {
            for(int y = min(y1, y2); y <= max(y1, y2); y++) {
                _points.push_back(pair<int, int>(x1, y));
            }
        } else if (y1 == y2) {
            for(int x = min(x1, x2); x <= max(x1, x2); x++) {
                _points.push_back(pair<int, int>(x, y1));
            }
        } else {
            for(int i = 0; i <= max(x1, x2) - min(x1, x2); i++) {
                int x = x1 < x2 ? x1 + i : x1 - i;
                int y = y1 < y2 ? y1 + i : y1 - i;
                _points.push_back(pair<int, int>(x, y));
            }
        }
        return _points;
    }
};

void Solver05::solve() {
    vector<Line> vlines;
    for(string line: lines) {
        smatch sm;
        regex_search(line, sm, regex("(\\d+),(\\d+) -> (\\d+),(\\d+)"));
        if(sm.size() != 5) throw runtime_error("Error parsing input");
        vlines.push_back(Line(sm[1], sm[2], sm[3], sm[4]));
    }
    map<pair<int, int>, int> lines_map;
    for(Line line: vlines) {
        // if(!line.is_straight()) continue; // Part 1 only
        for(auto point: line.points()) {
            lines_map[point] += 1;
        }
    }
    long occurences = count_if(lines_map.begin(), lines_map.end(), [](pair<pair<int, int>, int> it) { return it.second > 1; });
    cout << occurences << endl;
}
