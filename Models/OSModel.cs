using System.ComponentModel.DataAnnotations;

public class OSModel
    {
        [Key]
        public int NumeroOS { get; set; }

        [Required]
        public string TituloServico { get; set; }

        [Required]
        [StringLength(18)]
        public string CNPJ { get; set; }

        [Required]
        public string NomeDoCliente { get; set; }

        [Required]
        [StringLength(14)]
        public string CPF { get; set; }

        [Required]
        public string NomeDoPrestador { get; set; }

        [Required]
        public DateTime DataExecucaoServico { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal ValorDoServico { get; set; }
    }


