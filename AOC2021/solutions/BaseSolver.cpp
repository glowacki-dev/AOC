#include "BaseSolver.h"
#include <iostream>

BaseSolver::BaseSolver(std::string input_name) : BaseSolver(input_name, false) {};

BaseSolver::BaseSolver(std::string input_name, bool cleanup) {
    input.open("/Users/mg/Documents/AOC/AOC2021/data/" + input_name);
    if(!input) throw std::runtime_error("Error opening file.");
    read_stream(cleanup);
    input.close();
}

BaseSolver::~BaseSolver(void) {
    input.close();
}

void BaseSolver::read_stream(bool cleanup) {
    std::string line;
    while (getline(input, line))
    {
        if(!cleanup || line.length() > 0) lines.push_back(line);
    }
}
