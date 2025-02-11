using Position = (int, int);
using Direction = (int, int);

class Program {
    static readonly Direction UP = (-1, 0);
    static readonly Direction DOWN = (1, 0);
    static readonly Direction LEFT = (0, -1);
    static readonly Direction RIGHT = (0, 1);
    static readonly Direction[] directions = {UP, LEFT, DOWN, RIGHT};

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

    static bool inRange(Position position, int dim) {
        return 0 <= position.Item1 && position.Item1 <= dim && 0 <= position.Item2 && position.Item2 <= dim;
    }

    static int[,] buildDistanceGrid(int dim) {
        int[,] distances = new int[dim+1, dim+1];

        for (int i=0; i <= dim; i++) {
            for (int j=0; j <= dim; j++) {
                distances[i, j] = int.MaxValue;
            }
        }

        distances[0, 0] = 0;

        return distances;
    }

    static int[,] search(bool[,] grid, int dim) {
        Position current, next;
        int xCurrent, yCurrent, xNext, yNext;
        int[,] distances = buildDistanceGrid(dim);
        Queue<Position> toVisit = new();
        toVisit.Enqueue((0, 0));

        while (toVisit.Count != 0) {
            current = toVisit.Dequeue();
            (xCurrent, yCurrent) = current;

            foreach (Direction direction in directions) {
                next = move(current, direction);
                (xNext, yNext) = next;

                if (!inRange(next, dim) || grid[xNext, yNext]) continue;

                if (distances[xNext, yNext] == int.MaxValue) toVisit.Enqueue(next);

                if (distances[xNext, yNext] > 1 + distances[xCurrent, yCurrent]) {
                    distances[xNext, yNext] = 1 + distances[xCurrent, yCurrent];
                }
            }
        }

        return distances;
    }

    static void part1(string filename) {
        int x, y;
        int output;
        int dim = 70;
        int maxBytes = 1024;
        bool[,] grid = new bool[dim+1, dim+1];
        int[,] distances;
        List<Position> corrupted = new();

        using(StreamReader reader = File.OpenText(filename)) {
            int count = 0;
            string? line = reader.ReadLine();

            while (line != null && count < maxBytes) {
                int[] coords = line
                    .Split(',')
                    .Select(int.Parse)
                    .ToArray();
                y = coords[0];
                x = coords[1];
                grid[x, y] = true;
                line = reader.ReadLine();
                count++;
            }
        }

        distances = search(grid, dim);
        output = distances[dim, dim];

        Console.WriteLine("part1: " + output);
    }

    static void part2(string filename) {
        Position output = (-1, -1);
        int dim = 70;
        int maxBytes = 1024;
        /* int dim = 6; */
        /* int maxBytes = 12; */
        bool[,] grid = new bool[dim+1, dim+1];
        int[,] distances;
        List<Position> corrupted = new();

        using(StreamReader reader = File.OpenText(filename)) {
            int x, y;
            string? line = reader.ReadLine();

            while (line != null) {
                int[] coords = line
                    .Split(',')
                    .Select(int.Parse)
                    .ToArray();
                y = coords[0];
                x = coords[1];
                corrupted.Add((x, y));
                line = reader.ReadLine();
            }
        }

        foreach (var (x, y) in corrupted.Take(maxBytes)) {
            grid[x, y] = true;
        }

        foreach (var (x, y) in corrupted.Skip(maxBytes)) {
            grid[x, y] = true;
            distances = search(grid, dim);

            if (distances[dim, dim] == int.MaxValue) {
                output = (y, x);
                break;
            }
        }

        Console.WriteLine("part2: " + output);
    }
}
