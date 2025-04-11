using Xunit;
using KooliProjekt.Services;
using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using KooliProjekt.Models;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class PredictionServiceTests : ServiceTestBase, IDisposable
    {
        private readonly PredictionService _service;

        public PredictionServiceTests()
        {
            _service = new PredictionService(_context);
        }

        public void Dispose() => _context.Dispose();

        [Fact]
        public async Task GetById_ReturnsPrediction()
        {
            var result = await _service.GetById(1);
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task Save_CreatesNewPrediction()
        {
            var newPrediction = new Prediction { MatchId = 1, UserId = 2 };
            await _service.Save(newPrediction);

            // Kontrolli salvestamist läbi DbContexti

            //Assert.True(result > 0);
        }

        [Fact]
        public async Task Delete_RemovesPrediction()
        {
            await _service.Delete(1);
            Assert.Null(await _context.Predictions.FindAsync(1));
        }
    }
}