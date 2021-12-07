#include "Solver07.h"

int fast_partial_sum(int n) {
    return (n * (n + 1))/2;
}

void Solver07::solve() {
    string tmp;
    stringstream ss(lines[0]);
    vector<int> positions;

    while(getline(ss, tmp, ',')) {
        if(tmp.length() > 0)
        {
            positions.push_back(stoi(tmp));
        }
    }

    auto bounds = minmax_element(positions.begin(), positions.end());

    int min_score = INT_MAX;
    int min_position = INT_MAX;

    for(int i = *bounds.first; i <= *bounds.second; i++) {
        int score = accumulate(positions.begin(), positions.end(), 0, [i](int acc, int position){ return acc + fast_partial_sum(abs(i - position)); });
        if(score < min_score) {
            min_score = score;
            min_position = i;
        }
    }

    cout << min_score << ":" << min_position << endl;
}
