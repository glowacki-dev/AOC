#include "Solver01.h"
#include <iostream>

int main(int argc, const char * argv[]) {
    Solver01 solver("01.txt", true);
    solver.solve();
    std::cout << std::endl;
    return 0;
}
