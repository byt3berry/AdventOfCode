class Program {
    static void Main(string[] args) {
        if (args.Length != 1) { return; }

        Program program = new();

        program.part1(args[0]);
    }

    private void part1(string filename) {
        Dictionary<string, HashSet<string>> connections = [];

        using(StreamReader reader = File.OpenText(filename)) {
            string? line = reader.ReadLine();

            while (line != null) {
                string[] split = line.Split("-", 2);
                string computer1 = split[0];
                string computer2 = split[1];

                if (connections.TryGetValue(computer1, out HashSet<string>? connected1))
                    connected1.Add(computer2);
                else
                    connections[computer1] = [computer2];

                if (connections.TryGetValue(computer2, out HashSet<string>? connected2))
                    connected2.Add(computer1);
                else
                    connections[computer2] = [computer1];

                line = reader.ReadLine();
            }
        }

        foreach (var kv in connections) {
            Console.WriteLine($"{kv.Key}: {String.Join(", ", kv.Value)}");
        }

        int result = connections.Where(kv => kv.Value.Count == 3)
                                .Count();
        Console.WriteLine(result);
    }
}
