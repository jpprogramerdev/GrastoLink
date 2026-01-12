function imprimirPedido() {
    const conteudoImprimir = document.getElementById('area-impressao').innerHTML;
    const telaOriginal = document.body.innerHTML;

    document.body.innerHTML = conteudoImprimir;
    window.print();
    document.body.innerHTML = telaOriginal;
    location.reload();
}