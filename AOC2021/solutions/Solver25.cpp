#include "Solver25.h"

class Simulation {
    long width = 0;
    long height = 0;
    vector<int> data;
public:
    void add_line(string line) {
        if(width == 0) { width = line.length(); }
        height += 1;
        for(char c: line) {
            switch (c) {
                case '.':
                    data.push_back(0);
                    break;
                case '>':
                    data.push_back(1);
                    break;
                case 'v':
                    data.push_back(2);
                    break;
                default:
                    throw runtime_error("unrecognised input");
                    break;
            }
        }
    }

    bool move() {
        bool moved = false;
        vector<int> new_data = data;
        for(int y = 0; y < height; y++) {
            for(int x = 0; x < width; x++) {
                if(data[width * y + x] != 1) continue;
                long next = width * y + ((x + 1) % width);
                if(data[next] == 0) {
                    new_data[width * y + x] = 0;
                    new_data[next] = 1;
                    moved = true;
                }
            }
        }
        data = new_data;
        for(int y = 0; y < height; y++) {
            for(int x = 0; x < width; x++) {
                if(data[width * y + x] != 2) continue;
                long next = width * ((y + 1) % height) + x;
                if(data[next] == 0) {
                    new_data[width * y + x] = 0;
                    new_data[next] = 2;
                    moved = true;
                }
            }
        }
        data = new_data;
        return moved;
    }

    void preview() {
        for(int y = 0; y < height; y++) {
            for(int x = 0; x < width; x++) {
                switch (data[width * y + x]) {
                    case 0:
                        cout << '.';
                        break;
                    case 1:
                        cout << '>';
                        break;
                    case 2:
                        cout << 'v';
                        break;
                    default:
                        break;
                }
            }
            cout << endl;
        }
        cout << endl;
    }
};

void Solver25::solve() {
    Simulation sim;
    for(string line: lines) {
        sim.add_line(line);
    }
    int turn = 1;
    while(sim.move()) {
        turn += 1;
    }
    cout << turn << endl;
}

