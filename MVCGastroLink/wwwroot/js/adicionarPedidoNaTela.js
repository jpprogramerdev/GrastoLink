function adicionarPedidoNaTela(pedido) {
    const container = document.getElementById("pedidos-container");

    const card = document.createElement("div");
    card.classList.add("card", "m-2");
    card.style.width = "18rem";

    const cardBody = document.createElement("div");
    cardBody.classList.add("card-body");

    const title = document.createElement("h5");
    title.classList.add("card-title");
    title.textContent = `PEDIDO ${pedido.id} - ${new Date(pedido.dataHora).toLocaleString()}`;

    const subtitle = document.createElement("h6");
    subtitle.classList.add("card-subtitle", "mb-2", "text-muted");
    subtitle.textContent = `MESA ${pedido.mesa.numero}`;

    const ul = document.createElement("ul");
    ul.classList.add("list-group");

    pedido.itens.forEach(item => {
        const li = document.createElement("li");
        li.classList.add("list-group-item");
        li.textContent = `${item.prato.nome} - Qtde. ${item.quantidade}`;
        ul.appendChild(li);
    });

    cardBody.appendChild(title);
    cardBody.appendChild(subtitle);
    cardBody.appendChild(ul);
    card.appendChild(cardBody);

    container.appendChild(card);
}