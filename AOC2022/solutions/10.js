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
    this.preview(this.data);
    this.data.forEach(([cmd, arg]) => {
      switch (cmd) {
        case "noop":
          history.push(_.last(history));
          break;
        case "addx":
          history.push(_.last(history));
          history.push(_.last(history) + Number(arg));
          break;
      }
    });
    this.preview(history);
    let result = "";
    for (let i = 1; i <= 240; i++) {
      if (_.inRange((i - 1) % 40, (history[i] % 40) - 1, (history[i] % 40) + 2))
        result += "#";
      else result += " ";
      if (i % 40 === 0) result += "\n";
    }
    return result;
  }
}

module.exports = Solver;
