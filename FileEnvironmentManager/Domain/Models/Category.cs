namespace FileEnvironmentManager.Domain.Models
{
    public static class Category
    {
        public static readonly Dictionary<string, string> Known = new()
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
    }
}
