#include "Solver11.h"

class Map {
    vector<int> values;
    set<int> flashed;
    deque<int> will_flash;

public:
    int width = 0;

    void add_row(string line) {
        if(width == 0) width = (int)line.length();
        for(char c: line) {
            values.push_back(c - '0');
        }
    }

    int simulate() {
        flashed.clear();
        will_flash.clear();

        // initial increase
        for(int i = 0; i < values.size(); i++) {
            values[i] += 1;
            if(values[i] == 10) will_flash.push_back(i);
        }
        // flash until new ones
        while(will_flash.size() > 0) {
            int i = will_flash.front();
            will_flash.pop_front();
            flash(i);
        }
        for(int i: flashed) {
            values[i] = 0;
        }
        return (int)flashed.size();
    }

private:
    vector<int> indexes_around(int index) {
        vector<int> result;
        bool not_left_edge = index % width > 0;
        bool not_top_edge = index - width >= 0;
        bool not_right_edge = index % width < width - 1;
        bool not_bottom_edge = index + width < values.size();
        // left
        if(not_left_edge) result.push_back(index - 1);
        // top-left
        if(not_left_edge && not_top_edge) result.push_back(index - width - 1);
        // top
        if(not_top_edge) result.push_back(index - width);
        // top-right
        if(not_top_edge && not_right_edge) result.push_back(index - width + 1);
        // right
        if(not_right_edge) result.push_back(index + 1);
        // bottom-right
        if(not_right_edge && not_bottom_edge) result.push_back(index + width + 1);
        // bottom
        if(not_bottom_edge) result.push_back(index + width);
        // bottom-left
        if(not_bottom_edge && not_left_edge) result.push_back(index + width - 1);
        return result;
    }

    void flash(int i) {
        // mark as flashed and skip flashing if already flashed
        if(!flashed.emplace(i).second) return;
        // increase around
        for(int index: indexes_around(i)) {
            values[index] += 1;
            if(values[index] == 10) will_flash.push_back(index);
        }
    }
};

void Solver11::solve() {
    Map m = Map();
    for(string line: lines) {
        m.add_row(line);
    }
    int i = 0;
    int score = 0;
    while(true) {
        i += 1;
        int flashed = m.simulate();
        if(i <= 100) score += flashed;
        if(flashed == m.width * m.width) {
            cout << i << endl;
            break;
        }
    }
    cout << score << endl;
}

