class Solver {
  constructor(data) {
    this.data = data;
  }

  run() {
    var totals = [];
    var local = 0;
    this.data.forEach((line) => {
      if (line == "") {
        totals.push(local);
        local = 0;
      } else {
        local += Number(line);
      }
    });
    totals = totals.sort((a, b) => b - a);
    console.log(totals[0] + totals[1] + totals[2]);
  }
}

module.exports = Solver;
