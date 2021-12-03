#include "Solver03.h"
#include <iostream>

int main(int argc, const char * argv[]) {
    Solver03 solver("03.txt", true);
    solver.solve();
    std::cout << std::endl;
    return 0;
}
