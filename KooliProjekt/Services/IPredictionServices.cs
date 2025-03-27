using KooliProjekt.Data;
using KooliProjekt.Models;
using System.Threading.Tasks;

namespace KooliProjekt.Services
{
    public interface IPredictionService
    {
        Task<PagedResult<Prediction>> List(int page, int pageSize);
        Task<Prediction> GetById(int id);
        Task Save(Prediction prediction);
        Task Edit(Prediction prediction);
        Task Delete(int id);
        Task<bool> Exists(int id);
    }
}