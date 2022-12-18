var _ = require("lodash");
let createGraph = require("ngraph.graph");
let path = require("ngraph.path");

class Solver {
  constructor(data, preview) {
    this.map = {};
    this.preview = preview;
    data.forEach((line) => {
      if (line !== "") this.map[line] = "#";
    });
    this.preview(this.map);
  }

  run() {
    let min = -1;
    let max =
      _.max(
        _.flatten(
          Object.keys(this.map).map((k) => k.split(",").map((v) => Number(v)))
        )
      ) + 1;
    this.preview(max);
    let queue = [];
    let initFill = [min, min, min];
    queue.push(initFill);
    this.map[initFill] = ".";
    while (queue.length > 0) {
      let current = queue.pop();
      this.map[current.join(",")] = ".";
      [
        [1, 0, 0],
        [-1, 0, 0],
        [0, 1, 0],
        [0, -1, 0],
        [0, 0, 1],
        [0, 0, -1],
      ].forEach((direction) => {
        let newCoordinates = _.zipWith(current, direction, (a, b) => a + b);
        if (
          _.every(newCoordinates, (val) => val >= min && val <= max) &&
          this.map[newCoordinates.join(",")] === undefined
        ) {
          queue.push(newCoordinates);
          this.map[newCoordinates] = ".";
        }
      });
    }
    let score = 0;
    Object.keys(this.map).forEach((key) => {
      if (this.map[key] !== "#") return;
      let coordinates = key.split(",").map((v) => Number(v));
      [
        [1, 0, 0],
        [-1, 0, 0],
        [0, 1, 0],
        [0, -1, 0],
        [0, 0, 1],
        [0, 0, -1],
      ].forEach((direction) => {
        if (
          this.map[
            _.zipWith(coordinates, direction, (a, b) => a + b).join(",")
          ] === "."
        )
          score += 1;
      });
    });
    return score;
  }
}

module.exports = Solver;
