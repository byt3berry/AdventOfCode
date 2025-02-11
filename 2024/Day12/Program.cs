
class Program {
    static readonly (int, int)[] TOP_LEFT_OFFSETS = {(0, -1), (-1, -1), (-1, 0)};
    static readonly (int, int)[] TOP_RIGHT_OFFSETS = {(0, 1), (-1, 1), (-1, 0)};
    static readonly (int, int)[] BOTTOM_LEFT_OFFSETS = {(0, -1), (1, -1), (1, 0)};
    static readonly (int, int)[] BOTTOM_RIGHT_OFFSETS = {(0, 1), (1, 1), (1, 0)};
    static readonly (int, int)[][] OFFSETS = {
        TOP_LEFT_OFFSETS, TOP_RIGHT_OFFSETS,
        BOTTOM_LEFT_OFFSETS, BOTTOM_RIGHT_OFFSETS
    };

    static void Main(string[] args) {
        if (args.Length != 1) { return; }

        part1(args[0]);
        part2(args[0]);
    }

    static bool inRange(List<List<char>> plots, int x, int y) {
        return 0 <= x && x < plots.Count && 0 <= y && y < plots[0].Count;
    }

    static HashSet<(int, int)> find(List<List<char>> plots, int x, int y, ref HashSet<(int, int)> seen) {
        HashSet<(int, int)> output = new(){(x, y)};
        char plot = plots[x][y];
        seen.Add((x, y));

        for (int i=-1; i <= 1; i++) {
            for (int j=-1; j <= 1; j++) {
                if (
                        (i == 0 || j == 0)
                        && inRange(plots, x+i, y+j)
                        && plots[x+i][y+j] == plot
                        && !seen.Contains((x+i, y+j))
                   ) {
                    output.UnionWith(find(plots, x+i, y+j, ref seen));
                }
            }
        }

        return output;
    }

    static int computePerimeter(List<List<char>> plots, HashSet<(int, int)> found) {
        int output = 0;

        foreach ((int, int) coords in found) {
            var (x, y) = coords;
            char plot = plots[x][y];

            for (int i=-1; i <= 1; i++) {
                for (int j=-1; j <= 1; j++) {
                    if (i != 0 && j != 0) continue;
                    if (!inRange(plots, x+i, y+j) || plots[x+i][y+j] != plot) {
                        output++;
                    }
                }
            }
        }

        return output;
    }

    static void part1(string filename) {
        List<List<char>> plots = new();

        using(StreamReader reader = File.OpenText(filename)) {
            string? line = reader.ReadLine();

            while (line != null) {
                List<char> plot = line
                    .ToCharArray()
                    .ToList();

                plots.Add(plot);
                line = reader.ReadLine();
            }
        }

        HashSet<(int, int)> seen = new();
        int output = 0;

        for (int i=0; i < plots.Count; i++) {
            for (int j=0; j < plots[i].Count; j++) {
                if (!seen.Contains((i, j))) {
                    HashSet<(int, int)> found = find(plots, i, j, ref seen);
                    output += found.Count * computePerimeter(plots, found);
                }
            }
        }

        Console.WriteLine("part1: " + output);
    }

    static (int, int) add((int, int) x1, (int, int) x2) {
        return (x1.Item1 + x2.Item1, x1.Item2 + x2.Item2);
    }

    static bool isVertex(List<List<char>> plots, int x, int y, (int, int)[] offsets) {
        int newX, newY;
        int count = 0;
        char plot = plots[x][y];
        var (xDiag, yDiag) = offsets[1];

        foreach ((int, int) offset in offsets) {
            (newX, newY) = add((x, y), offset);

            if (inRange(plots, newX, newY) && plots[newX][newY] == plot) {
                count++;
            }
        }

        return count == 0 || count == 2 && inRange(plots, xDiag, yDiag) && plots[xDiag][yDiag] != plot;
    }

    static int countVertices(List<List<char>> plots, int x, int y) {
        int output = 0;
        char plot = plots[x][y];

        if (plot != 'O') return 0;
        
        foreach ((int, int)[] offsets in OFFSETS) {
            if (isVertex(plots, x, y, offsets)) {
                output++;
            }
        }

        return output;
    }

    static int countVertices(List<List<char>> plots, HashSet<(int, int)> found) {
        return found
            .Select(plot => countVertices(plots, plot.Item1, plot.Item2))
            .Sum();
    }

    static void part2(string filename) {
        List<List<char>> plots = new();

        using(StreamReader reader = File.OpenText(filename)) {
            string? line = reader.ReadLine();

            while (line != null) {
                List<char> plot = line
                    .ToCharArray()
                    .ToList();

                plots.Add(plot);
                line = reader.ReadLine();
            }
        }

        HashSet<(int, int)> seen = new();
        int output = 0;

        for (int i=0; i < plots.Count; i++) {
            for (int j=0; j < plots[i].Count; j++) {
                if (!seen.Contains((i, j))) {
                    HashSet<(int, int)> found = find(plots, i, j, ref seen);
                    int area = found.Count;
                    int vertices = found
                        .Select(plot => countVertices(plots, plot.Item1, plot.Item2))
                        .Sum();
                    /* Console.WriteLine($"area of {plots[i][j]} is {area}"); */
                    Console.WriteLine($"vertices of {plots[i][j]} is {vertices}");
                    output += area * vertices;
                }
            }
        }

        Console.WriteLine("part2: " + output);
    }
}
