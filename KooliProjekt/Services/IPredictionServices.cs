using KooliProjekt.Data;

namespace KooliProjekt.Services
{
    public interface IPredictionService
    {
        Task<PagedResult<Prediction>> List(int page, int pageSize);
        Task<Prediction> Get(int id);
        Task Save(Prediction prediction);
        Task Delete(int id);
    }
}
