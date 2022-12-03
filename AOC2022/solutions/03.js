var _ = require("lodash");

class Solver {
  constructor(data) {
    this.data = [];
    data.forEach((line) => {
      if (line !== "") this.data.push(_.chunk(line.split(""), line.length / 2));
    });
  }

  run() {
    var score = 0;
    this.data.forEach(([left, right]) => {
      let common = _.intersection(left, right);
      common
        .map((letter) => {
          if (letter >= "A" && letter <= "Z")
            return letter.charCodeAt(0) - "A".charCodeAt(0) + 27;
          else return letter.charCodeAt(0) - "a".charCodeAt(0) + 1;
        })
        .forEach((s) => (score += s));
    });
    return score;
  }
}

module.exports = Solver;
