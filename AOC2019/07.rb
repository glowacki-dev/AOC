require "forwardable"

class State
  def initialize(data)
    @data = data
    @index = 0
    @modes = []
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

  private

  def read(param)
    @modes[param] == 1 ? @data[@index + 1 + param] : @data[@data[@index + 1 + param]]
  end
  
  def store(offset, val)
    @data[@data[@index + 1 + offset]] = val
    self
  end
end

class Machine
  extend Forwardable
  
  def initialize(data)
    @state = State.new(data.dup)
    @halted = false
  end

  def run(inputs)
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
    when 99
      state.jump(-1)
      return nil
    else
      raise StandardError, "ded"
    end
  end
end

data = [
3,8,1001,8,10,8,105,1,0,0,21,38,63,76,89,106,187,268,349,430,99999,3,9,1001,9,5,9,102,3,9,9,1001,9,2,9,4,9,99,3,9,101,4,9,9,102,3,9,9,101,4,9,9,1002,9,3,9,101,2,9,9,4,9,99,3,9,101,5,9,9,1002,9,4,9,4,9,99,3,9,101,2,9,9,1002,9,5,9,4,9,99,3,9,1001,9,5,9,1002,9,5,9,1001,9,5,9,4,9,99,3,9,1001,9,2,9,4,9,3,9,102,2,9,9,4,9,3,9,1001,9,1,9,4,9,3,9,1002,9,2,9,4,9,3,9,101,1,9,9,4,9,3,9,1001,9,2,9,4,9,3,9,1001,9,1,9,4,9,3,9,101,1,9,9,4,9,3,9,102,2,9,9,4,9,3,9,1002,9,2,9,4,9,99,3,9,101,2,9,9,4,9,3,9,101,2,9,9,4,9,3,9,101,2,9,9,4,9,3,9,1001,9,2,9,4,9,3,9,102,2,9,9,4,9,3,9,1001,9,2,9,4,9,3,9,1001,9,1,9,4,9,3,9,1002,9,2,9,4,9,3,9,1001,9,1,9,4,9,3,9,101,2,9,9,4,9,99,3,9,1002,9,2,9,4,9,3,9,1001,9,2,9,4,9,3,9,101,2,9,9,4,9,3,9,1001,9,2,9,4,9,3,9,102,2,9,9,4,9,3,9,1001,9,1,9,4,9,3,9,1002,9,2,9,4,9,3,9,101,2,9,9,4,9,3,9,102,2,9,9,4,9,3,9,102,2,9,9,4,9,99,3,9,102,2,9,9,4,9,3,9,1001,9,2,9,4,9,3,9,101,1,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,1001,9,2,9,4,9,3,9,1001,9,2,9,4,9,3,9,102,2,9,9,4,9,3,9,101,1,9,9,4,9,3,9,101,1,9,9,4,9,3,9,101,2,9,9,4,9,99,3,9,1002,9,2,9,4,9,3,9,101,1,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,102,2,9,9,4,9,3,9,1001,9,1,9,4,9,3,9,101,1,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,101,1,9,9,4,9,3,9,1001,9,2,9,4,9,3,9,102,2,9,9,4,9,99
]

@max = 0

[5, 6, 7, 8, 9].permutation.each do |pa, pb, pc, pd, pe|
  ma = Machine.new(data)
  mb = Machine.new(data)
  mc = Machine.new(data)
  md = Machine.new(data)
  me = Machine.new(data)
  @e = 0
  a = ma.run([pa, @e])
  b = mb.run([pb, a])
  c = mc.run([pc, b])
  d = md.run([pd, c])
  tmp_e = me.run([pe, d])
  @e = tmp_e
  loop do
    a = ma.run([@e])
    break if a.nil?
    b = mb.run([a])
    break if b.nil?
    c = mc.run([b])
    break if c.nil?
    d = md.run([c])
    break if d.nil?
    tmp_e = me.run([d])
    break if tmp_e.nil?
    @e = tmp_e
  end
  @max = [@max, @e].max
end

puts @max
