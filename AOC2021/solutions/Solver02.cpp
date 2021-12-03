#include "Solver02.h"

void Solver02::solve() {
    int position = 0;
    int depth = 0;
    int aim = 0;
    string line;
    for(string line: lines) {
        switch (line[0]) {
            case 'f':
                position += line[line.length() - 1] - '0';
                depth += aim * (line[line.length() - 1] - '0');
                break;
            case 'u':
                aim -= line[line.length() - 1] - '0';
                break;
            case 'd':
                aim += line[line.length() - 1] - '0';
                break;
            default:
                break;
        }
    }

    cout << position << endl << depth << endl << position * depth;
}
