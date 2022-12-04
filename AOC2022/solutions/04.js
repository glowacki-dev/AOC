var _ = require("lodash");

class Solver {
  constructor(data) {
    this.data = [];
    data.forEach((line) => {
      if (line !== "")
        this.data.push(line.split(",").map((range) => range.split("-")));
    });
  }

  run() {
    var score = 0;
    this.data.forEach(([first, second]) => {
      let firstRange = _.range(first[0], Number(first[1]) + 1);
      let secondRange = _.range(second[0], Number(second[1]) + 1);
      let intersection = _.intersection(firstRange, secondRange);
      if (
        intersection.length === firstRange.length ||
        intersection.length === secondRange.length
      )
        score++;
    });
    return score;
  }
}

module.exports = Solver;
