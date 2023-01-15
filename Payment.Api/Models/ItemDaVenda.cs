using System.ComponentModel.DataAnnotations;

namespace tech_test_payment_api.Payment.Api.Models
{
    public class ItemDaVenda
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Quantidade de Itens não pode ser nulo!")]
        public int Quantidade { get; set; }

        [Required(ErrorMessage ="Valor total dos Itens não pode ser nulo!")]
        [DisplayFormat(DataFormatString = "{0,c}")]
        public double Valor { get; set; }

        public int VendaID { get; set; }

        [Required(ErrorMessage ="Produto não pode ser nulo!")]
        public virtual Produto Produto { get; set; }
        
    }
}