#pragma once
#include <string>
#include <fstream>
#include <vector>

class BaseSolver {
public:
    BaseSolver(std::string);
    BaseSolver(std::string, bool);
    ~BaseSolver();
    virtual void solve() = 0;
protected:
    std::ifstream input;
    std::vector<std::string> lines;
private:
    void read_stream(bool);
};
