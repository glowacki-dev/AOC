var _ = require("lodash");

class Solver {
  constructor(data, preview) {
    this.numbers = [];
    this.preview = preview;
    data.forEach((line) => {
      if (line !== "") {
        this.numbers.push({ value: Number(line), processed: false });
      }
    });
    this.preview(this.numbers);
  }

  run() {
    let i = 0;
    while (i < this.numbers.length) {
      let el = this.numbers[i];
      if (el.processed) {
        i += 1;
        continue;
      }
      el.processed = true;
      this.numbers.splice(i, 1);
      let newIndex = (i + el.value) % this.numbers.length;
      if (newIndex === 0) newIndex = this.numbers.length;
      this.preview([i, "Add", el.value, "at", newIndex].join(" "));
      this.numbers.splice(newIndex, 0, el);
      this.preview(this.numbers);
    }
    let start = _.findIndex(this.numbers, { value: 0 });
    this.preview(start);
    const indices = [1000, 2000, 3000];
    return _.sum(
      indices
        .map((i) => this.numbers[(i + start) % this.numbers.length])
        .map((n) => n.value)
    );
  }
}

module.exports = Solver;
