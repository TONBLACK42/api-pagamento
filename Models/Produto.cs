using System.ComponentModel.DataAnnotations;

namespace tech_test_payment_api.Models
{
    public class Produto
    {
        [Required(ErrorMessage ="Id não pode ser nulo!")]
        [Range(1,10000)]
        public int Id { get; set; }

        [Required(ErrorMessage ="por favor informe um nome para o produto.",AllowEmptyStrings =false)]
        public string Nome { get; set; }

        [Required(ErrorMessage ="Quantidade não pode ser nulo!")]
        [Range(1,100)]
        public int Quantidade { get; set; }

        [Required(ErrorMessage ="Valor não pode ser nulo!")]
        [DisplayFormat(DataFormatString = "{0,c}")]
        public double Valor { get; set; }
    }
}