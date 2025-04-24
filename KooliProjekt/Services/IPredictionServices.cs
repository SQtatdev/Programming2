using KooliProjekt.Models;
using System.Threading.Tasks;

namespace KooliProjekt.Services
{
    public interface IPredictionService
    {
        Task<PagedResult<Prediction>> List(int page, int pageSize);
        Task<Prediction> GetById(int id);
        Task<bool> Save(Prediction prediction);
        Task<bool> Edit(Prediction prediction);
        Task<bool> Delete(int id);
        Task<bool> Exists(int id);
    }
}