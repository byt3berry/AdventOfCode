using Position = (long, long);
using Velocity = (long, long);

class Program {
    static void Main(string[] args) {
        if (args.Length != 1) { return; }

        part1(args[0]);
        part2(args[0]);
    }

    static List<(Position, Velocity)> computeNStep(List<(Position, Velocity)> robots, int rows, int columns, int n) {
        List<(Position, Velocity)> output = new();

        foreach ((Position, Velocity) robot in robots) {
            var (x, y) = robot.Item1;
            var (dx, dy) = robot.Item2;

            x += dx * n;
            x = modPositive(x, columns);
            y += dy * n;
            y = modPositive(y, rows);

            output.Add(((x, y), (dx, dy)));
        }

        return output;
    }

    static long modPositive(long x, long mod) {
        return ((x % mod) + mod) % mod;
    }

    static void part1(string filename) {
        long output;
        int rows = 103;
        int columns = 101;
        long quadrant_tl, quadrant_tr, quadrant_bl, quadrant_br;
        List<(Position, Velocity)> robots = new();
        quadrant_tl = quadrant_tr = quadrant_bl = quadrant_br = 0;

        using(StreamReader reader = File.OpenText(filename)) {
            string[] split;
            long[] temp;
            Position position;
            Velocity velocity;

            while (!reader.EndOfStream) {
                split = reader.ReadLine()!
                    .Split(" ")
                    .Select(x => x[2..])
                    .ToArray();
                temp = split[0]
                    .Split(",")
                    .Select(long.Parse)
                    .ToArray();
                position = (temp[0], temp[1]);
                temp = split[1]
                    .Split(",")
                    .Select(long.Parse)
                    .ToArray();
                velocity = (temp[0], temp[1]);
                robots.Add((position, velocity));
            }
        }

        robots = computeNStep(robots, rows, columns, 100);

        foreach ((Position, Velocity) robot in robots) {
            var (x, y) = robot.Item1;

            if (x < columns / 2) {
                if (y < rows / 2) {
                    quadrant_tl++;
                } else if (y > rows / 2) {
                    quadrant_tr++;
                }
            } else if (x > columns / 2) {
                if (y < rows / 2) {
                    quadrant_bl++;
                } else if (y > rows / 2) {
                    quadrant_br++;
                }
            }
        }

        output = quadrant_tl * quadrant_tr * quadrant_bl * quadrant_br;

        Console.WriteLine("part1: " + output);
    }

    static void draw(bool[,] grid, int rows, int columns) {
        for (int i=0; i < rows; i++) {
            for (int j=0; j < columns; j++) {
                if (grid[i, j]) {
                    Console.Write(1);
                } else {
                    Console.Write(' ');
                }
            }

            Console.Write('\n');
        }
    }

    static bool[,] buildGrid(List<(Position, Velocity)> robots, int rows, int columns) {
        bool[,] grid = new bool[rows, columns];

        foreach ((Position, Velocity) robot in robots) {
            var (x, y) = robot.Item1;
            grid[y, x] = true;
        }

        return grid;
    }

    static bool inRange(int x, int y, int rows, int columns) {
        return 0 <= x && x < rows && 0 <= y && y < columns;
    }


    static int countNeighbours(bool[,] grid, int x, int y, int rows, int columns) {
        int output = 0;

        for (int i=-1; i <= 1; i++) {
            for (int j=-1; j <= 1; j++) {
                if (i == 0 && j == 0) continue;
                if (inRange(x+i, y+j, rows, columns) && grid[x+i, y+j]) output++;
            }
        }

        return output;
    }

    static float averageNeighbours(bool[,] grid, int rows, int columns) {
        float total = 0;
        float count = 0;

        for (int i=0; i < rows; i++) {
            for (int j=0; j < columns; j++) {
                if (grid[i, j]) {
                    total += countNeighbours(grid, i, j, rows, columns);
                    count++;
                }
            }
        }

        return total / count;
    }

    static void part2(string filename) {
        int output;
        int rows = 103;
        int columns = 101;
        bool[,] grid;
        float average;
        List<(Position, Velocity)> robots = new();
        List<(Position, Velocity)> step = new();

        using(StreamReader reader = File.OpenText(filename)) {
            string[] split;
            long[] temp;
            Position position;
            Velocity velocity;

            while (!reader.EndOfStream) {
                split = reader.ReadLine()!
                    .Split(" ")
                    .Select(x => x[2..])
                    .ToArray();
                temp = split[0]
                    .Split(",")
                    .Select(long.Parse)
                    .ToArray();
                position = (temp[0], temp[1]);
                temp = split[1]
                    .Split(",")
                    .Select(long.Parse)
                    .ToArray();
                velocity = (temp[0], temp[1]);
                robots.Add((position, velocity));
            }
        }

        output = 1;
        step = computeNStep(robots, rows, columns, 1);
        grid = buildGrid(robots, rows, columns);
        average = averageNeighbours(grid, rows, columns);

        while (average < 1) {
            step = computeNStep(step, rows, columns, 1);
            grid = buildGrid(step, rows, columns);
            average = averageNeighbours(grid, rows, columns);
            output++;
        }

        Console.WriteLine("part2: " + output);
    }
}
