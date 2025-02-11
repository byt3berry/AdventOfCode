using Position = (int, int);
using Direction = (int, int);

class Program {
    static readonly Direction NORTH = (-1, 0);
    static readonly Direction EST = (0, 1);
    static readonly Direction SOUTH = (1, 0);
    static readonly Direction WEST = (0, -1);
    static readonly Direction ERROR = (0, 0);

    static void Main(string[] args) {
        if (args.Length != 1) { return; }

        part1(args[0]);
        part2(args[0]);
    }

    static Direction turn90(in Direction direction) {
        if (direction == NORTH) return EST;
        if (direction == EST) return SOUTH;
        if (direction == SOUTH) return WEST;
        if (direction == WEST) return NORTH;

        return ERROR;
    }

    static Position step(in Position position, in Direction direction) {
        return (position.Item1 + direction.Item1, position.Item2 + direction.Item2);
    }

    static bool inRange(in Position position, in int rows, in int columns) {
        return 0 <= position.Item1 && position.Item1 < rows && 0 <= position.Item2 && position.Item2 < columns;
    }

    static HashSet<Position> run(Position position, Direction direction, HashSet<Position> objects, int rows, int columns) {
        HashSet<Position> positions = new();
        Position nextPosition;
        while (inRange(position, rows, columns)) {
            nextPosition = step(position, direction);

            if (objects.Contains(nextPosition)) {
                direction = turn90(direction);
            } else {
                positions.Add(position);
                position = nextPosition;
            }
        }

        return positions;
    }

    static void part1(string filename) {
        HashSet<Position> objects = new();
        Position position = (-1, -1);
        Direction direction = NORTH;
        int rows = 0;
        int columns = 0;

        using(StreamReader reader = File.OpenText(filename)) {
            int j = 0;

            while (!reader.EndOfStream) {
                char c = (char) reader.Read();

                if (c == '#') {
                    objects.Add(new (rows, j));
                } else if (c == '^') {
                    position = (rows, j);
                } else if (c == '\n') {
                    rows++;
                    columns = j;
                    j = 0;
                    continue;
                }

                j++;
            }
        }

        HashSet<Position> positions = run(position, direction, objects, rows, columns);
        int output = positions.Count;
        Console.WriteLine("part1: " + output);
    }

    static bool isLooping(Position position, Direction direction, in HashSet<Position> objects, in int rows, in int columns) {
        HashSet<(Position, Direction)> positions = new();
        Position nextPosition;
        while (inRange(position, rows, columns)) {
            nextPosition = step(position, direction);

            if (objects.Contains(nextPosition)) {
                direction = turn90(direction);
            } else {
                if (positions.Contains((position, direction))) return true;

                positions.Add((position, direction));
                position = nextPosition;
            }
        }

        return false;
    }

    static void part2(string filename) {
        HashSet<Position> objects = new();
        Position position = (-1, -1);
        Direction direction = NORTH;
        int rows = 0;
        int columns = 0;

        using(StreamReader reader = File.OpenText(filename)) {
            int j = 0;

            while (!reader.EndOfStream) {
                char c = (char) reader.Read();

                if (c == '#') {
                    objects.Add((rows, j));
                } else if (c == '^') {
                    position = (rows, j);
                } else if (c == '\n') {
                    rows++;
                    columns = j;
                    j = 0;
                    continue;
                }

                j++;
            }
        }

        HashSet<Position> positions = run(position, direction, objects, rows, columns);
        int output = 0;

        foreach (Position p in positions) {
            if (isLooping(position, direction, objects.Append(p).ToHashSet(), rows, columns)) {
                output++;
            }
        }

        Console.WriteLine("part2: " + output);
    }
}
