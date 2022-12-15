var _ = require("lodash");
let createGraph = require("ngraph.graph");
let path = require("ngraph.path");

class Range {
  constructor() {
    this.segments = [];
  }

  reduce() {
    if (this.segments.length <= 1) return false;
    for (let i = 0; i < this.segments.length; i++) {
      for (let j = i + 1; j < this.segments.length; j++) {
        let first = this.segments[i];
        let second = this.segments[j];
        // [second + first]
        if (first[0] <= second[1] && first[0] >= second[0]) {
          this.segments.push([second[0], Math.max(first[1], second[1])]);
          this.segments.splice(j, 1);
          this.segments.splice(i, 1);
          return true;
        }
        // [first + second]
        if (second[0] <= first[1] && second[0] >= first[0]) {
          this.segments.push([first[0], Math.max(first[1], second[1])]);
          this.segments.splice(j, 1);
          this.segments.splice(i, 1);
          return true;
        }
        // [first] + [second] || [second] + [first] (nothing left between)
        if (first[1] === second[0] - 1 || second[1] === first[0] - 1) {
          this.segments.push([
            Math.min(first[0], second[0]),
            Math.max(first[1], second[1]),
          ]);
          this.segments.splice(j, 1);
          this.segments.splice(i, 1);
        }
      }
    }
  }

  add(min, max) {
    this.segments.push([min, max]);
    while (this.reduce()) {}
    return this.segments;
  }
}

class Solver {
  constructor(data, preview) {
    this.sensors = [];
    this.beacons = [];
    this.preview = preview;
    let dataRegex = /.*x=([-\d]+).*y=([-\d]+).*x=([-\d]+).*y=([-\d]+).*/;
    data.forEach((line) => {
      if (line !== "") {
        let match = dataRegex.exec(line);
        this.sensors.push({
          x: Number(match[1]),
          y: Number(match[2]),
          r: Math.abs(match[1] - match[3]) + Math.abs(match[2] - match[4]),
        });
        this.beacons.push({ x: Number(match[3]), y: Number(match[4]) });
      }
    });
    this.preview(this.sensors);
    this.preview(this.beacons);
  }

  run() {
    let max = 4000000;
    for (let y = 0; y <= max; y++) {
      let excluded = new Range();
      this.sensors.forEach((sensor) => {
        // Sensor too far away
        if (Math.abs(sensor.y - y) > sensor.r) return;
        excluded.add(
          Math.max(sensor.x - sensor.r + Math.abs(sensor.y - y), 0),
          Math.min(sensor.x + sensor.r - Math.abs(sensor.y - y), max)
        );
      });
      if (excluded.segments.length > 1) {
        let x = _.min(excluded.segments.map((segment) => segment[1])) + 1;
        return BigInt(x) * 4000000n + BigInt(y);
      }
    }
    return 0;
  }
}

module.exports = Solver;
