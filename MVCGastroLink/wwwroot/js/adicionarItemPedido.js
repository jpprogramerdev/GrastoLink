let index = 0;

function adicionarItemPedido() {
    const template = document.getElementById('item-template');
    const clone = template.content.cloneNode(true);

    const container = clone.querySelector("div");

    container.querySelector("button").onclick = function () {
        container.remove();
        reindexarItens();
    };

    document.getElementById('itens').appendChild(clone);

    reindexarItens();
}

function reindexarItens() {
    const itens = document.querySelectorAll("#itens > div");

    itens.forEach((item, i) => {
        item.querySelector(".prato")
            .setAttribute("name", `PedidoRequestDTO.ItensPedido[${i}].PratoId`);

        item.querySelector(".quantidade")
            .setAttribute("name", `PedidoRequestDTO.ItensPedido[${i}].Quantidade`);

        item.querySelector(".observacao")
            .setAttribute("name", `PedidoRequestDTO.ItensPedido[${i}].Observacoes`);
    });

    index = itens.length;
}
