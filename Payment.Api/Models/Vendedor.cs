using System.ComponentModel.DataAnnotations;

namespace tech_test_payment_api.Payment.Api.Models
{
    public class Vendedor
    { 
        public int Id { get; set; }
        
        [Required(ErrorMessage ="por favor informe um nome.",AllowEmptyStrings =false)]
        public string Nome { get; set; }

        [Required(ErrorMessage ="É necessário informar o CPF.", AllowEmptyStrings =false)]
        [RegularExpression(@"[0-9]{3}\.?[0-9]{3}\.?[0-9]{3}\-?[0-9]{2}",ErrorMessage ="CPF Inválido!")]
        public string CPF { get; set; }

        [Required(ErrorMessage ="Por favor informe um e-mail.",AllowEmptyStrings =false)]
        [EmailAddress(ErrorMessage ="E-mail Invalido!")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Por favor informe um e-mail.", AllowEmptyStrings =false)]
        [Phone(ErrorMessage ="Telefone Invalido!")]
        public string Telefone { get; set; }

        public int VendaID { get; set; }       
    }
}