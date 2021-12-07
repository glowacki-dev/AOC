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

    int bound_min = *bounds.first;
    int bound_max = *bounds.second;

    while(bound_min != bound_max) {
        int score_min = 0, score_max = 0;
        for(int position: positions) {
            score_min += fast_partial_sum(abs(bound_min - position));
            score_max += fast_partial_sum(abs(bound_max - position));
        }

        if(score_max > score_min) {
            bound_max = floor((bound_max + bound_min) / 2.0);
        } else {
            bound_min = ceil((bound_max + bound_min) / 2.0);
        }
    }

    int min_score = accumulate(positions.begin(), positions.end(), 0, [bound_min](int acc, int position){ return acc + fast_partial_sum(abs(bound_min - position)); });

    cout << min_score << ":" << bound_min << endl;
}
