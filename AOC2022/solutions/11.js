var _ = require("lodash");
const path = require("path");

class Monkey {
  constructor() {
    this.counter = 0;
    this.targets = {};
  }

  take_turn(monkeys) {
    this.items.forEach((item) => {
      this.counter += 1;
      // Calculate new level
      item = this.op_fn(item);
      // Decrease
      item = Math.floor(item / 3);
      // Move away
      if (item % this.test === 0) {
        monkeys[this.targets.true].items.push(item);
      } else {
        monkeys[this.targets.false].items.push(item);
      }
    });
    this.items = [];
  }
}

class Solver {
  constructor(data, preview) {
    this.monkeys = [];
    this.preview = preview;
    data.forEach((line) => {
      if (line !== "") {
        let tokens = _.compact(line.split(" "));
        let last_monkey = _.last(this.monkeys);
        this.preview(tokens);
        switch (tokens[0]) {
          case "Monkey":
            this.monkeys.push(new Monkey());
            break;
          case "Starting":
            last_monkey.items = tokens
              .slice(2)
              .map((val) => Number(_.trim(val, ",")));
            break;
          case "Operation:":
            last_monkey.op_fn = (old) => eval(tokens.slice(3).join(" "));
            break;
          case "Test:":
            last_monkey.test = Number(tokens[3]);
            break;
          case "If":
            tokens[1] === "true:"
              ? (last_monkey.targets.true = Number(tokens[5]))
              : (last_monkey.targets.false = Number(tokens[5]));
            break;
        }
      }
    });
  }

  run() {
    this.preview(this.monkeys);
    for (let i = 0; i < 20; i++) {
      this.monkeys.forEach((monkey) => {
        monkey.take_turn(this.monkeys);
      });
    }
    this.preview(this.monkeys);
    return _.reduce(
      _.sortBy(this.monkeys, "counter").slice(-2),
      (acc, obj) => acc * obj.counter,
      1
    );
  }
}

module.exports = Solver;
