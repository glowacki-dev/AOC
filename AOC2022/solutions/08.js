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
            return { value: Number(val), visible: false };
          })
        );
    });
  }

  run() {
    var score = 0;
    let size = this.data.length;
    for (let y = 0; y < size; y++) {
      let max_x = -1;
      for (let x = 0; x < size; x++) {
        if (this.data[y][x].value > max_x) {
          max_x = this.data[y][x].value;
          this.data[y][x].visible = true;
        }
      }

      max_x = -1;
      for (let x = size - 1; x >= 0; x--) {
        if (this.data[y][x].value > max_x) {
          max_x = this.data[y][x].value;
          this.data[y][x].visible = true;
        }
      }
    }

    this.preview(this.data);

    for (let x = 0; x < size; x++) {
      let max_y = -1;
      for (let y = 0; y < size; y++) {
        if (this.data[y][x].value > max_y) {
          max_y = this.data[y][x].value;
          this.data[y][x].visible = true;
        }
      }

      max_y = -1;
      for (let y = size - 1; y >= 0; y--) {
        if (this.data[y][x].value > max_y) {
          max_y = this.data[y][x].value;
          this.data[y][x].visible = true;
        }
      }
    }

    this.preview(this.data);
    score = _.filter(_.flattenDeep(this.data), { visible: true }).length;
    return score;
  }
}

module.exports = Solver;
