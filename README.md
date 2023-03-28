## PROJETO: API DE PAGAMENTO.

- Afim de apresentar o projeto em funcionamento e falar um pouco mais da abordagem e tecnologias utilizadas, disponibilizo um link para um vídeo demonstrativo disponível em meu canal no youtube (Click no Gif abaixo 👇🏿):
  

<div align="center">
  <a href="https://youtu.be/4-O2PV1JUxs">
    <img alt="codando_api_projeto" align="center " width="350px" alt="GIF" src="https://github.com/TONBLACK42/api-pagamento/blob/main/GifDemonstracao.gif?raw=true">
  </a>
</div>

## A QUE O PROJETO SE PROPÕE?
 - Simular o Fluxo de pagamento para uma venda de um Produto/Serviço;
 - O objetivo é demonstrar meus conhecimentos em C#, Entity FrameWork, Banco de Dados. Além de, boas práticas e princípios como SOLID, DRY, OOP e padrões como MVC, REST, entre outros.

## RESUMO DE ESCOPO.
- Construir uma API REST utilizando .Net Core 6.0;
- A API deve expor uma rota com documentação swagger (http://.../api-docs).
- A API deve possuir 3 operações:
  1) Registrar venda: Recebe os dados do vendedor + itens vendidos. Registra venda com status "Aguardando pagamento";
  2) Buscar venda: Busca pelo Id da venda;
  3) Atualizar venda: Permite que seja atualizado o status da venda para os possíveis status:
  *  `Pagamento aprovado` | `Enviado para transportadora` | `Entregue` | `Cancelada`.
- Uma venda contém informação sobre o vendedor que a efetivou, data, identificador do pedido e os itens que foram vendidos;
- O vendedor deve possuir id, cpf, nome, e-mail e telefone;
- A inclusão de uma venda deve possuir pelo menos 1 item;
- A atualização de status deve permitir somente as seguintes transições: 
  - De: `Aguardando pagamento` Para: `Pagamento Aprovado`
  - De: `Aguardando pagamento` Para: `Cancelada`
  - De: `Pagamento Aprovado` Para: `Enviado para Transportadora`
  - De: `Pagamento Aprovado` Para: `Cancelada`
  - De: `Enviado para Transportador`. Para: `Entregue`
- A API não tem mecanismos de autenticação/autorização;
- A API utiliza um Banco de Dados "em memória".
