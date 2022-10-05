using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ProjectDK.HealthChecks
{
    public class TestHealthCheck : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
            }
            catch(Exception ex)
            {
                return HealthCheckResult.Unhealthy(ex.Message);
            }
            return HealthCheckResult.Healthy("SQL connection is OK");
        }
    }
}
