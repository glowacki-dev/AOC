var _ = require("lodash");
let createGraph = require("ngraph.graph");
let path = require("ngraph.path");

class Solver {
  constructor(data, preview) {
    this.packets = [[[2]], [[6]]];
    this.preview = preview;
    data.forEach((line) => {
      if (line !== "") {
        let value = eval(line);
        this.packets.push(value);
      }
    });
    this.preview(this.packets);
  }

  run() {
    let score = 1;
    function compare(left, right) {
      left = _.castArray(left);
      right = _.castArray(right);
      //this.preview(`Compare ${left} - ${right}`);
      if (left.length === 1 && right.length === 1) {
        //this.preview(`> Length 1: ${left[0]} - ${right[0]}`);
        if (Array.isArray(left[0]) || Array.isArray(right[0])) {
          return compare(left[0], right[0]);
        } else {
          if (left[0] < right[0]) return -1;
          if (left[0] > right[0]) return 1;
          return 0;
        }
      }
      for (let i = 0; i < Math.max(left.length, right.length); i++) {
        //this.preview(`Iteration ${i}: ${left[i]} - ${right[i]}`);
        if (left[i] === undefined) return -1;
        if (right[i] === undefined) return 1;
        let comparison = compare(left[i], right[i]);
        //this.preview(`> Result ${comparison}`);
        if (comparison !== 0) return comparison;
      }
      //this.preview(`> Same`);
      return 0;
    }

    this.packets.sort(compare);
    this.preview(this.packets);

    this.packets.forEach((packet, i) => {
      if (packet.toString() === [[2]].toString()) score *= i + 1;
      if (packet.toString() === [[6]].toString()) score *= i + 1;
    });
    return score;
  }
}

module.exports = Solver;
