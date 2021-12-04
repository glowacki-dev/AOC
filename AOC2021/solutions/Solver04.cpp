#include "Solver04.h"

class BingoBoard {
public:
    BingoBoard(vector<string> lines) {
        size = (int)lines.size();
        for(string line: lines) {
            string tmp;
            stringstream ss(line);
            vector<string> words;

            while(getline(ss, tmp, ' ')) {
                if(tmp.length() > 0)
                {
                    matches.push_back(pair<string, bool>(tmp, false));
                }
            }
        }
    }

    void match(string value) {
        replace_if(matches.begin(), matches.end(), [value](pair<string, bool> elem){ return value == elem.first; }, pair<string, bool>(value, true));
    }

    vector<string> winning_match() {
        for(int i = 0; i < size; i++) {
            vector<string> match;
            for(int j = 0; j < size; j++) {
                if(matches[size * i + j].second) match.push_back(matches[size * i + j].first);
            }
            if(match.size() == size) return match;
        }
        for(int i = 0; i < size; i++) {
            vector<string> match;
            for(int j = 0; j < size; j++) {
                if(matches[i + size * j].second) match.push_back(matches[i + size * j].first);
            }
            if(match.size() == size) return match;
        }
        return {};
    }
    
    vector<string> unmatched() {
        vector<string> result;
        for(auto match: matches) {
            if(match.second == false) result.push_back(match.first);
        }
        return result;
    }
private:
    int size;
    vector<pair<string, bool>> matches;
};

void Solver04::solve() {
    deque<string> numbers;
    string current_number;
    
    string tmp;
    stringstream ss(lines[0]);

    while(getline(ss, tmp, ',')) {
        if(tmp.length() > 0)
        {
            numbers.push_back(tmp);
        }
    }

    lines.erase(lines.begin());
    lines.erase(lines.begin());

    vector<string> board;
    vector<BingoBoard> boards;
    for(string line: lines) {
        if(line.length() > 0) {
            board.push_back(line);
        } else {
            boards.push_back(BingoBoard(board));
            board.clear();
        }
    }
    if(!board.empty()) {
        boards.push_back(BingoBoard(board));
    }

    vector<int> rejected_boards;
    auto win_condition = [&rejected_boards](int size) { return rejected_boards.size() == size; };
    for(string number: numbers) {
        for(int i = 0; i < boards.size(); i++)
        {
            if(find(rejected_boards.begin(), rejected_boards.end(), i) != rejected_boards.end()) continue;
            boards[i].match(number);
            auto winning = boards[i].winning_match();
            if(!winning.empty()) {
                rejected_boards.push_back(i);
                // 1 - stage 1
                // (int)boards.size() - stage 1
                if(win_condition((int)boards.size())) {
                    int unmatched_sum = 0;
                    for(string val: boards[i].unmatched()) {
                        unmatched_sum += stoi(val);
                    }
                    cout << "Sum: " << unmatched_sum << endl;
                    cout << "Last number: " << number << endl;
                    cout << "Score: " << unmatched_sum * stoi(number) << endl;
                    return;
                }
            }
        }
    }
}
