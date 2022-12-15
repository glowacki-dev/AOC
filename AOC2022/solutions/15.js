var _ = require("lodash");
let createGraph = require("ngraph.graph");
let path = require("ngraph.path");

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
    let y = 2000000;
    let excluded = new Set();
    this.sensors.forEach((sensor) => {
      // Sensor too far away
      if (Math.abs(sensor.y - y) > sensor.r) return;
      // Gather points to the right of sensor
      for (
        let x = sensor.x;
        x <= sensor.x + sensor.r - Math.abs(sensor.y - y);
        x++
      ) {
        excluded.add(x);
      }
      // Gather points to the left
      for (
        let x = sensor.x;
        x >= sensor.x - sensor.r + Math.abs(sensor.y - y);
        x--
      ) {
        excluded.add(x);
      }
    });
    this.sensors.forEach((sensor) => {
      if (sensor.y === y) excluded.delete(sensor.x);
    });
    this.beacons.forEach((beacon) => {
      if (beacon.y === y) excluded.delete(beacon.x);
    });
    this.preview(Array.from(excluded).sort((a, b) => a - b));
    return excluded.size;
  }
}

module.exports = Solver;
