using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace GerenciadorDePastas
{
    class Program
    {
        private static readonly Dictionary<string, string> CategoriasConhecidas = new Dictionary<string, string>
        {
            { "construcao", "construcao" },
            { "tecnologia", "tecnologia" },
            { "saude", "saude" },
            { "beleza", "beleza" },
            { "design", "design" },
            { "vendas", "vendas" },
            { "financas", "financas" },
            { "juridico", "juridico" },
            { "educacao", "educacao" },
            { "engenharia", "engenharia" },
            { "marketing", "marketing" }
        };

        static async Task Main(string[] args)
        {
            Console.WriteLine("--- Gerenciador de Pastas e Arquivos ---");

            while (true)
            {
                Console.WriteLine("\nEscolha uma opção:");
                Console.WriteLine("1 - Criar pasta simples");
                Console.WriteLine("2 - Criar pastas por intervalo e categoria única");
                Console.WriteLine("3 - Atribuir thumbnails a pastas existentes");
                Console.WriteLine("4 - Remover todos os arquivos das pastas");
                Console.WriteLine("0 - Sair");
                Console.Write("Opção: ");

                string opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        CriarPastaSimples();
                        break;
                    case "2":
                        CriarPastasPorIntervaloECategoriaUnica();
                        break;
                    case "3":
                        await AtribuirArquivosAPastasDeProjetoExistentes();
                        break;
                    case "4":
                        RemoverFotosDasPastasDeProjeto();
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

        static string ObterCaminhoDoUsuario(string mensagem)
        {
            while (true)
            {
                Console.WriteLine($"\n{mensagem}");
                Console.WriteLine("Ou deixe em branco para usar o diretório atual do aplicativo.");
                Console.Write("Caminho: ");
                string inputCaminho = Console.ReadLine().Trim();

                if (string.IsNullOrWhiteSpace(inputCaminho))
                {
                    string diretorioAtual = Directory.GetCurrentDirectory();
                    Console.WriteLine($"Usando o diretório atual: {diretorioAtual}");
                    return diretorioAtual;
                }

                try
                {
                    Path.GetFullPath(inputCaminho);
                    Console.WriteLine($"Caminho selecionado: {inputCaminho}");
                    return inputCaminho;
                }
                catch (Exception ex) when (ex is ArgumentException || ex is PathTooLongException || ex is NotSupportedException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Caminho inválido: {ex.Message}. Por favor, tente novamente.");
                    Console.ResetColor();
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Ocorreu um erro inesperado ao validar o caminho: {ex.Message}");
                    Console.ResetColor();
                }
            }
        }
        static void CriarPastaSimples()
        {
            Console.WriteLine("\n--- Criar Pasta Simples ---");
            string diretorioBase = ObterCaminhoDoUsuario("Digite o caminho completo onde deseja criar a pasta (ex: C:\\MeusDocumentos)");
            if (string.IsNullOrEmpty(diretorioBase)) return;

            Console.Write("Digite o nome da pasta que deseja criar: ");
            string nomePasta = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(nomePasta))
            {
                Console.WriteLine("Nome da pasta não pode ser vazio.");
                return;
            }

            string caminhoCompletoPasta = Path.Combine(diretorioBase, nomePasta);
            try
            {
                if (Directory.Exists(caminhoCompletoPasta))
                {
                    Console.WriteLine($"A pasta '{nomePasta}' já existe em: {caminhoCompletoPasta}");
                }
                else
                {
                    Directory.CreateDirectory(caminhoCompletoPasta);
                    Console.WriteLine($"Pasta '{nomePasta}' criada com sucesso em: {caminhoCompletoPasta}");
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Ocorreu um erro ao criar a pasta: {ex.Message}");
                Console.ResetColor();
            }
        }
        static void CriarPastasPorIntervaloECategoriaUnica()
        {
            Console.WriteLine("\n--- Criar Apenas Pastas por Intervalo e Categoria Única ---");
            string diretorioBase = ObterCaminhoDoUsuario("Digite o caminho base para criar as pastas do projeto (ex: C:\\MeusProjetos)");
            if (string.IsNullOrEmpty(diretorioBase)) return;

            if (!ObterIntervaloECategoria(out int inicio, out int fim, out string categoria)) return;

            Console.WriteLine($"\nCriando pastas de {inicio} a {fim} com a categoria '{categoria}' em: {diretorioBase}");
            int pastasCriadas = 0;
            for (int i = inicio; i <= fim; i++)
            {
                string nomePastaComCategoria = $"{i}_{categoria}";
                string caminhoCompletoPasta = Path.Combine(diretorioBase, nomePastaComCategoria);
                try
                {
                    if (!Directory.Exists(caminhoCompletoPasta))
                    {
                        Directory.CreateDirectory(caminhoCompletoPasta);
                        Console.WriteLine($"  - Pasta '{nomePastaComCategoria}' criada com sucesso!");
                        pastasCriadas++;
                    }
                    else
                    {
                        Console.WriteLine($"  - A pasta '{nomePastaComCategoria}' já existe.");
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"  - Erro ao criar a pasta '{nomePastaComCategoria}': {ex.Message}");
                    Console.ResetColor();
                }
            }
            Console.WriteLine($"\nProcesso finalizado. {pastasCriadas} novas pastas foram criadas.");
        }
        static async Task AtribuirArquivosAPastasDeProjetoExistentes()
        {
            Console.WriteLine("\n--- Atribuir Thumbnails a Pastas de Projeto Existentes ---");

            string diretorioProjetos = ObterCaminhoDoUsuario("Digite o caminho que contém as pastas de projeto (ex: 135_Construção)");
            if (!Directory.Exists(diretorioProjetos))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Erro: O diretório dos projetos não existe.");
                Console.ResetColor();
                return;
            }

            string diretorioOrigem = ObterCaminhoDoUsuario("Digite o caminho da pasta de origem (mock) com as subpastas de categoria");
            if (!Directory.Exists(diretorioOrigem))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Erro: O diretório de origem (mock) não existe.");
                Console.ResetColor();
                return;
            }

            Console.Write("\nVocê deseja [C]opiar ou [M]over os arquivos de origem após criar o thumbnail? (Copiar é mais seguro) ");
            bool copiar = Console.ReadLine().Trim().ToUpper().StartsWith("C");
            string acaoFeita = copiar ? "criado (origem mantida)" : "criado (origem removida)";

            Console.WriteLine("\nCatalogando arquivos de origem...");
            var arquivosPorCategoria = new Dictionary<string, Queue<string>>();

            foreach (var parCategoria in CategoriasConhecidas)
            {
                string nomePastaCategoria = parCategoria.Value;
                string caminhoSubpastaCategoria = Path.Combine(diretorioOrigem, nomePastaCategoria);
                var arquivosEncontrados = new List<string>();
                if (Directory.Exists(caminhoSubpastaCategoria))
                {
                    arquivosEncontrados.AddRange(Directory.GetFiles(caminhoSubpastaCategoria));
                }
                arquivosPorCategoria[nomePastaCategoria] = new Queue<string>(arquivosEncontrados);
                Console.WriteLine($"  - Categoria '{nomePastaCategoria}': {arquivosEncontrados.Count} arquivo(s) encontrado(s).");
            }

            var pastasDeProjeto = Directory.GetDirectories(diretorioProjetos);
            Console.WriteLine($"\nEncontradas {pastasDeProjeto.Length} pastas de projeto. Iniciando atribuição de thumbnails...");
            int arquivosProcessados = 0;

            foreach (var caminhoPastaProjeto in pastasDeProjeto)
            {
                string nomePastaProjeto = new DirectoryInfo(caminhoPastaProjeto).Name;
                string[] partesNome = nomePastaProjeto.Split('_');
                if (partesNome.Length < 2)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"  - Ignorando pasta '{nomePastaProjeto}' (formato inválido, esperado 'ID_Categoria').");
                    Console.ResetColor();
                    continue;
                }
                string nomeCategoriaDaPasta = partesNome.Last();

                string nomeOficialCategoria = CategoriasConhecidas.Values
                    .FirstOrDefault(v => v.Equals(nomeCategoriaDaPasta, StringComparison.OrdinalIgnoreCase));

                if (string.IsNullOrEmpty(nomeOficialCategoria))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"  - Ignorando pasta '{nomePastaProjeto}'. Categoria '{nomeCategoriaDaPasta}' não reconhecida.");
                    Console.ResetColor();
                    continue;
                }

                if (arquivosPorCategoria.ContainsKey(nomeOficialCategoria) && arquivosPorCategoria[nomeOficialCategoria].Count > 0)
                {
                    string arquivoOriginal = arquivosPorCategoria[nomeOficialCategoria].Dequeue();
                    try
                    {
                        string nomeOriginalArquivo = Path.GetFileName(arquivoOriginal);
                        string caminhoThumbnail = Path.Combine(caminhoPastaProjeto, nomeOriginalArquivo);

                        using (var image = await SixLabors.ImageSharp.Image.LoadAsync(arquivoOriginal))
                        {
                            image.Mutate(x => x.Resize(new ResizeOptions
                            {
                                Size = new Size(600, 600),
                                Mode = ResizeMode.Max
                            }));

                            await image.SaveAsync(caminhoThumbnail, new JpegEncoder());
                        }

                        if (!copiar)
                        {
                            File.Delete(arquivoOriginal);
                        }

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"  - Thumbnail para '{nomeOriginalArquivo}' atribuído a '{nomePastaProjeto}'.");
                        Console.ResetColor();
                        arquivosProcessados++;
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"  - Erro ao processar arquivo para '{nomePastaProjeto}': {ex.Message}");
                        Console.ResetColor();
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"  - Nenhum arquivo de mock disponível para a categoria '{nomeOficialCategoria}'.");
                    Console.ResetColor();
                }
            }
            Console.WriteLine("\n--- Resumo da Operação ---");
            Console.WriteLine($"{arquivosProcessados} thumbnail(s) foram {acaoFeita} com sucesso.");
        }

        static void RemoverFotosDasPastasDeProjeto()
        {
            Console.WriteLine("\n--- Remover Fotos das Pastas de Projeto ---");

            string diretorioProjetos = ObterCaminhoDoUsuario("Digite o caminho que contém as pastas de projeto (ex: D:\\MeusProjetos\\Destino)");
            if (!Directory.Exists(diretorioProjetos))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Erro: O diretório especificado não existe.");
                Console.ResetColor();
                return;
            }

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("\nATENÇÃO: Esta ação irá apagar permanentemente todos os arquivos dentro das subpastas encontradas no caminho especificado.");
            Console.WriteLine("As pastas em si não serão removidas, apenas seu conteúdo.");
            Console.Write("Você tem certeza que deseja continuar? (S/N): ");
            Console.ResetColor();

            string confirmacao = Console.ReadLine().Trim().ToUpper();
            if (confirmacao != "S")
            {
                Console.WriteLine("Operação cancelada.");
                return;
            }

            Console.WriteLine("\nIniciando remoção de arquivos...");
            int arquivosRemovidos = 0;
            var subpastas = Directory.GetDirectories(diretorioProjetos);

            if (subpastas.Length == 0)
            {
                Console.WriteLine("Nenhuma subpasta encontrada para limpar.");
            }

            foreach (var pasta in subpastas)
            {
                try
                {
                    var arquivos = Directory.GetFiles(pasta);
                    if (arquivos.Length > 0)
                    {
                        Console.WriteLine($"  - Limpando {arquivos.Length} arquivo(s) de '{new DirectoryInfo(pasta).Name}'...");
                        foreach (var arquivo in arquivos)
                        {
                            File.Delete(arquivo);
                            arquivosRemovidos++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"  - Erro ao limpar a pasta '{new DirectoryInfo(pasta).Name}': {ex.Message}");
                    Console.ResetColor();
                }
            }

            Console.WriteLine("\n--- Resumo da Remoção ---");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{arquivosRemovidos} arquivo(s) foram removidos com sucesso.");
            Console.ResetColor();
        }

        private static bool ObterIntervalo(out int inicio, out int fim)
        {
            inicio = 0;
            fim = 0;
            Console.Write("Digite o número (ID) inicial do intervalo de projetos: ");
            if (!int.TryParse(Console.ReadLine(), out inicio))
            {
                Console.WriteLine("Entrada inválida. Por favor, digite um número inteiro.");
                return false;
            }
            Console.Write("Digite o número (ID) final do intervalo de projetos: ");
            if (!int.TryParse(Console.ReadLine(), out fim))
            {
                Console.WriteLine("Entrada inválida. Por favor, digite um número inteiro.");
                return false;
            }
            if (inicio > fim)
            {
                Console.WriteLine("Erro: O número inicial do intervalo não pode ser maior que o número final.");
                return false;
            }
            return true;
        }

        private static bool ObterIntervaloECategoria(out int inicio, out int fim, out string categoria)
        {
            categoria = "";
            if (!ObterIntervalo(out inicio, out fim))
            {
                return false;
            }
            Console.Write("Digite a categoria para as pastas (ex: Tecnologia): ");
            categoria = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(categoria))
            {
                Console.WriteLine("Erro: A categoria não pode ser vazia.");
                return false;
            }
            categoria = categoria.Trim().Replace(" ", "_");
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                categoria = categoria.Replace(c.ToString(), "");
            }
            return true;
        }
    }
}
