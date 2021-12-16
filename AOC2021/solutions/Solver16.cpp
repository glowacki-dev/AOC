#include "Solver16.h"

class Packet {
    vector<Packet> sub_packets;
    string input;
    int version;
    int type;
    unsigned long long value;
    bool calculated = false;

public:
    int size;

    Packet(string input, bool binary = false) {
        this->input = binary ? input : hex_to_binary(input);
        this->size = build_packets();
    }

    int versions_sum() {
        int sum = version;
        for(auto packet: sub_packets) {
            sum += packet.versions_sum();
        }
        return sum;
    }

    unsigned long long calculate() {
        if(calculated) return value;
        switch (type) {
            case 0:
                value = accumulate(sub_packets.begin(), sub_packets.end(), 0ull, [](unsigned long long acc, Packet p) { return acc + p.calculate(); });
                break;
            case 1:
                value = accumulate(sub_packets.begin(), sub_packets.end(), 1ull, [](unsigned long long acc, Packet p) { return acc * p.calculate(); });
                break;
            case 2:
                value = min_element(sub_packets.begin(), sub_packets.end(), [](Packet a, Packet b) { return a.calculate() < b.calculate(); })->calculate();
                break;
            case 3:
                value = max_element(sub_packets.begin(), sub_packets.end(), [](Packet a, Packet b) { return a.calculate() < b.calculate(); })->calculate();
                break;
            case 4:
                break;
            case 5:
                value = sub_packets[0].calculate() > sub_packets[1].calculate() ? 1 : 0;
                break;
            case 6:
                value = sub_packets[0].calculate() < sub_packets[1].calculate() ? 1 : 0;
                break;
            case 7:
                value = sub_packets[0].calculate() == sub_packets[1].calculate() ? 1 : 0;
                break;
            default:
                throw runtime_error("Unknown type");
        }
        calculated = true;
        return value;
    }

private:
    string hex_to_binary(string hex) {
        const string lut[] = { "0000", "0001", "0010", "0011", "0100", "0101", "0110", "0111", "1000", "1001", "1010", "1011", "1100", "1101", "1110", "1111" };
        string binary = "";
        for(char c: hex) {
            if(c >= '0' && c <= '9') binary.append(lut[c - '0']);
            else binary.append(lut[10 + c - 'A']);
        }
        return binary;
    }

    int build_packets() {
        int start = 0;
        version = parse(start, 3);
        type = parse(start, 3);
        int type_id = 0;
        switch (type) {
            case 4:
                value = read_value(start, (int)input.length() - start);
                return start;
            default:
                type_id = parse(start, 1);
                break;
        }
        int length, sub_packets_count;
        switch (type_id) {
            case 0:
                length = parse(start, 15);
                while(length > 0) {
                    Packet sub = Packet(input.substr(start, (int)input.length() - start), true);
                    sub_packets.push_back(sub);
                    start += sub.size;
                    length -= sub.size;
                }
                break;
            case 1:
                sub_packets_count = parse(start, 11);
                while(sub_packets_count > 0) {
                    Packet sub = Packet(input.substr(start, (int)input.length() - start), true);
                    sub_packets.push_back(sub);
                    start += sub.size;
                    sub_packets_count -= 1;
                }
                break;
            default:
                throw runtime_error("Unknown type_id");
        }
        return start;
    }

    int parse(int &start, int length) {
        string sub = input.substr(start, length);
        start += length;
        return stoi(sub, 0, 2);
    }

    long read_value(int &start, int length) {
        string sub = input.substr(start, length);
        string value = "";
        int i = -5;
        do {
            i += 5;
            value.append(sub.substr(i + 1, 4));
        } while(sub[i] != '0');
        start += i + 5;
        return stol(value, 0, 2);
    }
};

void Solver16::solve() {
    for(string line: lines) {
        Packet p = Packet(line);
        cout << line << " : " << p.versions_sum() << " : " << p.calculate() << endl;
    }
}

