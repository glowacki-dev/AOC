var _ = require("lodash");
let createGraph = require("ngraph.graph");
let path = require("ngraph.path");

class Solver {
  constructor(data, preview) {
    this.pairs = [[]];
    this.preview = preview;
    data.forEach((line) => {
      if (line !== "") {
        let value = eval(line);
        if (this.pairs[this.pairs.length - 1].length < 2) {
          this.pairs[this.pairs.length - 1].push(value);
        } else {
          this.pairs.push([value]);
        }
      }
    });
    this.preview(this.pairs);
  }

  compare(left, right) {
    left = _.castArray(left);
    right = _.castArray(right);
    this.preview(`Compare ${left} - ${right}`);
    if (left.length === 1 && right.length === 1) {
      this.preview(`> Length 1: ${left[0]} - ${right[0]}`);
      if (Array.isArray(left[0]) || Array.isArray(right[0])) {
        return this.compare(left[0], right[0]);
      } else {
        if (left[0] < right[0]) return -1;
        if (left[0] > right[0]) return 1;
        return 0;
      }
    }
    for (let i = 0; i < Math.max(left.length, right.length); i++) {
      this.preview(`Iteration ${i}: ${left[i]} - ${right[i]}`);
      if (left[i] === undefined) return -1;
      if (right[i] === undefined) return 1;
      let comparison = this.compare(left[i], right[i]);
      this.preview(`> Result ${comparison}`);
      if (comparison !== 0) return comparison;
    }
    this.preview(`> Same`);
    return 0;
  }

  run() {
    let score = 0;
    this.pairs.forEach(([left, right], i) => {
      let comparison = this.compare(left, right);
      this.preview(comparison);
      if (comparison < 0) score += i + 1;
    });
    return score;
  }
}

module.exports = Solver;
