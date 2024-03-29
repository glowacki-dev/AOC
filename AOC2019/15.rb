require "forwardable"

class State
  def initialize(data)
    @data = data
    @index = 0
    @modes = []
    @relative_base = 0
  end

  def dump
    State.new(@data.dup).tap do |s|
      s.instance_variable_set(:@index, @index.dup)
      s.instance_variable_set(:@modes, @modes.dup)
      s.instance_variable_set(:@relative_base, @relative_base.dup)
    end
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

  def dump
    Machine.new(nil).tap do |m|
      m.instance_variable_set(:@state, state.dump)
      m.instance_variable_set(:@halted, @halted.dup)
    end
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
3,1033,1008,1033,1,1032,1005,1032,31,1008,1033,2,1032,1005,1032,58,1008,1033,3,1032,1005,1032,81,1008,1033,4,1032,1005,1032,104,99,101,0,1034,1039,1002,1036,1,1041,1001,1035,-1,1040,1008,1038,0,1043,102,-1,1043,1032,1,1037,1032,1042,1105,1,124,1001,1034,0,1039,101,0,1036,1041,1001,1035,1,1040,1008,1038,0,1043,1,1037,1038,1042,1105,1,124,1001,1034,-1,1039,1008,1036,0,1041,102,1,1035,1040,1001,1038,0,1043,101,0,1037,1042,1106,0,124,1001,1034,1,1039,1008,1036,0,1041,101,0,1035,1040,1002,1038,1,1043,101,0,1037,1042,1006,1039,217,1006,1040,217,1008,1039,40,1032,1005,1032,217,1008,1040,40,1032,1005,1032,217,1008,1039,33,1032,1006,1032,165,1008,1040,35,1032,1006,1032,165,1101,0,2,1044,1105,1,224,2,1041,1043,1032,1006,1032,179,1101,0,1,1044,1106,0,224,1,1041,1043,1032,1006,1032,217,1,1042,1043,1032,1001,1032,-1,1032,1002,1032,39,1032,1,1032,1039,1032,101,-1,1032,1032,101,252,1032,211,1007,0,68,1044,1105,1,224,1101,0,0,1044,1106,0,224,1006,1044,247,101,0,1039,1034,102,1,1040,1035,1001,1041,0,1036,102,1,1043,1038,101,0,1042,1037,4,1044,1105,1,0,30,84,39,21,27,93,20,65,45,95,19,6,71,25,33,13,80,53,60,70,65,80,45,65,53,62,93,13,19,72,33,49,54,92,9,29,25,69,7,46,9,96,97,70,8,69,71,97,3,75,94,49,96,11,76,24,29,84,87,99,33,76,83,83,21,62,97,82,63,71,78,74,29,94,90,34,92,58,75,44,66,99,28,37,84,18,18,94,86,50,4,74,3,96,74,39,99,55,93,44,94,55,40,78,2,88,70,6,69,67,87,40,4,93,76,30,1,42,40,87,23,83,89,24,73,19,62,88,43,92,94,50,71,53,19,75,22,9,82,46,65,84,92,63,99,57,23,62,93,61,14,87,67,84,90,38,96,83,33,63,40,80,75,10,79,89,52,14,97,32,87,72,57,79,7,79,6,93,66,77,50,19,97,78,65,96,24,94,80,12,10,70,9,60,77,67,17,83,76,36,79,27,43,91,6,72,77,49,4,47,56,85,81,11,46,96,93,33,82,44,69,49,34,98,77,95,38,19,85,1,62,73,49,95,39,62,36,83,23,93,34,32,21,94,89,30,85,76,13,92,87,3,84,43,3,74,39,81,6,85,16,69,89,21,56,80,65,92,84,97,7,63,23,8,87,37,70,54,75,92,95,96,51,83,34,24,86,39,59,48,89,45,34,89,72,3,77,63,98,38,70,39,38,98,97,85,46,96,53,81,89,27,83,75,31,81,71,39,81,62,79,11,78,18,90,94,1,91,1,79,77,74,64,20,73,55,75,78,2,77,24,92,56,55,25,70,21,38,69,49,81,19,34,92,97,74,61,79,18,77,51,76,62,92,10,85,83,87,39,90,31,98,95,61,32,63,82,59,75,65,53,72,91,17,75,75,54,85,57,32,13,39,70,48,86,59,50,96,32,23,84,61,85,48,59,92,33,15,58,83,95,48,80,70,84,58,69,70,37,99,18,73,79,32,71,22,41,75,26,71,25,55,73,31,5,53,71,95,65,87,50,62,95,80,54,95,73,79,20,94,65,83,33,26,88,3,11,99,76,93,28,97,67,49,90,94,19,85,28,10,96,70,55,84,17,75,33,47,91,44,88,96,1,6,89,40,69,27,58,98,61,25,77,79,43,83,38,13,72,44,99,20,33,69,8,5,47,72,78,24,53,94,78,39,99,87,9,63,82,52,69,64,48,93,46,48,89,22,84,32,69,7,36,99,80,4,27,92,54,14,85,56,19,99,93,99,49,67,82,90,23,10,77,77,37,79,67,78,27,81,79,34,67,81,40,88,76,89,94,64,80,73,79,57,72,22,14,93,3,88,84,88,41,12,29,4,97,57,83,38,93,51,55,20,75,57,78,22,76,22,24,85,91,79,27,19,46,90,18,71,3,39,28,26,94,87,83,31,35,73,56,99,83,35,65,92,45,98,93,2,73,88,15,90,62,85,95,20,96,75,52,4,62,81,78,49,67,69,20,5,85,72,79,45,34,73,89,20,37,60,79,97,6,41,78,40,70,42,29,89,21,76,88,44,82,17,9,73,52,71,73,25,89,71,30,82,85,26,86,61,43,7,71,13,99,72,40,95,79,39,67,39,65,90,91,14,96,20,73,98,66,13,92,70,1,93,2,86,45,54,85,73,30,62,14,97,89,39,77,99,40,89,76,49,97,42,60,97,62,82,35,98,49,80,15,91,34,87,65,77,44,93,65,87,76,82,20,78,46,90,18,81,73,98,47,99,48,69,2,82,90,90,47,85,49,94,37,81,76,90,0,0,21,21,1,10,1,0,0,0,0,0,0
]

@x = 0
@y = 0
@dirs = [[0, 1], [0, -1], [-1, 0], [1, 0]]

class Tile
  attr_reader :value, :x, :y, :robot

  def initialize(robot, x, y, value)
    @robot = robot.dump
    @x = x
    @y = y
    @value = value
  end

  def to_s
    case value
    when -1
      "x"
    when 0
      "█"
    when 1
      "."
    when 2
      "o"
    end
  end
end

@map = { }

@map[@x] = { }
@map[@x][@y] = Tile.new(Machine.new(data), @x, @y, -1)
@path = [@map[@x][@y]]

@min_x = 0
@min_y = 0
@max_x = 0
@max_y = 0

def show_map
  (@min_y..@max_y).reverse_each do |y|
    row = (@min_x..@max_x).map do |x|
      (x == @x && y == @y) ? "D" : (@map[x][y] || " ").to_s
    end.join
    puts row
  end
end

def save(robot, x, y, status)
  @map[x] ||= {}
  tile = Tile.new(robot, x, y, status)
  @map[x][y] = tile
  @min_x = [@min_x, x].min
  @max_x = [@max_x, x].max
  @min_y = [@min_y, y].min
  @max_y = [@max_y, y].max
  return tile
end

loop do
  last_tile = @path.last
  break unless last_tile

  @x = last_tile.x
  @y = last_tile.y
  next_dir = @dirs.find { |x, y| @map.dig(@x + x, @y + y).nil? }

  if next_dir.nil?
    @path.pop
    next
  end

  robot = last_tile.robot.dump

  x = @x + next_dir[0]
  y = @y + next_dir[1]

  status = robot.run(@dirs.index(next_dir) + 1)
  tile = save(robot, x, y, status)
  case status
  when 1
    @path << tile
  when 2
    @path << tile
    puts "GOT THERE"
    break
  end
end

show_map

puts @path.length

# Count steps on each tile
# Find incosistencies on path and account for them
