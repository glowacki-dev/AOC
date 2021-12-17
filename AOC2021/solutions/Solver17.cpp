#include "Solver17.h"

int y(int y0, int n) {
    return n * (y0 + y0 - n + 1) / 2;
}

int x(int x0, int n) {
    if(n >= x0) return x0 * (x0 + 1) / 2;
    return n * (x0 + x0 - n + 1) / 2;
}

void Solver17::solve() {
    int y0 = -162;
    int y1 = -134; // y can go from -162 (furthest direct shot) to 161 (highest trick shot from p1)
    int x0 = 56;
    int x1 = 76; // x  can g from 11 (min value that can reach at least 56) to 76 (furthest direct shot)
    int count = 0;

    /*
    // Part 1 - manually stop execution when no new solution for a while xD
    long long max_h = 0;
    long long current = 0;
    long long temp_max = 0;
    int speed = 0;
    int n = 0;
    for(int i = 1; i < INT_MAX; i++) {
        current = 0;
        temp_max = 0;
        speed = i;
        n = 0;
        while(current > y0) {
            current += speed;
            temp_max = max(temp_max, current);
            speed -= 1;
            n++;
            if(current <= y1 && current >= y0) {
                if(temp_max > max_h) max_h = temp_max;
                cout << "y0: " << i << " ended at: " << current << " max_h: " << max_h << " after: " << n << endl;
                break;
            }
        }
    }
    cout << max_h << endl;
    */
    // Part 2 - initial values set manually ¯\_(ツ)_/¯
    for(int xt = 11; xt <= 76; xt++) {
        for(int yt = -162; yt <= 161; yt++) {
            for(int n = 1; n <= 324; n++) { // 324 turns was the longest trickshot, but this should break automatically anyway
                int xx = x(xt, n);
                int yy = y(yt, n);
                cout << "x: " << xt << " y: " << yt << " n: " << n << " pos: " << xx << "," << yy;
                if(xx > x1 || yy < y0) {
                    cout << endl;
                    break;
                }
                if(xx >= x0 && xx <= x1 && yy >= y0 && yy <= y1) {
                    count++;
                    cout << " +" << endl;
                    break;
                }
                cout << endl;
            }
        }
    }
    cout << count << endl;
}

