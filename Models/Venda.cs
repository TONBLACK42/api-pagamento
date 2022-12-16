using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace tech_test_payment_api.Models
{
    public class Venda
    {

        [Required(ErrorMessage ="Id não pode ser nulo!")]
        [Range(1,10000)]
        public int Id { get; set; }

        [Required(ErrorMessage ="Id do Vendedor não pode ser nulo!")]
        [Range(1,10000)]
        public int IdVendedor { get; set; }

        [Required(ErrorMessage ="Vendedor não pode ser nulo!")]
        public Vendedor Vendedor { get; set; }

        [Required(ErrorMessage ="Item da Venda não pode ser nulo!")]
        public List<ItemDaVenda> ItensDaVenda { get; set; }

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