class Program {
    static void Main(string[] args) {
        if (args.Length != 1) { return; }

        /* part1(args[0]); */
        part2(args[0]);
    }

    static long blink(long stone, long n, ref Dictionary<(long, long), long> stored) {
        if (n == 0) return 1;
        if (stored.ContainsKey((stone, n))) return stored[(stone, n)];

        long result = 0;

        if (stone == 0) {
            result = blink(1, n-1, ref stored);
        } else if (stone.ToString().Length % 2 == 0) {
            string stoneString = stone.ToString();
            int half = stoneString.Length / 2;
            long left = long.Parse(stoneString[..half]);
            long right = long.Parse(stoneString[half..]);

            long leftResult = blink(left, n-1, ref stored);
            stored.TryAdd((left, n-1), leftResult);
            long rightResult = blink(right, n-1, ref stored);
            stored.TryAdd((right, n-1), rightResult);

            result = leftResult + rightResult;
        } else {
            result = blink(stone * 2024, n-1, ref stored);
        }

        stored.TryAdd((stone, n), result);
        return result;
    }

    static void part1(string filename) {
        List<long> stones;

        using(StreamReader reader = File.OpenText(filename)) {
            stones = reader
                .ReadLine()!
                .Split(" ")
                .Select(long.Parse)
                .ToList();
        }

        long output = 0;
        Dictionary<(long, long), long> stored = new();

        foreach (int stone in stones) {
            output += blink(stone, 25, ref stored);
        }

        Console.WriteLine("part1: " + output);
    }

    static void part2(string filename) {
        List<long> stones;

        using(StreamReader reader = File.OpenText(filename)) {
            stones = reader
                .ReadLine()!
                .Split(" ")
                .Select(long.Parse)
                .ToList();
        }

        long output = 0;
        Dictionary<(long, long), long> stored = new();

        foreach (int stone in stones) {
            output += blink(stone, 75, ref stored);
        }

        Console.WriteLine("part2: " + output);
    }
}
