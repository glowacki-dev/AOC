#include "Solver03.h"

void Solver03::solve() {
    string gamma = lines.front();
    string epsilon = lines.front();
    vector<string> most_common = lines;
    vector<string> least_common = lines;
    int i;
    auto lambda = [&i](string line) { return line[i] == '1'; };
    for(i = 0; i < gamma.length(); i++) {
        long ones = count_if(lines.begin(), lines.end(), lambda);
        long most_common_ones = count_if(most_common.begin(), most_common.end(), lambda);
        long least_common_ones = count_if(least_common.begin(), least_common.end(), lambda);

        if(ones > lines.size() / 2) {
            gamma[i] = '1';
            epsilon[i] = '0';
        } else {
            gamma[i] = '0';
            epsilon[i] = '1';
        }

        if(2 * most_common_ones >= most_common.size()) {
            most_common.erase(remove_if(most_common.begin(), most_common.end(), [i](string line) { return line[i] == '0'; }), most_common.end());
        } else {
            most_common.erase(remove_if(most_common.begin(), most_common.end(), lambda), most_common.end());
        }

        if(2 * least_common_ones >= least_common.size()) {
            least_common.erase(remove_if(least_common.begin(), least_common.end(), lambda), least_common.end());
        } else {
            least_common.erase(remove_if(least_common.begin(), least_common.end(), [i](string line) { return line[i] == '0'; }), least_common.end());
        }
    }

    cout << most_common.front() << endl << least_common.front() << endl;
    int gamma_i = stoi(gamma, nullptr, 2);
    int epsilon_i = stoi(epsilon, nullptr, 2);
    int most_common_i = stoi(most_common.front(), nullptr, 2);
    int least_common_i = stoi(least_common.front(), nullptr, 2);
    int power_consumption = gamma_i * epsilon_i;
    int life_support = most_common_i * least_common_i;
    cout << gamma_i << endl << epsilon_i << endl << power_consumption << endl << most_common_i << endl << least_common_i << endl << life_support << endl;
}
