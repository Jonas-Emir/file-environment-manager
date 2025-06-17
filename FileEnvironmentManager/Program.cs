
namespace GerenciadorDePastas
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Gerenciador de Pastas ---");

            while (true)
            {
                Console.WriteLine("\nEscolha uma opção:");
                Console.WriteLine("1 - Criar pasta simples");
                Console.WriteLine("2 - Criar pastas por intervalo e categoria");
                Console.WriteLine("0 - Sair");
                Console.Write("Opção: ");

                string opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        CriarPastaSimples();
                        break;
                    case "2":
                        CriarPastasPorIntervaloECategoria();
                        break;
                    case "0":
                        Console.WriteLine("\nSaindo do aplicativo. Pressione qualquer tecla para finalizar...");
                        Console.ReadKey();
                        return;
                    default:
                        Console.WriteLine("Opção inválida. Por favor, tente novamente.");
                        break;
                }

                Console.WriteLine("\nPressione qualquer tecla para continuar...");
                Console.ReadKey(); 
                Console.Clear(); 
            }
        }

        static void CriarPastaSimples()
        {
            Console.WriteLine("\n--- Criar Pasta Simples ---");
            Console.Write("Digite o nome da pasta que deseja criar: ");
            string nomePasta = Console.ReadLine();

            string caminhoPasta = Path.Combine(Directory.GetCurrentDirectory(), nomePasta);

            try
            {
                if (Directory.Exists(caminhoPasta))
                {
                    Console.WriteLine($"A pasta '{nomePasta}' já existe em: {caminhoPasta}");
                }
                else
                {
                    Directory.CreateDirectory(caminhoPasta);
                    Console.WriteLine($"Pasta '{nomePasta}' criada com sucesso em: {caminhoPasta}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro ao criar a pasta: {ex.Message}");
            }
        }

        static void CriarPastasPorIntervaloECategoria()
        {
            Console.WriteLine("\n--- Criar Pastas por Intervalo e Categoria ---");

            int inicioIntervalo;
            int fimIntervalo;
            string categoria;

            Console.Write("Digite o número inicial do intervalo: ");
            while (!int.TryParse(Console.ReadLine(), out inicioIntervalo))
            {
                Console.WriteLine("Entrada inválida. Por favor, digite um número inteiro para o início do intervalo: ");
            }

            Console.Write("Digite o número final do intervalo: ");
            while (!int.TryParse(Console.ReadLine(), out fimIntervalo))
            {
                Console.WriteLine("Entrada inválida. Por favor, digite um número inteiro para o fim do intervalo: ");
            }

            if (inicioIntervalo > fimIntervalo)
            {
                Console.WriteLine("Erro: O número inicial do intervalo não pode ser maior que o número final.");
                return;
            }

            Console.Write("Digite a categoria para as pastas (ex: Tecnologia): ");
            categoria = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(categoria))
            {
                Console.WriteLine("Erro: A categoria não pode ser vazia.");
                return;
            }

            categoria = categoria.Trim().Replace(" ", "_");

            Console.WriteLine($"\nCriando pastas de {inicioIntervalo} a {fimIntervalo} com a categoria '{categoria}'...");

            for (int i = inicioIntervalo; i <= fimIntervalo; i++)
            {
                string nomePastaComCategoria = $"{i}_{categoria}";
                string caminhoPasta = Path.Combine(Directory.GetCurrentDirectory(), nomePastaComCategoria);

                try
                {
                    if (Directory.Exists(caminhoPasta))
                    {
                        Console.WriteLine($"  - A pasta '{nomePastaComCategoria}' já existe.");
                    }
                    else
                    {
                        Directory.CreateDirectory(caminhoPasta);
                        Console.WriteLine($"  - Pasta '{nomePastaComCategoria}' criada com sucesso!");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  - Erro ao criar a pasta '{nomePastaComCategoria}': {ex.Message}");
                }
            }
            Console.WriteLine("\nProcesso de criação de pastas por intervalo finalizado.");
        }
    }
}