class Program {
    static void Main(string[] args) {
        if (args.Length != 1) { return; }

        part1(args[0]);
        part2(args[0]);
    }

    static bool checkUpdate(in Dictionary<int, List<int>> afters, in List<int> update) {
        for (int i=0; i < update.Count; i++) {
            int before = update[i];

            for (int j=i+1; j < update.Count; j++) {
                int after = update[j];

                if (afters.ContainsKey(after) && afters[after].Contains(before)) {
                    return false;
                }
            }
        }

        return true;
    }

    static void part1(in string filename) {
        int output = 0;
        Dictionary<int, List<int>> afters = new();
        List<List<int>> updates = new();

        using(StreamReader reader = File.OpenText(filename)) {
            string? line = reader.ReadLine();

            while (line != null && line != "") {
                int[] numbers = line.Split("|").Select(int.Parse).ToArray();

                if (afters.ContainsKey(numbers[0])) {
                    afters[numbers[0]].Add(numbers[1]);
                } else {
                    afters.Add(numbers[0], new(){numbers[1]});
                }

                line = reader.ReadLine();
            }

            line = reader.ReadLine();

            while (line != null) {
                List<int> update = line.Split(",").Select(int.Parse).ToList();
                updates.Add(update);
                line = reader.ReadLine();
            }
        }

        foreach (List<int> update in updates) {
            if (checkUpdate(afters, update)) {
                int middle = update.Count / 2;
                output += update[middle];
            }
        }

        Console.WriteLine("part1: " + output);
    }

    static List<int> checkUpdateCorrected(in Dictionary<int, List<int>> afters, in List<int> update, out bool isCorrected) {
        List<int> corrected = new(update);
        isCorrected = false;
        // TODO: move the second one just before the first one

        for (int i=0; i < corrected.Count; i++) {
            for (int j=i+1; j < corrected.Count; j++) {
                int before = corrected[i];
                int after = corrected[j];

                if (afters.ContainsKey(after) && afters[after].Contains(before)) {
                    corrected.RemoveAt(j);
                    corrected.Insert(i, after);
                    isCorrected = true;
                }
            }
        }

        return corrected;
    }

    static void part2(in string filename) {
        int output = 0;
        Dictionary<int, List<int>> afters = new();
        List<List<int>> updates = new();

        using(StreamReader reader = File.OpenText(filename)) {
            string? line = reader.ReadLine();

            while (line != null && line != "") {
                int[] numbers = line.Split("|").Select(int.Parse).ToArray();

                if (afters.ContainsKey(numbers[0])) {
                    afters[numbers[0]].Add(numbers[1]);
                } else {
                    afters.Add(numbers[0], new(){numbers[1]});
                }

                line = reader.ReadLine();
            }

            line = reader.ReadLine();

            while (line != null) {
                List<int> update = line.Split(",").Select(int.Parse).ToList();
                updates.Add(update);
                line = reader.ReadLine();
            }
        }

        bool isCorrected;
        List<int> corrected;
        int middle;

        foreach (List<int> update in updates) {
            corrected = checkUpdateCorrected(afters, update, out isCorrected);

            if (isCorrected) {
                middle = corrected.Count / 2;
                output += corrected[middle];
            }
        }

        Console.WriteLine("part2: " + output);
    }
}
