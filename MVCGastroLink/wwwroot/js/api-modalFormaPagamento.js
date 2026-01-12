document.addEventListener('DOMContentLoaded', function () {
    fetch('https://localhost:7071/api-gastrolink/formas-pagamento')
        .then(response => {
            if (!response.ok) {
                throw new Error('Erro ao buscar formas de pagamento');
            }
            return response.json();
        })
        .then(tipos => {
            const container = document.getElementById('tipos-pagamento');
            const hiddenInput = document.getElementById('formaPagamentoId');

            tipos.forEach(tipo => {
                const btn = document.createElement('button');
                btn.type = 'button';
                btn.className = 'btn btn-outline-primary m-2';
                btn.value = tipo.id;
                btn.textContent = tipo.forma;

                btn.addEventListener('click', function () {
                    document.querySelectorAll('#tipos-pagamento button').forEach(b => b.classList.remove('active'));

                    this.classList.add('active');

                    hiddenInput.value = tipo.id;
                });

                container.appendChild(btn);
            })
        })
        .catch(error => {
            console.error('Erro:', error);
        });
});