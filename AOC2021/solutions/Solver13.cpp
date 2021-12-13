#include "Solver13.h"

class Paper {
    set<pair<int, int>> dots;
    int width = 0;
    int height = 0;

public:
    void add_dot(string line) {
        string tmp;
        stringstream ss(line);
        string first = "";
        while(getline(ss, tmp, ',')) {
            if(first == "") first = tmp;
            else {
                dots.emplace(pair<int, int>{stoi(first), stoi(tmp)});
                if(stoi(first) > width) width = stoi(first);
                if(stoi(tmp) > height) height = stoi(tmp);
            }
        }
    }

    void fold(string line) {
        string tmp;
        stringstream ss(line);
        string first = "";
        while(getline(ss, tmp, '=')) {
            if(first == "") first = tmp;
            else {
                if(first[first.length() - 1] == 'x') fold_x(stoi(tmp));
                else fold_y(stoi(tmp));
            }
        }
    }

    long count() {
        return dots.size();
    }

    void preview() {
        for(int y = 0; y <= height; y++){
            for(int x = 0; x <= width; x++) {
                if(dots.find(pair<int, int>{x, y}) != dots.end()) cout << "#";
                else cout << " ";
            }
            cout << endl;
        }
        cout << endl;
    }

private:
    void fold_y(int value) {
        set<pair<int, int>> new_dots;
        for(pair<int, int> dot: dots) {
            if(dot.second < value) new_dots.emplace(dot);
            else new_dots.emplace(pair<int, int>{dot.first, 2 * value - dot.second});
        }
        height = value - 1;
        dots = new_dots;
    }

    void fold_x(int value) {
        set<pair<int, int>> new_dots;
        for(pair<int, int> dot: dots) {
            if(dot.first < value) new_dots.emplace(dot);
            else new_dots.emplace(pair<int, int>{2 * value - dot.first, dot.second});
        }
        width = value - 1;
        dots = new_dots;
    }
};

void Solver13::solve() {
    Paper p = Paper();
    bool first_fold = false;
    for(string line: lines) {
        if(line[0] == 'f') {
            p.fold(line);
            if(!first_fold) {
                first_fold = true;
                cout << p.count() << endl;
            }
        } else {
            p.add_dot(line);
        }
    }
    p.preview();
}

