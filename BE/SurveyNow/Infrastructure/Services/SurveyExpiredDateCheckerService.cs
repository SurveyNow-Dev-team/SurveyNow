using Application;
using Application.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services
{
    public class SurveyExpiredDateCheckerService : BackgroundService
    {
        private readonly ILogger<SurveyExpiredDateCheckerService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public SurveyExpiredDateCheckerService(ILogger<SurveyExpiredDateCheckerService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Expired surveys checker service executing!");
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation("Expired surveys checker service running at {currentTime}!", DateTime.UtcNow);
                    // Do work
                    await DoWork();

                    // Wait for a period of time
                    await Task.Delay(TimeSpan.FromHours(8), stoppingToken);
                }
            }
            catch (OperationCanceledException)
            {
                // When the stopping token is canceled, for example, a call made from services.msc,
                // we shouldn't exit with a non-zero exit code. In other words, this is expected...
                _logger.LogInformation("Expired surveys checker service cancelled!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        private async Task DoWork()
        {
            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                IUnitOfWork unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                IPointService pointService = scope.ServiceProvider.GetRequiredService<IPointService>();

                // Get all expired survey
                var expiredSurveyList = await unitOfWork.SurveyRepository.GetExpiredSurvey();
                if (expiredSurveyList == null || expiredSurveyList.Count == 0)
                {
                    _logger.LogInformation("No expired survey found!");
                    return;
                }
                _logger.LogInformation("Found {surveyCount} expired survey(s). Begining to process", expiredSurveyList.Count);
                foreach (var survey in expiredSurveyList)
                {
                    // Change status
                    survey.Status = Domain.Enums.SurveyStatus.Expired;
                    int missingParticipant = survey.TotalParticipant - survey.TotalAnswer;
                    try
                    {
                        await unitOfWork.BeginTransactionAsync();
                        unitOfWork.SurveyRepository.Update(survey);
                        await unitOfWork.SaveChangeAsync();
                        // Return point (If necessary)
                        if (missingParticipant > 0)
                        {
                            var pointRewardAmount = await pointService.GetSurveyRewardPointAmount(survey.Id);
                            var refundAmount = missingParticipant * pointRewardAmount;
                            string message = "Hoàn điểm cho khảo sát không đủ số người điền khi kết thúc";
                            await pointService.RefundPointForUser(survey.CreatedUserId, refundAmount, message);
                        }
                        await unitOfWork.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await unitOfWork.RollbackAsync();
                        _logger.LogError("Error occurred when trying to process expired survey" + ex.Message);
                        throw;
                    }
                }
                _logger.LogInformation("Successfully processed {surveyCount} expired survey(s)", expiredSurveyList.Count);
            }
        }
    }
}
