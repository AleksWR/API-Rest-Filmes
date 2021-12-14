using System.ComponentModel.DataAnnotations;

namespace API_Filmes.Models
{
    public class Filme
    {
        [Required]
        public int Id { get; set; }

        [Required (ErrorMessage = "Nome não foi inserido")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Autor não foi inserido")]
        public string Autor { get; set; }

        [Required(ErrorMessage = "Duração não foi inserido")]
        [Range (1,600)]
        public int Duracao { get; set; }

    }
}