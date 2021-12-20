#include "Solver19.h"

class Point {
    int x, y, z;
public:
    Point(int x, int y, int z) {
        this->x = x;
        this->y = y;
        this->z = z;
    }
};

class Scanner {
    vector<Point> points;
public:
    void add_point(string line) {
        string tmp;
        stringstream ss(line);
        vector<int> coordinates;
        while(getline(ss, tmp, ',')) {
            coordinates.push_back(stoi(tmp));
        }
        points.push_back(Point(coordinates[0], coordinates[1], coordinates[2]));
    }
};

void Solver19::solve() {
    vector<Scanner> scanners;
    for(string line: lines) {
        if(line.find("---", 0) == 0) {
            scanners.push_back(Scanner());
        } else {
            scanners.back().add_point(line);
        }
    }
    cout << scanners.size() << endl;
}

