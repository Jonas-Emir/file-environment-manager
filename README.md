# App Console - Automação e Manipulação de pastas e arquivos

Uma ferramenta de linha de comando em .NET para organização automatizada de arquivos e pastas, criada a partir de uma necessidade real nos testes de um projeto, surgiu uma necessidade bastante comum: organizar arquivos em massa de forma dinâmica durante os testes. No meu caso, eu precisava gerar uma massa de diretórios com base em IDs de categorias e mover/copiar imagens mock para esses diretórios.

A repetição e o risco de erro rapidamente tornaram claro que eu precisava de uma solução rápida, reutilizável e que se adaptasse facilmente às diferentes variações dos testes e para não fazer isso manualmente quando necessário, desenvolvi este pequeno app, que acabou se tornando uma ferramenta útil para qualquer tarefa que envolva organização de arquivos e pastas de forma automatizada e focado em resolver exatamente esse tipo de tarefa.

A ferramenta permite configurar um diretório de origem contendo os arquivos a serem organizados e, a partir de uma lista de identificadores ou critérios personalizados, cria dinamicamente a estrutura de diretórios desejada no destino, em seguida, move ou copia os arquivos gerando automaticamente thumbnails das imagens nos diretórios de destino.

## Demonstração
![gifDemo](https://github.com/Jonas-Emir/file-environment-manager/blob/main/AppConsole-Demo.gif)

A lógica é simples, mas flexível, no meu caso, utilizei IDs e Categorias como base para a criação das pastas de destino, gerando diretórios como `./12_tecnologia/`, `./34_saude/`, `./56_marketing/`, e organizando as imagens mock de acordo com esse critérios, no entanto, a ferramenta pode facilmente ser adaptada para outros critérios, como nome de cliente, data de criação, tipo de arquivo ou qualquer outra regra que fizer sentido no seu cenário.

Essa ferramenta nasceu de uma demanda específica, mas se mostrou útil o bastante para entrar na minha "caixa de ferramentas" de desenvolvimento. Muitas vezes, soluções assim surgem de problemas pequenos, mas quando transformadas em código, acabam ganhando vida longa em outros contextos.

## Como rodar

Para executar este projeto, você precisará ter o SDK do .NET instalado em sua máquina. Você pode baixá-lo diretamente do site oficial da Microsoft.

Após clonar o repositório, navegue até a pasta do projeto e execute o app com o .NET CLI:
```bash
git clone https://github.com/Jonas-Emir/file-environment-manager.git
cd file-environment-manager-main\FileEnvironmentManager
dotnet run
```

## Aberto a melhorias

O `file-environment-manager` nasceu como uma solução simples para um problema específico, mas tem potencial para crescer e se adaptar a novos cenários. Se você tiver ideias de melhoria, quiser adicionar novos modos de operação, integrar com outras fontes de dados ou apenas ajustar algo para o seu uso, fique à vontade para modificar, reutilizar ou contribuir.

Pull requests e sugestões são muito bem-vindos.

