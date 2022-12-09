var _ = require("lodash");
var helpers = require("mnemonist/set");

class Solver {
  constructor(data, preview) {
    this.data = [];
    this.positions = new Set();
    this.head = { x: 0, y: 0 };
    this.tail = { x: 0, y: 0 };
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
        this.head.y += 1;
        break;
      case "D":
        this.head.y -= 1;
        break;
      case "L":
        this.head.x -= 1;
        break;
      case "R":
        this.head.x += 1;
        break;
    }
  }

  moveTail(direction) {
    if (
      Math.abs(this.head.x - this.tail.x) <= 1 &&
      Math.abs(this.head.y - this.tail.y) <= 1
    )
      return;
    switch (direction) {
      case "U":
        if (this.head.x !== this.tail.x) this.tail.x = this.head.x;
        this.tail.y += 1;
        break;
      case "D":
        if (this.head.x !== this.tail.x) this.tail.x = this.head.x;
        this.tail.y -= 1;
        break;
      case "L":
        if (this.head.y !== this.tail.y) this.tail.y = this.head.y;
        this.tail.x -= 1;
        break;
      case "R":
        if (this.head.y !== this.tail.y) this.tail.y = this.head.y;
        this.tail.x += 1;
        break;
    }
  }

  run() {
    this.preview(this.data);
    let positions = new Set(["0,0"]);
    this.data.forEach((direction) => {
      this.moveHead(direction);
      this.moveTail(direction);
      this.preview([this.head, this.tail]);
      positions.add(`${this.tail.x},${this.tail.y}`);
    });
    this.preview(positions);
    return positions.size;
  }
}

module.exports = Solver;
