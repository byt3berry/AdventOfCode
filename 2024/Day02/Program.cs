class Program {
    static void Main(string[] args) {
        if (args.Length != 1) { return; }

        part1(args[0]);
        part2(args[0]);
    }

    static bool isSafe(in List<int> report) {
        int direction;

        if (report[0] == report[1]) {
            return false;
        } else if (report[0] < report[1]) {
            direction = -1;
        } else {
            direction = 1;
        }

        for (int i=0; i < report.Count-1; i++) {
            int diff = direction * (report[i] - report[i+1]);

            if (diff < 1 || 3 < diff) {
                return false;
            }
        }

        return true;
    }

    static void part1(in string filename) {
        int output = 0;

        using(StreamReader reader = File.OpenText(filename)) {
            string? line = reader.ReadLine();

            while (line != null) {
                List<int> report = line.Split(" ").Select(int.Parse).ToList();

                if (isSafe(report)) {
                    output++;
                }

                line = reader.ReadLine();
            }
        }

        Console.WriteLine("part1: " + output);
    }

    static bool tryCorrect(in List<int> report, int index) {
        if (index < 0) { return false; }

        List<int> copy = new(report);
        copy.RemoveAt(index);
        return isSafe(copy);
    }

    static bool isSafeCorrected(in List<int> report) {
        int direction;

        if (report[0] == report[1]) {
            report.RemoveAt(0);
            return isSafe(report);
        } else if (report[0] < report[1]) {
            direction = -1;
        } else {
            direction = 1;
        }

        for (int i=0; i < report.Count-1; i++) {
            int diff = direction * (report[i] - report[i+1]);

            if (diff < 1 || 3 < diff) {
                bool result = tryCorrect(report, i) || tryCorrect(report, i+1);

                // Need to tryCorrect with 0 because it may change the direction
                if (i == 1) {
                    return result || tryCorrect(report, 0);
                }

                return result;
            }
        }

        return true;
    }

    static void part2(in string filename) {
        int output = 0;

        using(StreamReader reader = File.OpenText(filename)) {
            string? line = reader.ReadLine();

            while (line != null) {
                List<int> report = line.Split(" ").Select(int.Parse).ToList();

                if (isSafeCorrected(report)) {
                    output++;
                }

                line = reader.ReadLine();
            }
        }

        Console.WriteLine("part2: " + output);
    }
}
