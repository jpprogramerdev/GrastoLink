function tocarSomNovoPedido() {
    const audio = document.getElementById('pedidoNovoSound');

    if (audioLiberado) {
        audio.currentTime = 0;
        audio.play().catch(err => console.warn("Falha ao tocar som:", err));
    }
}