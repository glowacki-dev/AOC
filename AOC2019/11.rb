require "forwardable"

class State
  def initialize(data)
    @data = data
    @index = 0
    @modes = []
    @relative_base = 0
  end

  def valid?
    @index >= 0
  end

  def current
    opcode = "%05d" % @data[@index]
    op = opcode[3..4].to_i
    @modes[0] = opcode[2].to_i
    @modes[1] = opcode[1].to_i
    @modes[2] = opcode[0].to_i
    op
  end

  def move(offset)
    @index += offset
    self
  end

  def jump(addr)
    @index = addr
    self
  end

  def a; read(0); end
  def b; read(1); end
  def c; read(2); end

  def set(param, val); store(param.to_s.ord - 97, val); end

  def adjust_base(offset)
    @relative_base += offset
    self
  end

  private

  def read(param)
    case @modes[param]
    when 0
      @data[@data[@index + 1 + param]]
    when 1
      @data[@index + 1 + param]
    when 2
      @data[@data[@index + 1 + param] + @relative_base]
    else
      raise StandardError, "Unknown read mode"
    end || 0
  end
  
  def store(offset, val)
    case @modes[offset]
    when 0
      @data[@data[@index + 1 + offset]] = val
    when 1
      raise StandardError, "Invalid write mode"
    when 2
      @data[@data[@index + 1 +offset] + @relative_base] = val
    else
      raise StandardError, "Unknown write mode"
    end
    self
  end
end

class Machine
  extend Forwardable
  
  def initialize(data)
    @state = State.new(data.dup)
    @halted = false
  end

  def run(*inputs)
    @inputs = inputs
    @halted = false
    ret = process while state.valid? && !halted
    ret
  end

  private

  attr_reader :state, :inputs, :halted

  def_delegators :state, :a, :b, :c

  def process
    case state.current
    when 1
      state.set(:c, a + b).move(4)
    when 2
      state.set(:c, a * b).move(4)
    when 3
      raise StandardError, "Missing input" unless inputs.any?
      state.set(:a, inputs.shift).move(2)
    when 4
      @halted = true
      ret = a
      state.move(2)
      return ret
    when 5
      a != 0 ? state.jump(b) : state.move(3)
    when 6
      a == 0 ? state.jump(b) : state.move(3)
    when 7
      state.set(:c, a < b ? 1 : 0).move(4)
    when 8
      state.set(:c, a == b ? 1 : 0).move(4)
    when 9
      state.adjust_base(a).move(2)
    when 99
      state.jump(-1)
      return nil
    else
      raise StandardError, "ded"
    end
  end
end

data = [
3,8,1005,8,319,1106,0,11,0,0,0,104,1,104,0,3,8,1002,8,-1,10,101,1,10,10,4,10,108,1,8,10,4,10,1001,8,0,28,2,1008,7,10,2,4,17,10,3,8,102,-1,8,10,101,1,10,10,4,10,1008,8,0,10,4,10,1002,8,1,59,3,8,1002,8,-1,10,101,1,10,10,4,10,1008,8,0,10,4,10,1001,8,0,81,1006,0,24,3,8,1002,8,-1,10,101,1,10,10,4,10,108,0,8,10,4,10,102,1,8,105,2,6,13,10,1006,0,5,3,8,1002,8,-1,10,101,1,10,10,4,10,108,0,8,10,4,10,1002,8,1,134,2,1007,0,10,2,1102,20,10,2,1106,4,10,1,3,1,10,3,8,102,-1,8,10,101,1,10,10,4,10,108,1,8,10,4,10,1002,8,1,172,3,8,1002,8,-1,10,1001,10,1,10,4,10,108,1,8,10,4,10,101,0,8,194,1,103,7,10,1006,0,3,1,4,0,10,3,8,1002,8,-1,10,1001,10,1,10,4,10,1008,8,1,10,4,10,101,0,8,228,2,109,0,10,1,101,17,10,1006,0,79,3,8,1002,8,-1,10,1001,10,1,10,4,10,108,0,8,10,4,10,1002,8,1,260,2,1008,16,10,1,1105,20,10,1,3,17,10,3,8,1002,8,-1,10,1001,10,1,10,4,10,1008,8,1,10,4,10,1002,8,1,295,1,1002,16,10,101,1,9,9,1007,9,1081,10,1005,10,15,99,109,641,104,0,104,1,21101,387365733012,0,1,21102,1,336,0,1105,1,440,21102,937263735552,1,1,21101,0,347,0,1106,0,440,3,10,104,0,104,1,3,10,104,0,104,0,3,10,104,0,104,1,3,10,104,0,104,1,3,10,104,0,104,0,3,10,104,0,104,1,21102,3451034715,1,1,21101,0,394,0,1105,1,440,21102,3224595675,1,1,21101,0,405,0,1106,0,440,3,10,104,0,104,0,3,10,104,0,104,0,21101,0,838337454440,1,21102,428,1,0,1105,1,440,21101,0,825460798308,1,21101,439,0,0,1105,1,440,99,109,2,22101,0,-1,1,21102,1,40,2,21101,0,471,3,21101,461,0,0,1106,0,504,109,-2,2106,0,0,0,1,0,0,1,109,2,3,10,204,-1,1001,466,467,482,4,0,1001,466,1,466,108,4,466,10,1006,10,498,1102,1,0,466,109,-2,2105,1,0,0,109,4,2101,0,-1,503,1207,-3,0,10,1006,10,521,21101,0,0,-3,21202,-3,1,1,22102,1,-2,2,21101,1,0,3,21102,540,1,0,1105,1,545,109,-4,2105,1,0,109,5,1207,-3,1,10,1006,10,568,2207,-4,-2,10,1006,10,568,22102,1,-4,-4,1106,0,636,22102,1,-4,1,21201,-3,-1,2,21202,-2,2,3,21102,587,1,0,1105,1,545,21201,1,0,-4,21101,0,1,-1,2207,-4,-2,10,1006,10,606,21102,0,1,-1,22202,-2,-1,-2,2107,0,-3,10,1006,10,628,22102,1,-1,1,21102,1,628,0,105,1,503,21202,-2,-1,-2,22201,-4,-2,-4,109,-5,2106,0,0
]

x = 0
y = 0
dirs = [[1, 0], [0, 1], [-1, 0], [0, -1]]
dir = 0

canvas = { }

canvas[x] = { }
canvas[x][y] = 1

min_x = 0
min_y = 0
max_x = 0
max_y = 0

robot = Machine.new(data)

loop do
  canvas[x] ||= {}
  canvas[x][y] ||= 0
  current_color = canvas[x][y]
  new_color = robot.run(current_color)
  break unless new_color
  canvas[x][y] = new_color
  rotation = robot.run(current_color)
  rotation = -1 if rotation == 0
  dir = (dir + rotation) % dirs.count
  x += dirs[dir][0]
  y += dirs[dir][1]
  min_x = [min_x, x].min
  max_x = [max_x, x].max
  min_y = [min_y, y].min
  max_y = [max_y, y].max
end

(min_x..max_x).reverse_each do |x|
  row = (min_y..max_y).map do |y|
    canvas[x][y] == 1 ? "█" : " "
  end.join
  puts row
end
