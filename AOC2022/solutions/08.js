var _ = require("lodash");
const path = require("path");

class Solver {
  constructor(data, preview) {
    this.data = [];
    this.preview = preview;
    data.forEach((line) => {
      if (line !== "")
        this.data.push(
          line.split("").map((val) => {
            return { value: Number(val), score: 0 };
          })
        );
    });
  }

  find_score(center_y, center_x) {
    let size = this.data.length;
    let scores = [0, 0, 0, 0];
    let target = this.data[center_y][center_x].value;
    for (let x = center_x + 1; x < size; x++) {
      scores[0] += 1;
      if (this.data[center_y][x].value >= target) {
        break;
      }
    }

    for (let x = center_x - 1; x >= 0; x--) {
      scores[1] += 1;
      if (this.data[center_y][x].value >= target) {
        break;
      }
    }

    for (let y = center_y + 1; y < size; y++) {
      scores[2] += 1;
      if (this.data[y][center_x].value >= target) {
        break;
      }
    }

    for (let y = center_y - 1; y >= 0; y--) {
      scores[3] += 1;
      if (this.data[y][center_x].value >= target) {
        break;
      }
    }

    return _.reduce(scores, _.multiply, 1);
  }

  run() {
    let size = this.data.length;
    for (let y = 0; y < size; y++) {
      for (let x = 0; x < size; x++) {
        this.data[y][x].score = this.find_score(y, x);
      }
    }
    this.preview(this.data);
    return _.maxBy(_.flattenDeep(this.data), "score").score;
  }
}

module.exports = Solver;
