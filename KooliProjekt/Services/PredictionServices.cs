using KooliProjekt.Data;
using KooliProjekt.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Services
{
    public class PredictionService : IPredictionService
    {
        private readonly Data.ApplicationDbContext _context;

        public PredictionService(Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Prediction>> List(int page, int pageSize)
        {
            var query = _context.Predictions.OrderBy(p => p.Id).AsNoTracking();

            return await query.ToPagedResult(page, pageSize);
        }

        public async Task<Prediction> GetById(int id)
        {
            return await _context.Predictions.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task Save(Prediction prediction)
        {
            _context.Predictions.Add(prediction);
            await _context.SaveChangesAsync();
        }

        public async Task Edit(Prediction prediction)
        {
            _context.Predictions.Update(prediction);
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

        public async Task<bool> Exists(int id)
        {
            return await _context.Predictions.AnyAsync(p => p.Id == id);
        }
    }
}