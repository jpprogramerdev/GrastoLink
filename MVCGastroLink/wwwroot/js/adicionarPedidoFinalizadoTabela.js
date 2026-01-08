function adicionarPedidoNaTela(pedido) {
    const tableBody = document.getElementById('tbody-pedidos');

    if (!tableBody) return;

    const row = document.createElement('tr');

    row.innerHTML = `
        <th scope="row">${pedido.id}</th>
        <td>${pedido.itens.length}</td>
        <td>R$ ${pedido.valorTotal.toFixed(2)}</td>
        <td>${pedido.mesa.numero}</td>
    `;

    tableBody.prepend(row);
}