using FileEnvironmentManager.Domain.Interfaces;
using FileEnvironmentManager.Utils;

namespace FileEnvironmentManager.Application.Menu
{
    public class MenuService
    {
        private readonly IFolderService _folderService;
        private readonly IImageService _imageService;

        public MenuService(IFolderService folderService, IImageService imageService)
        {
            _folderService = folderService;
            _imageService = imageService;
        }

        public async Task StartAsync()
        {
            await ShowMainMenu();
        }

        public async Task ShowMainMenu()
        {
            while (true)
            {
                Console.WriteLine("\nEscolha uma opção:");
                Console.WriteLine("1 - Criar pasta simples");
                Console.WriteLine("2 - Criar pastas por intervalo e categoria única");
                Console.WriteLine("3 - Atribuir thumbnails a pastas existentes");
                Console.WriteLine("4 - Remover todos os arquivos das pastas");
                Console.WriteLine("0 - Sair");
                Console.Write("Opção: ");
                var option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        {
                            var base1 = ConsoleHelper.AskPath("Digite o caminho base:");
                            var name = ConsoleHelper.Ask("Digite o nome da pasta:");
                            try
                            {
                                _folderService.CreateSimpleFolder(base1, name);
                                ConsoleHelper.WriteSuccess($"Pasta '{name}' criada com sucesso em '{base1}'.");
                            }
                            catch (Exception ex)
                            {
                                ConsoleHelper.WriteError($"Erro ao criar a pasta: {ex.Message}");
                            }
                            break;
                        }
                    case "2":
                        {
                            var base2 = ConsoleHelper.AskPath("Digite o caminho base:");
                            ConsoleHelper.AskInterval(out int start, out int end);
                            var cat = ConsoleHelper.Ask("Digite a categoria:");
                            try
                            {
                                _folderService.CreateFoldersWithCategory(base2, start, end, cat);
                                ConsoleHelper.WriteSuccess($"Pastas de {start} a {end} com a categoria '{cat}' criadas em '{base2}'.");
                            }
                            catch (Exception ex)
                            {
                                ConsoleHelper.WriteError($"Erro ao criar pastas: {ex.Message}");
                            }
                            break;
                        }
                    case "3":
                        {
                            var target = ConsoleHelper.AskPath("Digite o caminho do projeto:");
                            var source = ConsoleHelper.AskPath("Digite o caminho da origem:");
                            var copy = ConsoleHelper.Ask("Copiar ou mover? (C/M):").ToUpper().StartsWith("C");
                            try
                            {
                                await ConsoleAnimation.RunWithSpinner(() =>
                                      _imageService.AssignThumbnailsToProjectFolders(target, source, copy),
                                      "Atribuindo thumbnails"
                                  );
                                Console.WriteLine("Operação concluída!");

                                var action = copy ? "copiados" : "movidos";
                                ConsoleHelper.WriteSuccess($"Thumbnails {action} com sucesso para as pastas do projeto em '{target}'.");
                            }
                            catch (Exception ex)
                            {
                                ConsoleHelper.WriteError($"Erro ao atribuir thumbnails: {ex.Message}");
                            }
                            break;
                        }
                    case "4":
                        {
                            var dir = ConsoleHelper.AskPath("Digite o caminho base dos projetos:");
                            try
                            {
                                await ConsoleAnimation.RunWithSpinner(() =>
                                {
                                    _folderService.RemoveAllFilesFromFolders(dir);
                                    return Task.CompletedTask;
                                }, "Removendo arquivos");
                                Console.WriteLine("Arquivos removidos com sucesso.");
                                ConsoleHelper.WriteSuccess($"Todos os arquivos removidos das pastas em '{dir}'.");
                            }
                            catch (Exception ex)
                            {
                                ConsoleHelper.WriteError($"Erro ao remover arquivos: {ex.Message}");
                            }
                            break;
                        }
                    case "0":
                        ConsoleHelper.WriteInfo("Saindo do programa...");
                        return;
                    default:
                        ConsoleHelper.WriteError("Opção inválida. Por favor, tente novamente.");
                        break;
                }
            }
        }
    }
}
