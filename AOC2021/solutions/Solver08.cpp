#include "Solver08.h"

class Segment {
    map<int, vector<char>> samples;
    map<char, char> mapping;
    vector<vector<char>> data;
    vector<vector<char>> output;
public:
    Segment(string input) {
        string tmp;
        stringstream ss(input);
        bool separator = false;
        while(getline(ss, tmp, ' ')) {
            vector<char> v(tmp.begin(), tmp.end());
            sort(v.begin(), v.end());
            if(tmp == "|") separator = true;
            else if(separator) output.push_back(v);
            else data.push_back(v);
        }
    }

    void process() {
        find_uniques();
        find_9();
        find_6();
        find_0();
        fill_mappings_without_b();
        construct_2_and_3();
        fill_mapping_b();
        construct_5();
    }

    int decode() {
        int result = 0;
        for(vector<char> value: output) {
            for(auto &it : samples) {
                if(it.second == value) {
                    result *= 10;
                    result += it.first;
                    break;
                }
            }
        }
        return result;
    }

private:
    typedef vector<char>::iterator iter;
    typedef iter setFunction (iter, iter, iter, iter, iter);
    vector<char> samples_compute(vector<char> first, vector<char> second, int expected_length, setFunction func) {
        vector<char> result(7);
        auto iterator = func(first.begin(), first.end(), second.begin(), second.end(), result.begin());
        result.resize(iterator - result.begin());
        if(expected_length >= 0 && result.size() != expected_length) throw runtime_error("result size mismatch");
        return result;
    }

    vector<char> samples_union(int first, int second, int expected_length) {
        return samples_compute(samples[first], samples[second], expected_length, &set_union);
    }

    vector<char> samples_diff(vector<char> first, vector<char> second, int expected_length) {
        return samples_compute(first, second, expected_length, &set_difference);
    }
    char samples_diff(int first, int second) {
        return samples_diff(samples[first], samples[second], 1)[0];
    }

    vector<char> samples_intersection(vector<char> first, vector<char> second, int expected_length) {
        return samples_compute(first, second, expected_length, &set_intersection);
    }
    char samples_intersection(int first, int second) {
        return samples_intersection(samples[first], samples[second], 1)[0];
    }

    vector<char> compile(vector<char> input) {
        vector<char> result;
        for(char c: input) {
            result.push_back(mapping[c]);
        }
        sort(result.begin(), result.end());
        return result;
    }

    void find_uniques() {
        for(auto s: data) {
            switch (s.size()) {
                case 2:
                    samples[1] = s;
                    break;
                case 3:
                    samples[7] = s;
                    break;
                case 4:
                    samples[4] = s;
                    break;
                case 7:
                    samples[8] = s;
                    break;
                default:
                    break;
            }
        }
        remove(data.begin(), data.end(), samples[1]);
        remove(data.begin(), data.end(), samples[7]);
        remove(data.begin(), data.end(), samples[4]);
        remove(data.begin(), data.end(), samples[8]);
    }
    void find_9() {
        auto result = samples_union(7, 4, 5);
        samples[9] = *find_if(data.begin(), data.end(), [this, result](vector<char> entry) { return entry.size() == 6 && this->samples_diff(entry, result, -1).size() == 1; });
        remove(data.begin(), data.end(), samples[9]);
    }
    void find_6() {
        samples[6] = *find_if(data.begin(), data.end(), [this](vector<char> entry) { return entry.size() == 6 && this->samples_intersection(entry, samples[1], -1).size() == 1; });
        remove(data.begin(), data.end(), samples[6]);
    }
    void find_0() {
        samples[0] = *find_if(data.begin(), data.end(), [](vector<char> entry) { return entry.size() == 6; });
        remove(data.begin(), data.end(), samples[0]);
    }
    void fill_mappings_without_b() {
        mapping['a'] = samples_diff(7, 1);
        mapping['c'] = samples_diff(8, 6);
        mapping['d'] = samples_diff(8, 0);
        mapping['e'] = samples_diff(8, 9);
        mapping['f'] = samples_intersection(6, 1);
        mapping['g'] = samples_diff(samples[9], samples_union(7, 4, 5), 1)[0];
    }
    void construct_2_and_3() {
        samples[2] = compile(vector<char>{ 'a', 'c', 'd', 'e', 'g' });
        remove(data.begin(), data.end(), samples[2]);
        samples[3] = compile(vector<char>{ 'a', 'c', 'd', 'f', 'g' });
        remove(data.begin(), data.end(), samples[3]);
    }
    void fill_mapping_b() {
        mapping['b'] = samples_diff(4, 3);
    }
    void construct_5() {
        samples[5] = compile(vector<char>{ 'a', 'b', 'd', 'f', 'g' });
        remove(data.begin(), data.end(), samples[5]);
    }
};

void Solver08::solve() {
    int result = 0;
    for(string line: lines) {
        Segment segment = Segment(line);
        segment.process();
        int r = segment.decode();
        cout << line << " : " << r << endl;
        result += r;
    }
    cout << result << endl;
}
