using System.ComponentModel.DataAnnotations;

namespace tech_test_payment_api.Models
{
    public class ItemDaVenda
    {
        [Required(ErrorMessage ="Id não pode ser nulo!")]
        [Range(1,10000)]
        public int Id { get; set; }

        [Required(ErrorMessage ="Id da Venda não pode ser nulo!")]
        [Range(1,10000)]
        public int idVenda { get; set; }

        [Required(ErrorMessage ="Id do Produto não pode ser nulo!")]
        [Range(1,10000)]
        public int iProduto { get; set; }

        [Required(ErrorMessage ="Produto não pode ser nulo!")]
        public Produto Produto { get; set; }

        [Required(ErrorMessage ="Valor não pode ser nulo!")]
        [DisplayFormat(DataFormatString = "{0,c}")]
        public double Valor { get; set; }

    }
}