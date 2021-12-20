#include "Solver20.h"

class Image {
    vector<int> algorithm;
    vector<int> image;
    long size;
    int pad_value; // If algorithm begins with 1, then entire backgroud will flip colors each enhancement
public:
    Image(string algorithm) {
        for(char c: algorithm) {
            if(c == '#') this->algorithm.push_back(1);
            else this->algorithm.push_back(0);
        }
        pad_value = 0;
    }

    void load_data(string line) {
        if(image.empty()) {
            size = line.length();
        }
        for(char c: line) {
            if(c == '#') image.push_back(1);
            else image.push_back(0);
        }
    }

    void enhance() {
        long new_size = size + 2; // Image can grows up to 2 pixels in each direction
        vector<int> new_image;
        for(int y = -1; y <= size; y++) { // Start 1 "above" and end 1 "below" image so we calculate borders
            for(int x = -1; x <= size; x++) {
                new_image.push_back(enhanced_pixel(x,y));
            }
        }
        size = new_size;
        image = new_image;
        if(algorithm[0] == 1 && pad_value == 0) pad_value = 1;
        else pad_value = 0;
    }

    long lit_count() {
        return count(image.begin(), image.end(), 1);
    }

    void preview() {
        for(int i = 0; i < image.size(); i++) {
            if(image[i] == 1) cout << "#";
            else cout << " ";
            if(i % size == size - 1) cout << endl;
        }
        cout << endl;
    }
private:
    int enhanced_pixel(int x, int y) {
        unsigned int index = enhanced_subpixel(x - 1, y - 1);
        index <<= 1;
        index += enhanced_subpixel(x, y - 1);
        index <<= 1;
        index += enhanced_subpixel(x + 1, y - 1);
        index <<= 1;
        index += enhanced_subpixel(x - 1, y);
        index <<= 1;
        index += enhanced_subpixel(x, y);
        index <<= 1;
        index += enhanced_subpixel(x + 1, y);
        index <<= 1;
        index += enhanced_subpixel(x - 1, y + 1);
        index <<= 1;
        index += enhanced_subpixel(x, y + 1);
        index <<= 1;
        index += enhanced_subpixel(x + 1, y + 1);
        return algorithm[index];
    }

    int enhanced_subpixel(int x, int y) {
        if(x < 0 || y < 0 || x >= size || y >= size) return pad_value;
        return image[size * y + x];
    }
};

void Solver20::solve() {
    Image img = Image(lines[0]);
    lines.erase(lines.begin());
    for(string line: lines) {
        img.load_data(line);
    }
    //img.preview();
    long lit_2 = 0;
    for(int i = 0; i < 50; i++) {
        if(i == 2) lit_2 = img.lit_count();
        img.enhance();
        //img.preview();
    }
    cout << lit_2 << endl;
    cout << img.lit_count() << endl;
}

