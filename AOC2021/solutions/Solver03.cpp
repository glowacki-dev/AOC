#include "Solver03.h"

void Solver03::solve() {
    string gamma = lines.front();
    string epsilon = lines.front();
    string most_common = "";
    string least_common = "";
    for(int i = 0; i < gamma.length(); i++) {
        long ones = count_if(lines.begin(), lines.end(), [i](string line) { return line[i] == '1'; });
        long most_common_match = count_if(lines.begin(), lines.end(), [i, most_common](string line) { return line.rfind(most_common, 0) == 0; });
        long most_common_ones = count_if(lines.begin(), lines.end(), [i, most_common](string line) { return line.rfind(most_common, 0) == 0 && line[i] == '1'; });
        long least_common_match = count_if(lines.begin(), lines.end(), [i, least_common](string line) { return line.rfind(least_common, 0) == 0; });
        long least_common_ones = count_if(lines.begin(), lines.end(), [i, least_common](string line) { return line.rfind(least_common, 0) == 0 && line[i] == '1'; });

        if(ones > lines.size() / 2) {
            gamma[i] = '1';
            epsilon[i] = '0';
        } else {
            gamma[i] = '0';
            epsilon[i] = '1';
        }

        if(most_common_match == 1) {
            most_common = *find_if(lines.begin(), lines.end(), [most_common](string line) { return line.rfind(most_common, 0) == 0; });
        } else {
            if(2 * most_common_ones >= most_common_match) {
                most_common.append("1");
            } else {
                most_common.append("0");
            }
        }
        
        if(least_common_match == 1) {
            least_common = *find_if(lines.begin(), lines.end(), [least_common](string line) { return line.rfind(least_common, 0) == 0; });
        } else {
            if(2 * least_common_ones >= least_common_match) {
                least_common.append("0");
            } else {
                least_common.append("1");
            }
        }
    }

    cout << most_common << endl << least_common << endl;
    int gamma_i = stoi(gamma, nullptr, 2);
    int epsilon_i = stoi(epsilon, nullptr, 2);
    int most_common_i = stoi(most_common, nullptr, 2);
    int least_common_i = stoi(least_common, nullptr, 2);
    int power_consumption = gamma_i * epsilon_i;
    int life_support = most_common_i * least_common_i;
    cout << gamma_i << endl << epsilon_i << endl << power_consumption << endl << most_common_i << endl << least_common_i << endl << life_support << endl;
}
