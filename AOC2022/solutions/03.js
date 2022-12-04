var _ = require("lodash");

class Solver {
  constructor(data) {
    this.data = [];
    data.forEach((line) => {
      if (line !== "") this.data.push(line.split(""));
    });
  }

  run() {
    var score = 0;
    var counter = 0;
    var arrays = [];
    this.data.forEach((arr) => {
      counter++;
      arrays.push(arr);
      if (counter === 3) {
        let common = _.intersection(...arrays);
        if (common.length !== 1) console.error("Something is wrong");
        common
          .map((letter) => {
            if (letter >= "A" && letter <= "Z")
              return letter.charCodeAt(0) - "A".charCodeAt(0) + 27;
            else return letter.charCodeAt(0) - "a".charCodeAt(0) + 1;
          })
          .forEach((s) => (score += s));
        counter = 0;
        arrays = [];
      }
    });
    return score;
  }
}

module.exports = Solver;
