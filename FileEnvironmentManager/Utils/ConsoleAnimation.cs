namespace FileEnvironmentManager.Utils
{
    public static class ConsoleAnimation
    {
        public static async Task RunWithSpinner(Func<Task> action, string message = "Processando")
        {
            var cancelToken = new CancellationTokenSource();
            var spinner = Task.Run(() =>
            {
                var symbols = new[] { '|', '/', '-', '\\' };
                int counter = 0;
                while (!cancelToken.Token.IsCancellationRequested)
                {
                    Console.Write($"\r{message}... {symbols[counter++ % symbols.Length]} ");
                    Thread.Sleep(100);
                }
            });

            try
            {
                await action();
            }
            finally
            {
                cancelToken.Cancel();
                await spinner;
                Console.Write("\r" + new string(' ', Console.WindowWidth) + "\r");
            }
        }
    }
}
