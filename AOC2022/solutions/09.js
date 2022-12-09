var _ = require("lodash");
var helpers = require("mnemonist/set");

class Solver {
  constructor(data, preview) {
    this.data = [];
    this.positions = new Set();
    this.segments = [];
    for (let i = 0; i < 10; i++) this.segments.push({ x: 0, y: 0 });
    this.preview = preview;
    data.forEach((line) => {
      if (line !== "") {
        let [direction, count] = line.split(" ");
        for (let i = 0; i < count; i++) this.data.push(direction);
      }
    });
  }

  moveHead(direction) {
    switch (direction) {
      case "U":
        this.segments[0].y += 1;
        break;
      case "D":
        this.segments[0].y -= 1;
        break;
      case "L":
        this.segments[0].x -= 1;
        break;
      case "R":
        this.segments[0].x += 1;
        break;
    }
  }

  // segments[0] is head, so we skip it
  moveSegments(index = 1) {
    if (index >= this.segments.length) return;
    let parent = this.segments[index - 1];
    let current = this.segments[index];
    if (
      Math.abs(parent.x - current.x) <= 1 &&
      Math.abs(parent.y - current.y) <= 1
    )
      return;

    // check if diagonal move is required
    if (
      Math.abs(parent.x - current.x) > 1 ||
      Math.abs(parent.y - current.y) > 1
    ) {
      if (parent.x > current.x) current.x += 1;
      if (parent.x < current.x) current.x -= 1;
      if (parent.y > current.y) current.y += 1;
      if (parent.y < current.y) current.y -= 1;
    } else {
      if (parent.x > current.x + 1) current.x += 1;
      else if (parent.x < current.x - 1) current.x -= 1;
      else if (parent.y > current.y + 1) current.y += 1;
      else if (parent.y < current.y - 1) current.y -= 1;
    }
    this.moveSegments(index + 1);
  }

  run() {
    this.preview(this.data);
    let positions = new Set(["0,0"]);
    let tail = this.segments[this.segments.length - 1];
    this.data.forEach((direction) => {
      this.moveHead(direction);
      this.moveSegments();
      this.preview(this.segments);
      positions.add(`${tail.x},${tail.y}`);
    });
    this.preview(positions);
    return positions.size;
  }
}

module.exports = Solver;
