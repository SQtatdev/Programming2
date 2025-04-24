using KooliProjekt.Data;
using KooliProjekt.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            return await _context.Predictions
                .Include(p => p.MatchId)
                .Include(p => p.UserId)
                .OrderBy(p => p.Id)
                .AsNoTracking()
                .ToPagedResult(page, pageSize);
        }

        public async Task<Prediction> GetById(int id)
        {
            return await _context.Predictions
                .Include(p => p.MatchId)
                .Include(p => p.UserId)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> Save(Prediction prediction)
        {
            try
            {
                _context.Predictions.Add(prediction);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Edit(Prediction prediction)
        {
            try
            {
                var existing = await _context.Predictions.FindAsync(prediction.Id);
                if (existing == null) return false;

                _context.Entry(existing).CurrentValues.SetValues(prediction);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Delete(int id)
        {
            var prediction = await _context.Predictions.FindAsync(id);
            if (prediction == null) return false;

            _context.Predictions.Remove(prediction);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Exists(int id)
        {
            return await _context.Predictions.AnyAsync(p => p.Id == id);
        }
    }
}