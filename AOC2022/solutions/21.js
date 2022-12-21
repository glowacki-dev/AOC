var _ = require("lodash");

class Solver {
  constructor(data, preview) {
    this.monkeys = {};
    this.preview = preview;
    data.forEach((line) => {
      if (line !== "") {
        let [name, action] = line.split(": ");
        if (action.includes(" ")) {
          let [left, op, right] = action.split(" ");
          this.monkeys[name] = { left, op, right };
        } else {
          this.monkeys[name] = { value: Number(action) };
        }
      }
    });
    this.preview(this.monkeys);
  }

  calculate(name) {
    if (this.monkeys[name].value !== undefined) {
      return this.monkeys[name].value;
    }

    let left = this.calculate(this.monkeys[name].left);
    let right = this.calculate(this.monkeys[name].right);
    this.monkeys[name].value = eval(
      `${left} ${this.monkeys[name].op} ${right}`
    );
    return this.monkeys[name].value;
  }

  run() {
    return this.calculate("root");
  }
}

module.exports = Solver;
