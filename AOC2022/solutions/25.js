var _ = require("lodash");

class Solver {
  constructor(data, preview) {
    this.values = [];
    this.preview = preview;
    data.forEach((line) => {
      if (line !== "") {
        this.values.push(
          line.split("").map((val) => {
            if (val === "-") return -1;
            if (val === "=") return -2;
            return Number(val);
          })
        );
      }
    });
    this.preview(this.values);
  }

  toDecimal(values) {
    let number = 0;
    _.reverse(values).forEach((val, idx) => {
      number += val * Math.pow(5, idx);
    });
    return number;
  }

  fromDecimal(number) {
    let values = [];
    while (number > 0) {
      number += 2;
      let remainder = (number % 5) - 2;
      number = Math.floor(number / 5);
      values.push(remainder);
    }
    return _.reverse(
      values.map((val) => {
        if (val === -1) return "-";
        if (val === -2) return "=";
        return val;
      })
    );
  }

  run() {
    let sum = 0;
    this.values.forEach((value) => {
      sum += this.toDecimal(value);
    });
    this.preview(sum);
    return this.fromDecimal(sum).join("");
  }
}

module.exports = Solver;
