var _ = require("lodash");
var Queue = require("mnemonist/queue");

class Solver {
  constructor(data) {
    this.data = [];
    data.forEach((line) => {
      if (line !== "") this.data.push(line);
    });
  }

  run() {
    this.data.forEach((stream) => {
      let i = 0;
      let buffer = new Queue();
      for (; i < stream.length; i++) {
        buffer.enqueue(stream[i]);
        if (buffer.size < 4) continue;
        if (buffer.size > 4) buffer.dequeue();
        if (_.uniq(buffer.toArray()).length == buffer.size) break;
      }
      console.log(i + 1);
    });
    return 0;
  }
}

module.exports = Solver;
