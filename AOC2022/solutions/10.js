var _ = require("lodash");
const path = require("path");

class Solver {
  constructor(data, preview) {
    this.data = [];
    this.preview = preview;
    data.forEach((line) => {
      if (line !== "") this.data.push(line.split(" "));
    });
  }

  run() {
    let history = [null, 1];
    const checks = [20, 60, 100, 140, 180, 220];
    this.preview(this.data);
    this.data.forEach(([cmd, arg]) => {
      switch (cmd) {
        case "noop":
          history.push(history[history.length - 1]);
          break;
        case "addx":
          history.push(history[history.length - 1]);
          history.push(history[history.length - 1] + Number(arg));
          break;
        default:
          break;
      }
    });
    this.preview(history);
    return _.sum(checks.map((val) => val * history[val]));
  }
}

module.exports = Solver;
