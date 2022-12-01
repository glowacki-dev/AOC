var _ = require("lodash");

class Solver {
  constructor(data) {
    this.data = [];
    var local = [];
    data.forEach((line) => {
      if (line === "") {
        this.data.push(local);
        local = [];
      } else {
        local.push(Number(line));
      }
    });
  }

  run() {
    var totals = _.map(this.data, _.sum);
    return _.sum(_.sortBy(totals).slice(-3));
  }
}

module.exports = Solver;
