using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tech_test_payment_api.Models
{
    public class Venda
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Vendedor não pode ser nulo!")]
        public virtual Vendedor Vendedores { get; set; }

        [Required(ErrorMessage ="Item da Venda não pode ser nulo!")]
        public virtual ICollection<ItemDaVenda> ItensDaVenda { get; set; }

        [Required(ErrorMessage ="Total não pode ser nulo!")]
        [DisplayFormat(DataFormatString = "{0,c}")]
        public double Total { get; set; }

        [Required(ErrorMessage ="Status não pode ser nulo!")]
        public EnumStatus Status { get; set; }

        [Required(ErrorMessage ="Data não pode ser nulo!")]
        [DisplayFormat(DataFormatString = "{0,d}")]
        public DateTime Data { get; set; }

    }
}