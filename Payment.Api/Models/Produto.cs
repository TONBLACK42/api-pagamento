using System.ComponentModel.DataAnnotations;

namespace tech_test_payment_api.Payment.Api.Models
{
    public class Produto
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="por favor informe um nome para o produto.",AllowEmptyStrings =false)]
        public string Nome { get; set; }

        [Required(ErrorMessage ="Quantidade não pode ser nulo!")]
        //[MinLength(1), MaxLength(3)]
        public int Quantidade { get; set; }

        [Required(ErrorMessage ="Valor não pode ser nulo!")]
        [DisplayFormat(DataFormatString = "{0,c}")]
        public double Valor { get; set; }

        //public virtual ItemDaVenda ItemDaVenda { get; set; }
    }
}