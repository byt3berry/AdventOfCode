public class Position : IEquatable<Position> {
    public int X { get; }
    public int Y { get; }

    public Position(int x, int y) {
        X = x;
        Y = y;
    }

    public bool Equals(Position? other) {
        if (other is null) return false;
        return X == other.X && Y == other.Y;
    }

    public override int GetHashCode() {
        return ToString().GetHashCode();
    }

    public override string ToString() {
        return $"({X}, {Y})";
    }
}

class Program {
    static void Main(string[] args) {
        if (args.Length != 1) { return; }

        part1(args[0]);
        part2(args[0]);
    }

    static bool inRange(in Position position, in int rows, in int columns) {
        return 0 <= position.X && position.X < rows && 0 <= position.Y && position.Y < columns;
    }

    static void findAntinodes(in List<Position> antennas, ref HashSet<Position> antinodes, in int rows, in int columns) {
        Position antenna1, antenna2;
        Position antinode1, antinode2;
        int deltaX, deltaY;

        for (int i=0; i < antennas.Count; i++) {
            for (int j=i+1; j < antennas.Count; j++) {
                antenna1 = antennas[i];
                antenna2 = antennas[j];
                deltaX = antenna2.X - antenna1.X;

                if (antenna1.Y < antenna2.Y) {
                    deltaY = antenna2.Y - antenna1.Y;
                    antinode1 = new Position(antenna1.X - deltaX, antenna1.Y - deltaY);
                    antinode2 = new Position(antenna2.X + deltaX, antenna2.Y + deltaY);

                    if (inRange(antinode1, rows, columns)) {
                        antinodes.Add(antinode1);
                    }

                    if (inRange(antinode2, rows, columns)) {
                        antinodes.Add(antinode2);
                    }
                } else {
                    deltaY = antenna1.Y - antenna2.Y;
                    antinode1 = new Position(antenna1.X - deltaX, antenna1.Y + deltaY);
                    antinode2 = new Position(antenna2.X + deltaX, antenna2.Y - deltaY);

                    if (inRange(antinode1, rows, columns)) {
                        antinodes.Add(antinode1);
                    }

                    if (inRange(antinode2, rows, columns)) {
                        antinodes.Add(antinode2);
                    }
                }
            }
        }
    }

    static void part1(string filename) {
        Dictionary<char, List<Position>> antennas = new();
        HashSet<Position> antinodes = new();
        int rows = 0;
        int columns = 0;

        using(StreamReader reader = File.OpenText(filename)) {
            int i = 0;
            int j = 0;

            while (!reader.EndOfStream) {
                char c = (char) reader.Read();

                if (c == '\n') {
                    i++;
                    columns = j;
                    j = 0;
                    continue;
                } else if (c != '.') {
                    if (!antennas.ContainsKey(c)) {
                        antennas.Add(c, new());
                    }

                    antennas[c].Add(new Position(i, j));
                }

                j++;
            }

            rows = i;
        }

        foreach (char key in antennas.Keys) {
            findAntinodes(antennas[key], ref antinodes, rows, columns);
        }

        int output = antinodes.Count;
        Console.WriteLine("part1: " + output);
    }

    static void addMultipleOfAntinode(in Position antenna, in int deltaX, in int deltaY, ref HashSet<Position> antinodes, in int rows, in int columns) {
        antinodes.Add(antenna);
        Position antinode = new Position(antenna.X + deltaX, antenna.Y + deltaY);

        while (inRange(antinode, rows, columns)) {
            antinodes.Add(antinode);
            antinode = new Position(antinode.X + deltaX, antinode.Y + deltaY);
        }
    }

    static void findAntinodesRepeated(in List<Position> antennas, ref HashSet<Position> antinodes, in int rows, in int columns) {
        Position antenna1, antenna2;
        int deltaX, deltaY;

        for (int i=0; i < antennas.Count; i++) {
            for (int j=i+1; j < antennas.Count; j++) {
                antenna1 = antennas[i];
                antenna2 = antennas[j];
                deltaX = antenna2.X - antenna1.X;

                if (antenna1.Y < antenna2.Y) {
                    deltaY = antenna2.Y - antenna1.Y;
                    addMultipleOfAntinode(antenna1, -deltaX, -deltaY, ref antinodes, rows, columns);
                    addMultipleOfAntinode(antenna2, deltaX, deltaY, ref antinodes, rows, columns);
                } else {
                    deltaY = antenna1.Y - antenna2.Y;
                    addMultipleOfAntinode(antenna1, -deltaX, deltaY, ref antinodes, rows, columns);
                    addMultipleOfAntinode(antenna2, deltaX, -deltaY, ref antinodes, rows, columns);
                }
            }
        }
    }

    static void part2(string filename) {
        Dictionary<char, List<Position>> antennas = new();
        HashSet<Position> antinodes = new();
        int rows = 0;
        int columns = 0;

        using(StreamReader reader = File.OpenText(filename)) {
            int i = 0;
            int j = 0;

            while (!reader.EndOfStream) {
                char c = (char) reader.Read();

                if (c == '\n') {
                    i++;
                    columns = j;
                    j = 0;
                    continue;
                } else if (c != '.') {
                    if (!antennas.ContainsKey(c)) {
                        antennas.Add(c, new());
                    }

                    antennas[c].Add(new Position(i, j));
                }

                j++;
            }

            rows = i;
        }

        foreach (char key in antennas.Keys) {
            findAntinodesRepeated(antennas[key], ref antinodes, rows, columns);
        }

        int output = antinodes.Count;
        Console.WriteLine("part2: " + output);
    }
}
