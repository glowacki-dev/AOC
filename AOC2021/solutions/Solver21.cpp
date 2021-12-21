#include "Solver21.h"

class Game {
    int turn = 0;
    int max_score;
public:
    vector<int> positions;
    vector<int> scores {0, 0};
    int current = 0;

    Game(int p0, int p1, int max_score) {
        positions.push_back(p0 - 1); // 0 - indexed
        positions.push_back(p1 - 1);
        this->max_score = max_score;
    }

    bool is_finished() {
        return any_of(scores.begin(), scores.end(), [this](int score) { return score >= max_score; });
    }

    void simulate(int dice) {
        turn += 1;
        positions[current] += dice;
        positions[current] %= 10;
        scores[current] += positions[current] + 1;
        if(current == 0) current = 1;
        else current = 0;
    }

    int score() {
        return (*min_element(scores.begin(), scores.end())) * turn * 3;
    }
};

void Solver21::solve() {
    int p1 = 8;
    int p2 = 9;

    // Part 1
    Game game = Game(p1, p2, 1000);
    int dice = 0;
    while(!game.is_finished()) {
        game.simulate(3 * dice + 6); // +1, +2, +3
        dice += 3;
    }
    cout << game.score() << endl;

    // Part 2
    map<int, int> rolls_variants = {
        {3, 1},
        {4, 3},
        {5, 6},
        {6, 7},
        {7, 6},
        {8, 3},
        {9, 1}
    };
    map<vector<int>, long long> active_games;
    active_games[vector<int>{ p1 - 1, p2 - 1, 0, 0, 0 }] = 1;
    auto sim = [](vector<int> input, int dice) {
        vector<int> new_state(input);
        new_state[input[4]] += dice;
        new_state[input[4]] %= 10;
        new_state[input[4] + 2] += new_state[input[4]] + 1;
        if(input[4] == 0) new_state[4] = 1;
        else new_state[4] = 0;
        return new_state;
    };
    while(true) {
        map<vector<int>, long long> new_games;
        int active_count = 0;
        for(auto game: active_games) {
            if(game.first[2] >= 21 || game.first[3] >= 21) new_games[game.first] += game.second; // game ended
            else {
                active_count += 1;
                for(pair<int, int> dice: rolls_variants) {
                    new_games[sim(game.first, dice.first)] += game.second * dice.second;
                }
            }
        }
        if(active_count == 0) break;
        active_games = new_games;
    }
    vector<long long> wins { 0, 0 };
    for (auto game: active_games) {
        if(game.first[2] >= 21) wins[0] += game.second;
        else wins[1] += game.second;
    }
    cout << wins[0] << endl << wins[1] << endl;
}

