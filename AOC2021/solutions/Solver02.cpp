#include <iostream>
#include <fstream>
#include <climits>
#include <string>
#include <sstream>
#include <deque>
#include <numeric>
#include "Solver02.h"

using namespace std;

void Solver02::solve() {
    int position = 0;
    int depth = 0;
    string line;
    for(std::string line: lines) {
        switch (line[0]) {
            case 'f':
                position += line[line.length() - 1] - '0';
                break;
            case 'u':
                depth -= line[line.length() - 1] - '0';
                break;
            case 'd':
                depth += line[line.length() - 1] - '0';
                break;
            default:
                break;
        }
    }

    cout << position << endl << depth << endl << position * depth;
}
