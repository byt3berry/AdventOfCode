class Program {
    static void Main(string[] args) {
        if (args.Length != 1) { return; }

        part1(args[0]);
        part2(args[0]);
    }

    static int[] advInstruction(int[] registers, int combo) {
        double numerator = registers[0];
        double denominator;

        if (0 <= combo && combo <= 3) denominator = combo;
        else if (4 <= combo && combo <= 6) denominator = registers[combo - 4];
        else {
            Console.WriteLine("Should not happen");
            return registers;
        }

        denominator = Math.Pow(2, denominator);
        registers[0] = (int) (numerator / denominator);

        return registers;
    }

    static int[] bxlInstruction(int[] registers, int literal) {
        registers[1] ^= literal;
        return registers;
    }

    static int[] bstInstruction(int[] registers, int combo) {
        if (0 <= combo && combo <= 3) registers[1] = combo % 8;
        else if (4 <= combo && combo <= 6) registers[1] = registers[combo - 4] % 8;
        else {
            Console.WriteLine("Should not happen");
        }

        return registers;
    }

    static bool jnzInstruction(int[] registers, int literal, ref int pc) {
        if (registers[0] != 0) {
            pc = literal;
            return true;
        }

        return false;
    }

    static int[] bxcInstruction(int[] registers) {
        registers[1] ^= registers[2];
        return registers;
    }

    static int outInstruction(int[] registers, int combo) {
        if (0 <= combo && combo <= 3) return combo % 8;
        else if (4 <= combo && combo <= 6) return registers[combo - 4] % 8;
        else return -1;
    }

    static int[] bdvInstruction(int[] registers, int combo) {
        double numerator = registers[0];
        double denominator;

        if (0 <= combo && combo <= 3) denominator = combo;
        else if (4 <= combo && combo <= 6) denominator = registers[combo - 4];
        else {
            return registers;
        }

        denominator = Math.Pow(2, denominator);
        registers[1] = (int) (numerator / denominator);

        return registers;
    }

    static int[] cdvInstruction(int[] registers, int combo) {
        double numerator = registers[0];
        double denominator;

        if (0 <= combo && combo <= 3) denominator = combo;
        else if (4 <= combo && combo <= 6) denominator = registers[combo - 4];
        else {
            return registers;
        }

        denominator = Math.Pow(2, denominator);
        registers[2] = (int) (numerator / denominator);

        return registers;
    }

    static List<int> run(List<int> program, int[] registers) {
        bool res;
        int opcode, operand;
        int pc = 0;
        List<int> output = new();

        while (true) {
            opcode = program[pc];
            operand = program[pc+1];


            switch (opcode) {
                case 0:
                    advInstruction(registers, operand);
                    break;
                case 1:
                    bxlInstruction(registers, operand);
                    break;
                case 2:
                    bstInstruction(registers, operand);
                    break;
                case 3:
                    res = jnzInstruction(registers, operand, ref pc);
                    if (!res) {
                        return output;
                    }

                    pc -= 2;
                    break;
                case 4:
                    bxcInstruction(registers);
                    break;
                case 5:
                    output.Add(outInstruction(registers, operand));
                    break;
                case 6:
                    bdvInstruction(registers, operand);
                    break;
                case 7:
                    cdvInstruction(registers, operand);
                    break;
            }

            pc += 2;
        }
    }

    static void part1(string filename) {
        string output;
        int[] registers = new int[3];
        List<int> program;

        using(StreamReader reader = File.OpenText(filename)) {
            registers[0] = int.Parse(reader.ReadLine()![12..]);
            registers[1] = int.Parse(reader.ReadLine()![12..]);
            registers[2] = int.Parse(reader.ReadLine()![12..]);

            reader.ReadLine();

            program = reader
                .ReadLine()![9..]
                .Split(',')
                .Select(int.Parse)
                .ToList();
        }

        output = String.Join(',', run(program, registers));

        Console.WriteLine("part1: " + output);
    }

    static List<int> runChecked(List<int> program, int[] registers) {
        bool res;
        int opcode, operand, outputted;
        int pc = 0;
        List<int> output = new();

        while (true) {
            opcode = program[pc];
            operand = program[pc+1];


            switch (opcode) {
                case 0:
                    advInstruction(registers, operand);
                    break;
                case 1:
                    bxlInstruction(registers, operand);
                    break;
                case 2:
                    bstInstruction(registers, operand);
                    break;
                case 3:
                    res = jnzInstruction(registers, operand, ref pc);
                    if (!res) {
                        return output;
                    }

                    pc -= 2;
                    break;
                case 4:
                    bxcInstruction(registers);
                    break;
                case 5:
                    outputted = outInstruction(registers, operand);

                    if (output.Count >= program.Count || program[output.Count] != outputted) return output;
                    output.Add(outputted);

                    break;
                case 6:
                    bdvInstruction(registers, operand);
                    break;
                case 7:
                    cdvInstruction(registers, operand);
                    break;
            }

            pc += 2;
        }
    }

    static int findA(List<int> program, int[] registers) {
        int i = 0;
        int count = program.Count;
        List<int> output;

        while (true) {
            registers[0] = i;
            output = runChecked(program, registers);
            if (output.Count == count && program.SequenceEqual(output)) return i;

            i++;
        }
    }

    static void part2(string filename) {
        int output;
        int[] registers = new int[3];
        List<int> program;

        using(StreamReader reader = File.OpenText(filename)) {
            registers[0] = int.Parse(reader.ReadLine()![12..]);
            registers[1] = int.Parse(reader.ReadLine()![12..]);
            registers[2] = int.Parse(reader.ReadLine()![12..]);

            reader.ReadLine();

            program = reader
                .ReadLine()![9..]
                .Split(',')
                .Select(int.Parse)
                .ToList();
        }

        output = findA(program, registers);

        Console.WriteLine("part2: " + output);
    }
}
