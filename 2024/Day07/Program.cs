
class Program {
    static readonly List<char> CHARSET1 = new(){'*', '+'};
    static readonly List<char> CHARSET2 = new(){'*', '+', '|'};

    static void Main(string[] args) {
        if (args.Length != 1) { return; }

        part1(args[0]);
        part2(args[0]);
    }

    static long compute(in List<long> numbers, in string operations) {
        long output = numbers[0];

        for (int i=0; i < numbers.Count-1; i++) {
            output = compute(output, numbers[i+1], operations[i]);
        }

        return output;
    }

    static long compute(in long x1, in long x2, in char operation) {
        switch (operation) {
            case '+': return x1 + x2;
            case '*': return x1 * x2;
            case '|': return long.Parse(x1.ToString() + x2.ToString());
            default: throw new InvalidDataException();
        }
    }

    static List<string> generate(in string start, in List<char> charset, in int length) {
        if (start.Length == length) {
            return new List<string>(){start};
        }

        List<string> output = new();

        foreach (char s in charset) {
            output = output.Concat(generate(start + s, charset, length)).ToList();
        }

        return output;
    }

    static bool bruteforce(in long total, in List<long> numbers, List<char> charset) {
        List<string> possibilities = generate("", charset, numbers.Count-1);

        foreach (string possibilitie in possibilities) {
            long computation = compute(numbers, possibilitie);

            if (total == computation) {
                return true;
            }
        }

        return false;
    }

    static void part1(string filename) {
        long output = 0;

        using(StreamReader reader = File.OpenText(filename)) {
            long total;
            List<long> numbers;
            string? line = reader.ReadLine();

            while (line != null) {
                string[] split = line.Split(": ", 2);
                total = long.Parse(split[0]);
                numbers = split[1].Split(" ").Select(long.Parse).ToList();

                if (bruteforce(total, numbers, CHARSET1)) {
                    output += total;
                }

                line = reader.ReadLine();
            }
        }

        Console.WriteLine("part1: " + output);
    }

    static void part2(string filename) {
        long output = 0;

        using(StreamReader reader = File.OpenText(filename)) {
            long total;
            List<long> numbers;
            string? line = reader.ReadLine();

            while (line != null) {
                string[] split = line.Split(": ", 2);
                total = long.Parse(split[0]);
                numbers = split[1].Split(" ").Select(long.Parse).ToList();

                if (bruteforce(total, numbers, CHARSET2)) {
                    output += total;
                }

                line = reader.ReadLine();
            }
        }

        Console.WriteLine("part2: " + output);
    }
}
