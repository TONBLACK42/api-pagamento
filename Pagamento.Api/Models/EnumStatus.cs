namespace api_pagamento.Pagamento.Api.Models
{
    public enum EnumStatus
    {
        AguardandoPagamento =1,
        PagamentoAprovado,
        EnviandoParaTransportadora,
        Entregue,
        Cancelada
    }
}