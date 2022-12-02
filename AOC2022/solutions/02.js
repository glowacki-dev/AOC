var _ = require("lodash");

class Solver {
  constructor(data) {
    this.data = [];
    data.forEach((line) => {
      if (line !== "") this.data.push(line.split(" "));
    });
    this.outcomes = {
      X: {
        A: 3 + 0,
        B: 1 + 0,
        C: 2 + 0,
      },
      Y: {
        A: 1 + 3,
        B: 2 + 3,
        C: 3 + 3,
      },
      Z: {
        A: 2 + 6,
        B: 3 + 6,
        C: 1 + 6,
      },
    };
  }

  run() {
    var score = 0;
    this.data.forEach(([enemy, own]) => {
      score += this.outcomes[own][enemy];
    });
    return score;
  }
}

module.exports = Solver;
