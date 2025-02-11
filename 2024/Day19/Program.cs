class Program {
    static void Main(string[] args) {
        if (args.Length != 1) { return; }

        part1(args[0]);
        part2(args[0]);
    }

    static bool isPossible(List<string> patterns, string design, ref HashSet<string> notPossibles) {
        if (design.Length == 0) return true;
        if (notPossibles.Contains(design)) return false;

        foreach (string pattern in patterns) {
            if (design.StartsWith(pattern) && isPossible(patterns, design.Substring(pattern.Length), ref notPossibles)) {
                return true;
            }
        }

        notPossibles.Add(design);
        return false;
    }

    static void part1(string filename) {
        int output = 0;
        List<string> patterns;
        List<string> designs = new();
        HashSet<string> notPossibles = new();

        using(StreamReader reader = File.OpenText(filename)) {
            string? line;
            patterns = reader
                .ReadLine()!
                .Split(", ")
                .ToList();

            reader.ReadLine();
            line = reader.ReadLine();

            while (line != null) {
                designs.Add(line);
                line = reader.ReadLine();
            }
        }

        foreach (string design in designs) {
            if (isPossible(patterns, design, ref notPossibles)) output++;
        }

        Console.WriteLine("part1: " + output);
    }

    static bool contains(HashSet<List<string>> set, List<string> list) {
        return set.Where(x => x.SequenceEqual(list)).Any();
    }

    static void writeWays(HashSet<List<string>> waysSet, string design) {
        Console.WriteLine($"ways for : \"{design}\" ({waysSet.Count})");
        foreach (List<string> ways in waysSet) {
            Console.Write("[");
            foreach (string pattern in ways) {
                Console.Write(pattern + " ");
            }
            Console.Write("]");
        }
        Console.WriteLine();
    }

    static bool isPossibleCount(List<string> patterns, string design, ref Dictionary<string, HashSet<List<string>>> count, ref HashSet<string> notPossibles) {
        bool found = false;
        if (!count.ContainsKey(design)) count[design] = new();
        else return true;
        if (design.Length == 0) {
            count[design].Add(new (){});
            return true;
        }
        if (notPossibles.Contains(design)) return false;

        foreach (string pattern in patterns) {
            if (design.StartsWith(pattern)) {
                Console.WriteLine($"try {design} with {pattern}");
                string subdesign = design.Substring(pattern.Length);

                if (isPossibleCount(patterns, subdesign, ref count, ref notPossibles)) {
                    foreach (List<string> subway in count[subdesign]) {
                        List<string> way = new (){pattern};
                        way.AddRange(subway);

                        if (!contains(count[design], way)) count[design].Add(way);
                    }

                    found = true;
                }
            }
        }

        if (!found) notPossibles.Add(design);
        return found;
    }

    static void part2(string filename) {
        int output = 0;
        List<string> patterns;
        List<string> designs = new();
        HashSet<string> notPossibles = new();
        Dictionary<string, HashSet<List<string>>> count = new();

        using(StreamReader reader = File.OpenText(filename)) {
            string? line;
            patterns = reader
                .ReadLine()!
                .Split(", ")
                .ToList();

            reader.ReadLine();
            line = reader.ReadLine();

            while (line != null) {
                designs.Add(line);
                line = reader.ReadLine();
            }
        }

        /* isPossibleCount(patterns, designs[0], ref count, ref notPossibles); */

        /* foreach (var (design, ways) in count) { */
        /*     writeWays(ways, design); */
        /* } */

        for (int i=0; i < designs.Count; i++) {
        /* foreach (string design in designs) { */
            Console.WriteLine($"{i}/{designs.Count}");
            if (isPossibleCount(patterns, designs[i], ref count, ref notPossibles)) {
                output += count[designs[i]].Count;
                /* Console.WriteLine($"{design}: {count[design].Count}"); */
            }
        }

        Console.WriteLine("part2: " + output);
    }
}
