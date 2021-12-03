#include "Solver01.h"

void Solver01::solve() {
    const int WINDOW_SIZE = 3;
    string line;
    int previous = INT_MAX;
    int score = 0;
    deque<int> window;
    for(string line: lines) {
        window.push_back(stoi(line));
        if (window.size() == WINDOW_SIZE)
        {
            int current = accumulate (window.begin(), window.end(), decltype(window)::value_type(0));
            if(current > previous) score++;
            previous = current;
            window.pop_front();
        }
    }

    cout << score;
}
