var _ = require("lodash");
let createGraph = require("ngraph.graph");
let path = require("ngraph.path");

class Solver {
  constructor(data, preview) {
    let guides = [];
    this.map = {};
    this.preview = preview;
    data.forEach((line) => {
      if (line !== "")
        guides.push(
          line.split(" -> ").map((val) => {
            return {
              x: Number(val.split(",")[0]),
              y: Number(val.split(",")[1]),
              type: "rock",
            };
          })
        );
    });
    guides.forEach((lineset) => {
      for (let i = 1; i < lineset.length; i++) {
        let left = lineset[i - 1];
        let right = lineset[i];
        if (left.x === right.x) {
          for (
            let j = Math.min(left.y, right.y);
            j <= Math.max(left.y, right.y);
            j++
          ) {
            _.setWith(this.map, [j, left.x], "#", Object);
          }
        } else {
          for (
            let j = Math.min(left.x, right.x);
            j <= Math.max(left.x, right.x);
            j++
          ) {
            _.setWith(this.map, [left.y, j], "#", Object);
          }
        }
      }
    });
    this.floor = Math.max(...Object.keys(this.map).map((v) => Number(v)));
    this.preview(this.floor);
    this.preview(this.map);
  }

  simulate_sand() {
    let y = 0;
    let x = 500;
    while (true) {
      if (y > this.floor) return false;
      if (_.get(this.map, [y + 1, x]) === undefined) {
        y = y + 1;
      } else if (_.get(this.map, [y + 1, x - 1]) === undefined) {
        y = y + 1;
        x = x - 1;
      } else if (_.get(this.map, [y + 1, x + 1]) === undefined) {
        y = y + 1;
        x = x + 1;
      } else {
        _.setWith(this.map, [y, x], "o", Object);
        return true;
      }
    }
  }

  run() {
    let score = 0;
    while (true) {
      if (!this.simulate_sand()) break;
      else score += 1;
    }
    this.preview(this.map);
    return score;
  }
}

module.exports = Solver;
