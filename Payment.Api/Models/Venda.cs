using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tech_test_payment_api.Payment.Api.Models
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

        EnumStatus _status;
        [Required(ErrorMessage ="Status não pode ser nulo!")]
        public EnumStatus Status 
        { 
            get
            {
                return _status <=0 ? _status = EnumStatus.AguardandoPagamento: _status;
            }
            set { _status = value;}
        }

        DateTime _data;
        [Required(ErrorMessage ="Data não pode ser nulo!")]
        [DisplayFormat(DataFormatString = "{0,d}")]
        public DateTime Data
        { 
              get 
              { 
                return DateTime.Compare(_data,new DateTime()) == 0 ?  _data = DateTime.Now: _data;
              } 
              set { _data = value;}
        }

    }
}