using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

public class OSModel
{
    [Key]
    public int NumeroOS { get; set; }

    [Required]
    public string TituloServico { get; set; }

    [Required]
    [StringLength(18)]
    [CustomValidationCNPJ(ErrorMessage = "CNPJ inválido")]
    public string CNPJ { get; set; }

    [Required]
    public string NomeDoCliente { get; set; }

    [Required]
    [StringLength(14)]
    [CustomValidationCPF(ErrorMessage = "CPF inválido")]
    public string CPF { get; set; }

    [Required]
    public string NomeDoPrestador { get; set; }

    [Required]
    public DateTime DataExecucaoServico { get; set; }

    [Required]
    [DataType(DataType.Currency)]
    public decimal ValorDoServico { get; set; }
}

// Validação personalizada de CPF
public class CustomValidationCPFAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        string cpf = value as string;

        if (IsValidCPF(cpf))
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }

    private bool IsValidCPF(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            return false;

        cpf = Regex.Replace(cpf, @"[^\d]", "");

        if (cpf.Length != 11)
            return false;

        // Verifica se todos os dígitos são iguais, que não é um CPF válido
        if (new string(cpf[0], cpf.Length) == cpf)
            return false;

        int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        string tempCpf = cpf.Substring(0, 9);
        int soma = 0;

        for (int i = 0; i < 9; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

        int resto = soma % 11;
        if (resto < 2)
            resto = 0;
        else
            resto = 11 - resto;

        string digito = resto.ToString();
        tempCpf = tempCpf + digito;
        soma = 0;

        for (int i = 0; i < 10; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

        resto = soma % 11;
        if (resto < 2)
            resto = 0;
        else
            resto = 11 - resto;

        digito = digito + resto.ToString();

        return cpf.EndsWith(digito);
    }
}

// Validação personalizada de CNPJ
public class CustomValidationCNPJAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        string cnpj = value as string;

        if (IsValidCNPJ(cnpj))
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }

    private bool IsValidCNPJ(string cnpj)
    {
        if (string.IsNullOrWhiteSpace(cnpj))
            return false;

        cnpj = Regex.Replace(cnpj, @"[^\d]", "");

        if (cnpj.Length != 14)
            return false;

        int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

        string tempCnpj = cnpj.Substring(0, 12);
        int soma = 0;

        for (int i = 0; i < 12; i++)
            soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

        int resto = (soma % 11);
        if (resto < 2)
            resto = 0;
        else
            resto = 11 - resto;

        string digito = resto.ToString();
        tempCnpj = tempCnpj + digito;
        soma = 0;

        for (int i = 0; i < 13; i++)
            soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

        resto = (soma % 11);
        if (resto < 2)
            resto = 0;
        else
            resto = 11 - resto;

        digito = digito + resto.ToString();

        return cnpj.EndsWith(digito);
    }
}


