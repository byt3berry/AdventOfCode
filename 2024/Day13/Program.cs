class Program {
    static void Main(string[] args) {
        if (args.Length != 1) { return; }

        part1(args[0]);
        part2(args[0]);
    }

    static int solve((int, int) buttonA, (int, int) buttonB, (int, int) prize) {
        List<int> found = new();
        var (xA, yA) = buttonA;
        var (xB, yB) = buttonB;
        var (xPrize, yPrize) = prize;

        int maxA = int.Min(xPrize / xA, yPrize / yA);
        int maxB = int.Min(xPrize / xB, yPrize / yB);
        maxA = int.Min(maxA, 100);
        maxB = int.Min(maxB, 100);
        /* Console.WriteLine($"maxA: {maxA}"); */
        /* Console.WriteLine($"maxB: {maxB}"); */

        for (int i=maxB; i >= 0; i--) {
            for (int j=0; j < maxA; j++) {
                int xTotal = i * xB + j * xA;
                int yTotal = i * yB + j * yA;
                /* Console.WriteLine(i + 3 * j); */

                if ((xTotal, yTotal) == prize) {
                    /* Console.WriteLine($"{j} {i}"); */
                    /* Console.WriteLine($"found: {i + 3 * j}"); */
                    found.Add(i + 3 * j);
                }
                /* if (xTotal > xPrize || yTotal > yPrize) break; */
            }
        }

        return found.Count == 0 ? 0 : found.Min();
    }

    static void part1(string filename) {
        int tokens = 0;
        int output = 0;

        using(StreamReader reader = File.OpenText(filename)) {
            string? line;
            (int, int) buttonA, buttonB, prize;
            List<int> split;

            while (!reader.EndOfStream) {
                split = reader.ReadLine()![10..]
                    .Split(", ")
                    .Select(x => x[2..])
                    .Select(int.Parse)
                    .ToList();
                buttonA = (split[0], split[1]);

                split = reader.ReadLine()![10..]
                    .Split(", ")
                    .Select(x => x[2..])
                    .Select(int.Parse)
                    .ToList();
                buttonB = (split[0], split[1]);

                split = reader.ReadLine()![7..]
                    .Split(", ")
                    .Select(x => x[2..])
                    .Select(int.Parse)
                    .ToList();
                prize = (split[0], split[1]);

                /* Console.WriteLine($"buttonA: {buttonA}"); */
                /* Console.WriteLine($"buttonB: {buttonB}"); */
                /* Console.WriteLine($"prize: {prize}"); */
                tokens = solve(buttonA, buttonB, prize);
                output += tokens;
                /* Console.WriteLine("solved: " + tokens); */
                /* Console.WriteLine(""); */

                line = reader.ReadLine();
                /* break; */
            }
        }

        Console.WriteLine("part1: " + output);
    }

    static void part2(string filename) {
        int output = 0;

        Console.WriteLine("part2: " + output);
    }
}
