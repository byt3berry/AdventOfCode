class Program {
    static void Main(string[] args) {
        if (args.Length != 1) { return; }

        Program program = new();

        program.part1(args[0]);
    }

    private long Mix(long secretNumber, long value) => value ^ secretNumber;
    private long Prune(long secretNumber) => secretNumber % 16777216;

    private long NextSecretNumber(long secretNumber) {
        secretNumber = Prune(Mix(secretNumber, secretNumber * 64));
        secretNumber = Prune(Mix(secretNumber, (long)Math.Floor(secretNumber / 32d)));
        secretNumber = Prune(Mix(secretNumber, secretNumber * 2048));

        return secretNumber;
    }

    private void part1(string filename) {
        List<long> list = [];

        using(StreamReader reader = File.OpenText(filename)) {
            string? line = reader.ReadLine();

            while (line != null) {
                long column = Int64.Parse(line);

                list.Add(column);
                line = reader.ReadLine();
            }
        }

        long sum = list.Select(x => {
                                  long output = x;
                                  for (int i=0; i < 2_000; i++) 
                                  {
                                      output = NextSecretNumber(output);
                                  }

                                  return output;
                              })
                              .Sum();

        Console.WriteLine(sum);
    }
}
