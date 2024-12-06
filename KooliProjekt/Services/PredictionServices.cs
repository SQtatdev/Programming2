using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class PredictionService : IPredictionService
    {
        private readonly ApplicationDbContext _context;

        public PredictionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Prediction>> List(int page, int pageSize)
        {
            return await _context.Predictions.GetPagedAsync(page, pageSize);
        }

        public async Task<Prediction> Get(int id)
        {
            return await _context.Predictions.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task Save(Prediction prediction)
        {
            if (prediction.Id == 0)
            {
                _context.Predictions.Add(prediction);
            }
            else
            {
                _context.Predictions.Update(prediction);
            }

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var prediction = await _context.Predictions.FindAsync(id);
            if (prediction != null)
            {
                _context.Predictions.Remove(prediction);
                await _context.SaveChangesAsync();
            }
        }
    }
}
