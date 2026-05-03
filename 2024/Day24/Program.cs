class Program {
    static void Main(string[] args) {
        if (args.Length != 1) { return; }

        Program program = new();

        program.part1(args[0]);
    }

    private void part1(string filename) {
        Dictionary<string, bool> variables = [];
        List<(string Operation, string Operand1, string Operand2, string Result)> operations = [];

        using(StreamReader reader = File.OpenText(filename)) {
            string? line = reader.ReadLine();

            while (line != null) {
                if (String.IsNullOrWhiteSpace(line)) break;

                string[] split = line.Split(": ", 2);
                string variable = split[0];
                bool value = Int32.Parse(split[1]) switch {
                    0 => false,
                    1 => true,
                    _ => throw new NotImplementedException(),
                };

                variables[variable] = value;

                line = reader.ReadLine();
            }

            line = reader.ReadLine();

            while (line != null) {
                string[] split = line.Split(" ", 5);
                string operation = split[1];
                string operand1 = split[0];
                string operand2 = split[2];
                string result = split[4];

                operations.Add((operation, operand1, operand2, result));

                line = reader.ReadLine();
            }
        }

        List<(string Operation, string Operand1, string Operand2, string Result)> remaining = [];
        while (operations.Count > 0)
        {
            foreach ((string Operation, string Operand1, string Operand2, string Result) operation in operations) {
                if (!variables.ContainsKey(operation.Operand1) || !variables.ContainsKey(operation.Operand2))
                {
                    remaining.Add(operation);
                    continue;
                }

                variables[operation.Result] = operation.Operation switch {
                    "AND" => variables[operation.Operand1] && variables[operation.Operand2],
                    "OR" => variables[operation.Operand1] || variables[operation.Operand2],
                    "XOR" => variables[operation.Operand1] ^ variables[operation.Operand2],
                    _ => throw new NotImplementedException(),
                };
            }

            operations = [.. remaining];
            remaining.Clear();
        }

        long output = variables.Where(kv => kv.Key.StartsWith("z"))
                               .OrderByDescending(kv => kv.Key)
                               .Select(kv => kv.Value)
                               .Aggregate<bool, long>(0, (aggregate, next) => aggregate << 1 | (next ? 1L : 0L));

        Console.WriteLine(output);
    }
}
