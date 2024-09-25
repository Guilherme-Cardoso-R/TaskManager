namespace TaskManager.Models
{
    public class Tarefa
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string Prioridade { get; set; }
        public string Status { get; set; }
        public DateTime DataPrazo { get; set; }
        public string Responsavel { get; set; }
    }
}
