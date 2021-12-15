#pragma once
#include <algorithm>
#include <climits>
#include <deque>
#include <fstream>
#include <iostream>
#include <list>
#include <map>
#include <numeric>
#include <queue>
#include <regex>
#include <set>
#include <sstream>
#include <stack>
#include <string>
#include <vector>

using namespace std;

class BaseSolver {
public:
    BaseSolver(string);
    BaseSolver(string, bool);
    ~BaseSolver();
    virtual void solve() = 0;
protected:
    ifstream input;
    vector<string> lines;
private:
    void read_stream(bool);
};
