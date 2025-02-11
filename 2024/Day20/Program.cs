using Position = (int, int);
using Direction = (int, int);

enum Type {
    WALL,
    PATH,
    START,
    END,
    OTHER,
}

class Program {
    static readonly Direction UP = (-1, 0);
    static readonly Direction DOWN = (1, 0);
    static readonly Direction LEFT = (0, -1);
    static readonly Direction RIGHT = (0, 1);
    static readonly Direction[] directions = {UP, DOWN, LEFT, RIGHT};

    static void Main(string[] args) {
        if (args.Length != 1) { return; }

        part1(args[0]);
        part2(args[0]);
    }

    static void draw(bool[,] grid, int dim) {
        for (int i=0; i <= dim; i++) {
            for (int j=0; j <= dim; j++) {
                if (grid[i, j]) {
                    Console.Write('#');
                } else {
                    Console.Write('.');
                }
            }
            Console.WriteLine();
        }
    }

    static Position move(in Position position, in Direction direction) {
        return (position.Item1 + direction.Item1, position.Item2 + direction.Item2);
    }

    static bool inRange(Position position, int rows, int columns) {
        return 0 <= position.Item1 && position.Item1 < rows && 0 <= position.Item2 && position.Item2 < columns;
    }

    static int[,] buildDistanceGrid(int rows, int columns, Position start) {
        int[,] distances = new int[rows, columns];

        for (int i=0; i < rows; i++) {
            for (int j=0; j < columns; j++) {
                distances[i, j] = int.MaxValue;
            }
        }

        var (x, y) = start;
        distances[x, y] = 0;

        return distances;
    }

    static int[,] search(List<List<Type>> grid, Position start) {
        Position current, next;
        int xCurrent, yCurrent, xNext, yNext;
        int rows = grid.Count;
        int columns = grid[0].Count;
        int[,] distances = buildDistanceGrid(rows, columns, start);
        Queue<Position> toVisit = new();
        toVisit.Enqueue(start);

        while (toVisit.Count != 0) {
            current = toVisit.Dequeue();
            (xCurrent, yCurrent) = current;

            foreach (Direction direction in directions) {
                next = move(current, direction);
                (xNext, yNext) = next;

                if (!inRange(next, rows, columns) || grid[xNext][yNext] == Type.WALL) continue;

                if (distances[xNext, yNext] == int.MaxValue) toVisit.Enqueue(next);

                if (distances[xNext, yNext] > 1 + distances[xCurrent, yCurrent]) {
                    distances[xNext, yNext] = 1 + distances[xCurrent, yCurrent];
                }
            }
        }

        return distances;
    }

    static Dictionary<int, int> cheat(List<List<Type>> grid, Position start, Position end, int reference) {
        int[,] distances;
        int time, difference;
        var (xEnd, yEnd) = end;
        Dictionary<int, int> times = new();
        Type old1, old2;

        for (int i=0; i < grid.Count; i++) {
            for (int j=0; j < grid[0].Count - 2; j++) {
                if (grid[i][j] == Type.WALL) continue;
                if ((grid[i][j+1], grid[i][j+2]) != (Type.WALL, Type.PATH)) continue;

                old1 = grid[i][j];
                old2 = grid[i][j+1];

                grid[i][j] = Type.PATH;
                grid[i][j+1] = Type.PATH;

                distances = search(grid, start);
                time = distances[xEnd, yEnd];
                difference = reference - time;

                if (!times.ContainsKey(difference)) times[difference] = new();
                times[difference]++;

                grid[i][j] = old1;
                grid[i][j+1] = old2;
            }
        }

        for (int i=0; i < grid.Count - 2; i++) {
            for (int j=0; j < grid[0].Count; j++) {
                if (grid[i][j] == Type.WALL) continue;
                if ((grid[i+1][j], grid[i+2][j]) != (Type.WALL, Type.PATH)) continue;

                old1 = grid[i][j];
                old2 = grid[i+1][j];

                grid[i][j] = Type.PATH;
                grid[i+1][j] = Type.PATH;

                distances = search(grid, start);
                time = distances[xEnd, yEnd];
                difference = reference - time;

                if (!times.ContainsKey(difference)) times[difference] = new();
                times[difference]++;

                grid[i][j] = old1;
                grid[i+1][j] = old2;
            }
        }

        return times;
    }

    static void part1(string filename) {
        int output;
        int rows, columns, reference;
        int[,] distances;
        int xEnd = -1;
        int yEnd = -1;
        Position start = (-1, -1);
        List<List<Type>> grid = new();
        HashSet<Position> visited = new();
        Dictionary<int, int> times;

        using(StreamReader reader = File.OpenText(filename)) {
            while (!reader.EndOfStream) {
                grid.Add(reader
                        .ReadLine()!
                        .ToCharArray()
                        .Select(
                            x => x switch {
                            '#' => Type.WALL,
                            '.' => Type.PATH,
                            'S' => Type.START,
                            'E' => Type.END,
                            _   => Type.OTHER,
                            }
                            )
                        .ToList()
                        );
            }
        }

        rows = grid.Count;
        columns = grid[0].Count;

        for (int i=0; i < rows; i++) {
            for (int j=0; j < columns; j++) {
                if (grid[i][j] == Type.START) start = (i, j);
                if (grid[i][j] == Type.END) (xEnd, yEnd) = (i, j);
            }
        }

        distances = search(grid, start);
        reference = distances[xEnd, yEnd];
        times = cheat(grid, start, (xEnd, yEnd), reference);
        output = times.Where(x => x.Key >= 100).Select(x => x.Value).Sum();

        Console.WriteLine("part1: " + output);
    }

    static void part2(string filename) {
        int output = 0;

        Console.WriteLine("part2: " + output);
    }
}
