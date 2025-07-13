using HealthCare.Project.Core.Repository.Contract;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Project.Core.Helper
{
    public class ExpiredMedicineCleanupService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ExpiredMedicineCleanupService> _logger;

        public ExpiredMedicineCleanupService(IServiceProvider serviceProvider, ILogger<ExpiredMedicineCleanupService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var medicineRepository = scope.ServiceProvider.GetRequiredService<IMedicineRepository>();
                        await medicineRepository.DeleteExpiredMedicinesAsync(DateOnly.FromDateTime(DateTime.UtcNow));

                        _logger.LogInformation("Expired medicines deleted successfully at {Time}", DateTime.UtcNow);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while deleting expired medicines.");
                }

                //  Run once a day (24 hours)
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }
        }
    }
}
