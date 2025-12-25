using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using MVCGastroLink.DTO;

namespace MVCGastroLink.ViewsModel {
    public class CriarPedidoViewlModel {
        [ValidateNever]
        public List<MesaDTO> MesasDTO { get; set; }

        public PedidoRequestDTO PedidoRequestDTO { get; set; }

        [ValidateNever]
        public List<PratoDTO> PratosDTO { get; set; }
    }
}
