#include "Solver02.h"
#include <iostream>

int main(int argc, const char * argv[]) {
    Solver02 solver("02.txt", true);
    solver.solve();
    std::cout << std::endl;
    return 0;
}
