var _ = require("lodash");

class Solver {
  constructor(data) {
    this.stacks = [];
    this.moves = [];
    let mode = 0;
    data.forEach((line) => {
      if (mode === 0) {
        if (line === "") {
          for (var i = 0; i < this.stacks.length; ++i) {
            this.stacks[i] = this.stacks[i].slice(0, -1);
            this.stacks[i].reverse();
          }
          mode = 1;
          return;
        }
        let content = _.chunk(line, 4).map((chunk) => chunk[1]);
        if (this.stacks.length === 0) {
          this.stacks = new Array(content.length);
          for (var i = 0; i < this.stacks.length; ++i) this.stacks[i] = [];
        }
        content.forEach((value, index) => {
          if (value !== " ") {
            this.stacks[index].push(value);
          }
        });
      } else {
        if (line !== "") {
          let command = line.split(" ");
          this.moves.push([
            Number(command[1]),
            Number(command[3]) - 1,
            Number(command[5]) - 1,
          ]); // amount, from, to
        }
      }
    });
  }

  run() {
    this.moves.forEach(([amount, from, to]) => {
      let moved = [];
      for (var i = 0; i < amount; i++) {
        moved.push(this.stacks[from].pop());
      }
      moved.reverse();
      moved.forEach((el) => this.stacks[to].push(el));
    });
    return this.stacks.map((stack) => stack[stack.length - 1]).join("");
  }
}

module.exports = Solver;
