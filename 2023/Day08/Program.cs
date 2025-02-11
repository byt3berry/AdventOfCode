class Program {
    static void Main(string[] args) {
        if (args.Length != 1) { return; }

        part1(args[0]);
        part2(args[0]);
    }

    static void part1(string filename) {
        int output = 0;
        List<int> instructions = new();
        Dictionary<string, string[]> map = new();

        using(StreamReader reader = File.OpenText(filename)) {
            string? line;
            instructions = reader
                .ReadLine()!
                .ToCharArray()
                .Select(instruction =>
                        instruction switch {
                            'L' => 0,
                            'R' => 1,
                            _   => -1,
                        })
                .ToList();

            // Blank line
            reader.ReadLine();
            line = reader.ReadLine();

            while (line != null) {
                string key = line[0..3];
                string destL = line[7..10];
                string destR = line[12..15];

                map.Add(key, new string[]{destL, destR});
                line = reader.ReadLine();
            }
        }

        string state = "AAA";

        while (state != "ZZZ") {
            int instruction = instructions[output % instructions.Count];
            state = map[state][instruction];

            output++;
        }

        Console.WriteLine("part1: " + output);
    }

    static long gcd(long a, long b) {
        long remainder = a % b;
        return remainder == 0 ? b : gcd(b, remainder);
    }

    static long lcm(long a, long b) {
        return a * b / gcd(a, b);
    }
    static void part2(string filename) {
        long output = 0;
        List<long> instructions = new();
        List<string> states = new();
        List<long> steps = new();
        Dictionary<string, string[]> map = new();

        using(StreamReader reader = File.OpenText(filename)) {
            string? line;
            string key, left, right;

            instructions = reader
                .ReadLine()!
                .ToCharArray()
                .Select(instruction =>
                        instruction switch {
                            'L' => 0L,
                            'R' => 1L,
                            _   => throw new InvalidDataException("Expected L or R"),
                        })
                .ToList();

            reader.ReadLine(); // Blank line
            line = reader.ReadLine();

            while (line != null) {
                key = line[0..3];
                left = line[7..10];
                right = line[12..15];

                map.Add(key, new string[]{left, right});

                if (key[2] == 'A') {
                    states.Add(key);
                }

                line = reader.ReadLine();
            }
        }

        long instruction;
        long count;
        long index;
        string state;

        for (int i=0; i < states.Count; i++) {
            state = states[i];
            count = 0;

            while (state[2] != 'Z') {
                index = count % (long) instructions.Count;
                instruction = instructions[(int) index];
                state = map[state][instruction];
                count++;
            } 

            steps.Add(count);
        }

        output = steps.Aggregate(1L, (acc, x) => lcm(acc, x));
        Console.WriteLine("part2: " + output);
    }
}
