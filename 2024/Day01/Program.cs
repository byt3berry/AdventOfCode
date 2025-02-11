class Program {
    static void Main(string[] args) {
        if (args.Length != 1) { return; }

        part1(args[0]);
        part2(args[0]);
    }

    static void part1(string filename) {
        List<int> list1 = new();
        List<int> list2 = new();

        using(StreamReader reader = File.OpenText(filename)) {
            string? line = reader.ReadLine();

            while (line != null) {
                string[] split = line.Split("   ", 2);
                int column1 = Int32.Parse(split[0]);
                int column2 = Int32.Parse(split[1]);

                list1.Add(column1);
                list2.Add(column2);
                line = reader.ReadLine();
            }
        }

        list1.Sort();
        list2.Sort();

        int output = 0;

        for (int i=0; i < list1.Count; i++) {
            output += Int32.Abs(list1[i] - list2[i]);
        }

        Console.WriteLine("part1: " + output);
    }

    static void part2(string filename) {
        List<int> list = new();
        Dictionary<int, int> counter = new();

        using(StreamReader reader = File.OpenText(filename)) {
            string? line = reader.ReadLine();

            while (line != null) {
                string[] split = line.Split("   ", 2);
                int column1 = Int32.Parse(split[0]);
                int column2 = Int32.Parse(split[1]);

                list.Add(column1);

                if (counter.ContainsKey(column2)) {
                    counter[column2] += 1;
                } else {
                    counter[column2] = 1;
                }

                line = reader.ReadLine();
            }
        }

        int output = 0;

        foreach (int x in list) {
            if (counter.ContainsKey(x)) {
                output += x * counter[x];
            }
        }

        Console.WriteLine("part2: " + output);
    }
}
