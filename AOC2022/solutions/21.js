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
    this.monkeys["root"].op = "=";
    this.monkeys["humn"].value = "x";
    this.preview(this.monkeys);
  }

  calculate(name) {
    let monkey = this.monkeys[name];
    if (monkey.value !== undefined) {
      return monkey.value;
    }

    let left = this.calculate(monkey.left);
    let right = this.calculate(monkey.right);
    let op = monkey.op;
    this.preview([name, left, right, op, typeof left, typeof right]);
    this.monkeys[name].value = `(${left} ${this.monkeys[name].op} ${right})`;
    if (typeof left === "number" && typeof right === "number") {
      monkey.value = eval(monkey.value);
    }
    return monkey.value;
  }

  run() {
    return this.calculate("root");
  }
}

module.exports = Solver;
