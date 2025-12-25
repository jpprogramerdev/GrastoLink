let index = 0;

function adicionarItemPedido() {
    const template = document.getElementById('item-template');
    const clone = template.content.cloneNode(true);

    clone.querySelector(".prato").setAttribute("name", `PedidoRequestDTO.ItensPedido[${index}].PratoId`);

    clone.querySelector(".quantidade").setAttribute("name", `PedidoRequestDTO.ItensPedido[${index}].Quantidade`);

    clone.querySelector(".observacao").setAttribute("name", `PedidoRequestDTO.ItensPedido[${index}].Observacoes`);

    document.getElementById('itens').appendChild(clone);

    index++;
}