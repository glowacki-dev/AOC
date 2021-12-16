#include "Solver16.h"

int main(int argc, const char * argv[]) {
    Solver16 solver("16.txt", true);
    solver.solve();
    cout << endl;
    return 0;
}
