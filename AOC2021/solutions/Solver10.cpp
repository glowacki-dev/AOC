#include "Solver10.h"

void Solver10::solve() {
    int p1_score = 0;
    vector<long> p2_scores;
    map<char, tuple<char, int, int>> ops;
    ops[')'] = make_tuple('(', 3, 1);
    ops[']'] = make_tuple('[', 57, 2);
    ops['}'] = make_tuple('{', 1197, 3);
    ops['>'] = make_tuple('<', 25137, 4);

    for(string line: lines) {
        stack<char> input;
        int mismatched = 0;
        for(char c: line) {
            try {
                auto op = ops.at(c);
                if(input.top() == get<0>(op)) input.pop();
                else mismatched = get<1>(op);
            } catch (const std::out_of_range& oor) {
                input.push(c);
            }
            if(mismatched != 0) break;
        }

        if(mismatched != 0) p1_score += mismatched;
        else {
            long score = 0;
            while(input.size() > 0) {
                score *= 5;
                auto op = find_if(ops.begin(), ops.end(), [input](auto op) { return get<0>(op.second) == input.top(); });
                score += get<2>(op->second);
                input.pop();
            }
            p2_scores.push_back(score);
        }
    }

    cout << p1_score << endl;
    sort(p2_scores.begin(), p2_scores.end());
    cout << p2_scores[p2_scores.size() / 2] << endl;
}
