#include "Solver22.h"

class Instruction {
    vector<pair<long, long>> ranges;
    bool state;
public:
    Instruction(smatch sm) {
        if(sm.size() != 8) throw runtime_error("Invalid input");
        state = (sm[1] == "on");
        for (int i = 2; i < 8; i+=2) {
            ranges.push_back(pair<long, long>{stol(sm[i]),stol(sm[i+1])});
        }
    }

    void apply(vector<pair<vector<pair<long, long>>, long long>> &values) {
        vector<pair<vector<pair<long, long>>, long long>> new_cubes;
        if(state) {
            long count = volume(ranges);
            new_cubes.push_back(pair<vector<pair<long, long>>, long long> { ranges, count });
            for(auto cube: values) {
                auto ret = overlap(cube);
                if(ret.first) {
                    new_cubes.push_back(ret.second);
                }
            }
        } else {
            for(auto cube: values) {
                auto ret = overlap(cube);
                if(ret.first) {
                    new_cubes.push_back(ret.second);
                }
            }
        }
        for(auto cube: new_cubes) values.push_back(cube);
        long long turned_on = 0;
        for(auto value: values) {
            turned_on += value.second;
        }
    }

    pair<bool, pair<vector<pair<long, long>>, long long>> overlap(pair<vector<pair<long, long>>, long long> other_cube) {
        if(!overlap(other_cube.first)) return pair<bool, pair<vector<pair<long, long>>, long>>{ false, { } };
        vector<pair<long, long>> overlap_range = find_overlap_range(other_cube.first);
        long overlap_count = volume(overlap_range);
        if(other_cube.second > 0) overlap_count *= -1; // this overlap volume is used for offsetting, so it's negative for intersection with positive ones
        return pair<bool, pair<vector<pair<long, long>>, long long>>{ true, { overlap_range, overlap_count }};
    }

    bool overlap(vector<pair<long, long>> other_ranges) {
        return ranges[0].second >= other_ranges[0].first && ranges[0].first <= other_ranges[0].second &&
               ranges[1].second >= other_ranges[1].first && ranges[1].first <= other_ranges[1].second &&
               ranges[2].second >= other_ranges[2].first && ranges[2].first <= other_ranges[2].second;
    }

    vector<pair<long, long>> find_overlap_range(vector<pair<long, long>> other_range) {
        return vector<pair<long, long>> {
            {max(ranges[0].first, other_range[0].first), min(ranges[0].second, other_range[0].second)},
            {max(ranges[1].first, other_range[1].first), min(ranges[1].second, other_range[1].second)},
            {max(ranges[2].first, other_range[2].first), min(ranges[2].second, other_range[2].second)}
        };
    }

    long long volume(vector<pair<long, long>> other_range) {
        return (other_range[0].second + 1 - other_range[0].first) * (other_range[1].second + 1 - other_range[1].first) * (other_range[2].second + 1 - other_range[2].first);
    }
};

void Solver22::solve() {
    vector<Instruction> instructions;
    vector<pair<vector<pair<long, long>>, long long>> values;
    for(string line: lines) {
        smatch sm;
        regex_search(line, sm, regex("(on|off) x=(-?\\d+)..(-?\\d+),y=(-?\\d+)..(-?\\d+),z=(-?\\d+)..(-?\\d+)"));
        instructions.push_back(Instruction(sm));
    }
    for(Instruction in: instructions) {
        in.apply(values);
    }
    long long turned_on = 0;
    for(auto value: values) {
        turned_on += value.second;
    }
    cout << turned_on << endl;
}

