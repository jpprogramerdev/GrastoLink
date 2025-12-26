function adicionarPedidoNaTela(pedido) {
    const container = document.getElementById("pedidos-container");

    const card = document.createElement("div");
    card.classList.add("card", "m-2");
    card.style.width = "18rem";
    card.setAttribute("data-pedido-id", pedido.id);

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
        li.innerHTML = `
            <p>${item.prato.nome} - Qtde. ${item.quantidade}</p>
            ${item.observacoes ? `<p>OBS: ${item.observacoes}</p>` : ""}
        `;
        ul.appendChild(li);
    });

    let containerBotoes;
    const cardExistente = document.querySelector(".card[data-pedido-id]");

    if (cardExistente) {
        containerBotoes = cardExistente
            .querySelector(".container-button")
            .cloneNode(true);
    } else {
        containerBotoes = document.createElement("div");
        containerBotoes.classList.add("container-button");

        const btnRemover = document.createElement("a");
        btnRemover.className = "btn btn-outline-danger btn-sm";
        btnRemover.textContent = "Remover";
        containerBotoes.appendChild(btnRemover);

        const btnStatus = document.createElement("button");
        btnStatus.type = "button";
        btnStatus.className = "btn btn-outline-info btn-sm";
        btnStatus.textContent = "Alterar Status";
        containerBotoes.appendChild(btnStatus);
    }

    const btnRemover = containerBotoes.querySelector("a");
    if (btnRemover) {
        btnRemover.href = `/Pedido/ExcluirPedido?pedidoId=${pedido.id}`;
        btnRemover.onclick = (e) => {
            e.preventDefault();
            card.remove();
            atualizarTotalPedidos();
        };
    }

    const btnStatus = containerBotoes.querySelector("button");
    if (btnStatus) {
        btnStatus.onclick = () => {
            card.remove();
            atualizarTotalPedidos();
        };
    }

    cardBody.appendChild(title);
    cardBody.appendChild(subtitle);
    cardBody.appendChild(ul);
    cardBody.appendChild(containerBotoes);

    card.appendChild(cardBody);

    container.appendChild(card);

    atualizarTotalPedidos();
}
