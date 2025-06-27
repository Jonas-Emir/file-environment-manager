namespace FileEnvironmentManager.Utils
{
    internal class ConsoleHelper
    {
        public static string Ask(string message)
        {
            Console.Write(message + " ");
            return Console.ReadLine();
        }

        public static string AskPath(string prompt)
        {
            Console.WriteLine(prompt);
            Console.WriteLine("Deixe em branco para usar o diretório atual.");
            var input = Console.ReadLine();
            return string.IsNullOrWhiteSpace(input) ? Directory.GetCurrentDirectory() : input;
        }

        public static void AskInterval(out int start, out int end)
        {
            Console.Write("Digite o número inicial: ");
            int.TryParse(Console.ReadLine(), out start);
            Console.Write("Digite o número final: ");
            int.TryParse(Console.ReadLine(), out end);
        }

        public static void WriteSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void WriteWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void WriteError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void WriteInfo(string message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
