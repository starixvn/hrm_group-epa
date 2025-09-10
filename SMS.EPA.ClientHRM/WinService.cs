namespace SMS.EPA.ClientHrm
{
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using SMS.EPA.ClientHrm.Services;
using SMS.EPA.ClientHRM.Jobs;

public class WinService : BackgroundService
    {
        private readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(WinService));

        private readonly ISyncEPADataService epaService;
        private readonly IUserMappingService userMappingService;
        private readonly IUnitHeadService unitHeadService;

        private readonly JobScheduler scheduler;

        public WinService(
            ISyncEPADataService epaService,
            IUserMappingService userMappingService,
            IUnitHeadService unitHeadService)
        {
            this.epaService = epaService;
            this.userMappingService = userMappingService;
            this.unitHeadService = unitHeadService;

            scheduler = new JobScheduler();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                logger.Info("HRMService: Starting");

                scheduler.Start(epaService, userMappingService, unitHeadService);

                // Keep the service running until cancellation is requested
                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(1000, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                logger.Error("HRMService: Error -- " + ex.Message, ex);
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                logger.Info("HRMService: Stopping");
                scheduler.Stop();
            }
            catch (Exception ex)
            {
                logger.Error("HRMService: Stop -- " + ex.Message, ex);
            }

            await base.StopAsync(cancellationToken);
        }
    }
}