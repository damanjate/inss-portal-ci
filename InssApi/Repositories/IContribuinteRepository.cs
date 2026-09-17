using InssApi.Models;
using InssApi.Repositories;

namespace InssApi.Repositories;

public interface IContribuinteRepository
{
    Task<Contribuinte?> ObterPorIdAsync(int id, CancellationToken ct);
    Task<Contribuinte?> ObterPorNuitAsync(string Nuit, CancellationToken ct);

    Task<IReadOnlyList<Contribuinte>> ListarAsync(CancellationToken ct);

    Task<Contribuinte?> ObterComPedidosAsync(int id, CancellationToken ct);

    Task AdicionarAsync(Contribuinte contribuite, CancellationToken ct);

    void Remover(Contribuinte contribuite);

    Task SalvarAsync(CancellationToken ct);
}

