class Program {
    static void Main(string[] args) {
        if (args.Length != 1) { return; }

        part1(args[0]);
        part2(args[0]);
    }

    static bool inRange(List<List<int>> heights, int x, int y) {
        int rows = heights.Count;
        int columns = heights[0].Count;
        return 0 <= x && x < rows && 0 <= y && y < columns;
    }

    static HashSet<Tuple<int, int>> search(List<List<int>> heights, Tuple<int, int> position) {
        int x = position.Item1;
        int y = position.Item2;
        int height = heights[x][y];

        if (height == 9) return new(){position};

        HashSet<Tuple<int, int>> output = new();

        for (int i=-1; i <= 1 ; i++) {
            for (int j=-1; j <= 1; j++) {
                if ((i == 0 || j == 0) && inRange(heights, x+i, y+j) && heights[x+i][y+j] == height+1) {
                    output.UnionWith(search(heights, new(x+i, y+j)));
                }
            }
        }

        return output;
    }

    static void part1(string filename) {
        int output = 0;
        int rows = 0;
        int columns = 0;
        List<List<int>> heights = new();
        List<Tuple<int, int>> start = new();

        using(StreamReader reader = File.OpenText(filename)) {
            List<int> row = new();
            int height;
            int i = 0;
            int j = 0;

            while (!reader.EndOfStream) {
                char c = (char) reader.Read();

                if (c == '\n') {
                    i++;
                    j = 0;
                    heights.Add(row);
                    row = new();
                    continue;
                } else if ('0' <= c && c <= '9') {
                    height = int.Parse(c.ToString());

                    if (height == 0) {
                        start.Add(new(i, j));
                    }

                    row.Add(height);
                } else {
                    row.Add(-1);
                }

                j++;
            }
        }

        rows = heights.Count;
        columns = heights[0].Count;

        foreach (Tuple<int, int> point in start) {
            output += search(heights, point).Count;
        }

        Console.WriteLine("part1: " + output);
    }

    static List<Tuple<int, int>> searchRanking(List<List<int>> heights, Tuple<int, int> position) {
        int x = position.Item1;
        int y = position.Item2;
        int height = heights[x][y];

        if (height == 9) return new(){position};

        List<Tuple<int, int>> output = new();

        for (int i=-1; i <= 1 ; i++) {
            for (int j=-1; j <= 1; j++) {
                if ((i == 0 || j == 0) && inRange(heights, x+i, y+j) && heights[x+i][y+j] == height+1) {
                    output.AddRange(searchRanking(heights, new(x+i, y+j)));
                }
            }
        }

        return output;
    }

    static void part2(string filename) {
        int output = 0;
        int rows = 0;
        int columns = 0;
        List<List<int>> heights = new();
        List<Tuple<int, int>> start = new();

        using(StreamReader reader = File.OpenText(filename)) {
            List<int> row = new();
            int height;
            int i = 0;
            int j = 0;

            while (!reader.EndOfStream) {
                char c = (char) reader.Read();

                if (c == '\n') {
                    i++;
                    j = 0;
                    heights.Add(row);
                    row = new();
                    continue;
                } else if ('0' <= c && c <= '9') {
                    height = int.Parse(c.ToString());

                    if (height == 0) {
                        start.Add(new(i, j));
                    }

                    row.Add(height);
                } else {
                    row.Add(-1);
                }

                j++;
            }
        }

        rows = heights.Count;
        columns = heights[0].Count;

        foreach (Tuple<int, int> point in start) {
            output += searchRanking(heights, point).Count;
        }

        Console.WriteLine("part2: " + output);
    }
}
