#include "Solver06.h"

void Solver06::solve() {
    string tmp;
    stringstream ss(lines[0]);
    vector<int> tmp_olds(7, 0);

    while(getline(ss, tmp, ',')) {
        if(tmp.length() > 0)
        {
            tmp_olds[stoi(tmp)] += 1;
        }
    }

    deque<long> news(9, 0), olds(tmp_olds.begin(), tmp_olds.end());

    for(int i = 0; i < 256; i++) {
        long spawns = olds.front() + news.front();
        olds.push_back(spawns);
        news.push_back(spawns);
        olds.pop_front();
        news.pop_front();
    }
    cout << accumulate(olds.begin(), olds.end(), decltype(olds)::value_type(0)) + accumulate(news.begin(), news.end(), decltype(news)::value_type(0)) << endl;
}
