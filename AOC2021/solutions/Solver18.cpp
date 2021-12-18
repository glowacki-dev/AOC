#include "Solver18.h"

class Node {
public:
    int value;
    Node* left;
    Node* right;
    Node* parent;

    Node(Node* up, int val = -1) {
        parent = up;
        left = NULL;
        right = NULL;
        value = val;
    }

    int depth() {
        if(parent == NULL) return 0;
        else return parent->depth() + 1;
    }

    void fill(string input) {
        Node* current = this;
        current->parent = NULL;
        for(char c: input) {
            switch (c) {
                case '[': // Nest left
                    if(current->left) current = left;
                    else {
                        current->left = new Node(current);
                        current->right = new Node(current);
                        current = current->left;
                    }
                    break;
                case ']': // Unnest
                    current = current->parent;
                    break;
                case ',': // Move from left to right
                    current = current->parent->right; // Already created when nesting left
                    break;
                default: // Number
                    if(current->value == -1) {
                        //first digit
                        current->value = (c - '0');
                    } else {
                        // more digits (for testing)
                        current->value *= 10;
                        current->value += (c - '0');
                    }
                    break;
            }
        }
    }

    bool reduce() {
        deque<Node*> bfs;
        stack<Node*> dfs;
        deque<Node*> level_leafs;
        deque<Node*> order_leafs;
        bfs.push_back(this);
        dfs.push(this);
        while(bfs.size() > 0) {
            Node* current = bfs.front();
            bfs.pop_front();
            if(current->value != -1) {
                level_leafs.push_back(current);
            } else {
                bfs.push_back(current->left);
                bfs.push_back(current->right);
            }
        }
        while(dfs.size() > 0) {
            Node* current = dfs.top();
            dfs.pop();
            if(current->value != -1) {
                order_leafs.push_back(current);
            } else {
                dfs.push(current->right);
                dfs.push(current->left);
            }
        }
        auto it = find_if(level_leafs.begin(), level_leafs.end(), [](Node* node) { return node->depth() > 4 && node->value != -1; });
        if(it != level_leafs.end()) {
            // explode
            auto first_it = find(order_leafs.begin(), order_leafs.end(), *it);
            auto second_it = next(first_it);
            Node* first = (*first_it);
            Node* second = (*second_it);
            //cout << "EXPLODE " << first->value << "," << second->value << endl;
            if(first_it != order_leafs.begin()) {
                auto prev_it = prev(first_it);
                Node* previous = (*prev_it);
                previous->value += first->value;
            }
            auto next_it = next(second_it);
            if(next_it != order_leafs.end()) {
                Node* next = (*next_it);
                next->value += second->value;
            }
            Node* parent = first->parent;
            parent->left = NULL;
            parent->right = NULL;
            parent->value = 0;
            return true;
        } else {
            it = find_if(order_leafs.begin(), order_leafs.end(), [](Node* node) { return node->value >= 10; });
            if(it != order_leafs.end()) {
                // split
                Node* split = *it;
                //cout << "SPLIT " << split->value << endl;
                int left_value = split->value / 2;
                int right_value = split->value - left_value;
                split->left = new Node(split, left_value);
                split->right = new Node(split, right_value);
                split->value = -1;
                return true;
            }
        }
        return false;
    }

    int magnitude() {
        if(left == NULL) return value;
        return 3 * left->magnitude() + 2 * right->magnitude();
    }

    string preview() {
        if(value != -1) return to_string(value);
        return "[" + left->preview() + "," + right->preview() + "]";
    }
};

void Solver18::solve() {
    deque<Node*> trees;
    vector<Node*> permutations;
    for(string line: lines) {
        Node* root = new Node(NULL);
        root->fill(line);
        trees.push_back(root);
        for(string second_line: lines) {
            if(line == second_line) continue;
            Node* first = new Node(NULL);
            first->fill(line);
            Node* second = new Node(NULL);
            second->fill(second_line);
            Node* sum = new Node(NULL);
            sum->left = first;
            first->parent = sum;
            sum->right = second;
            second->parent = sum;
            permutations.push_back(sum);
        }
    }
    while(trees.size() > 1) {
        Node* sum = new Node(NULL);
        sum->left = trees.front();
        sum->left->parent = sum;
        trees.pop_front();
        sum->right = trees.front();
        sum->right->parent = sum;
        trees.pop_front();
        while(sum->reduce());
        trees.push_front(sum);
    }
    while(trees.front()->reduce()); // most likely not needed
    cout << trees.front()->preview() << endl;
    cout << trees.front()->magnitude() << endl;
    int largest_magnitude = 0;
    for(auto tree: permutations) {
        while(tree->reduce());
        if(tree->magnitude() > largest_magnitude) {
            largest_magnitude = tree->magnitude();
            cout << "New largest magnitude: " << largest_magnitude << " for: " << tree->preview() << endl;
        }
    }
}

