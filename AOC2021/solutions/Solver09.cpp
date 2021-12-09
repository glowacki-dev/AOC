#include "Solver09.h"

class HeightMap {
    int width = 0;
    vector<int> values;

public:
    void add_row(string line) {
        if(width == 0) width = (int)line.length();
        for(char c: line) {
            values.push_back(c - '0');
        }
    }

    int get_risk_score() {
        vector<int> lows = get_low_points();
        return accumulate(lows.begin(), lows.end(), 0, [this](int acc, int i) { return acc + 1 + values[i]; });
    }

    vector<set<int>> get_basins() {
        vector<set<int>> result;
        for(int lowest: get_low_points()) {
            set<int> basin { lowest };
            while(grow_around(basin));
            result.push_back(basin);
        }
        return result;
    }

private:
    vector<int> indexes_around(int index) {
        vector<int> result;
        if(index % width > 0 && index - 1 >= 0) result.push_back(index - 1);
        if(index % width < width - 1 && index + 1 < values.size()) result.push_back(index + 1);
        if(index - width >= 0) result.push_back(index - width);
        if(index + width < values.size()) result.push_back(index + width);
        return result;
    }

    bool is_lowest(int index) {
        vector<int> around = indexes_around(index);
        return all_of(around.begin(), around.end(), [this, index](int i) { return values[index] < values[i]; });
    }

    vector<int> get_low_points() {
        vector<int> result;
        for(int i = 0; i < values.size(); i++) {
            if(is_lowest(i)) {
                result.push_back(i);
            }
        }
        return result;
    }

    bool grow_around(set<int> &basin) {
        set<int> new_points;
        for(int point: basin) {
            vector<int> around = indexes_around(point);
            for(int index: around) {
                if(basin.find(index) != basin.end() || new_points.find(index) != new_points.end()) continue;
                if(values[index] != 9) new_points.emplace(index);
            }
        }
        if(new_points.size() > 0) {
            basin.insert(new_points.begin(), new_points.end());
            return true;
        }
        return false;
    }
};

void Solver09::solve() {
    HeightMap hm = HeightMap();
    for(string line: lines) {
        hm.add_row(line);
    }
    cout << hm.get_risk_score() << endl;
    vector<set<int>> result = hm.get_basins();
    sort(result.begin(), result.end(), [](set<int> i, set<int> j) { return i.size() > j.size(); });
    cout << accumulate(result.begin(), result.begin() + 3, 1, [](int acc, set<int> el) { return acc * el.size(); }) << endl;
}
