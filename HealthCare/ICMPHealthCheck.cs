using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace HealthCare
{
	public class ICMPHealthCheck : IHealthCheck
	{
		private readonly string HOST;
		private readonly int HealthCheckTripTime;

		public ICMPHealthCheck(string host, int healthCheckTripTime)
		{
			HOST = host;
			HealthCheckTripTime = healthCheckTripTime;
		}

		public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
		{
			try
			{
				using var ping = new Ping();
				var response = await ping.SendPingAsync(HOST);

				var msg = $"Name: {HOST}, succeded in: {response.RoundtripTime}";
				switch (response.Status)
				{
					case IPStatus.Success:
						return (response.RoundtripTime < HealthCheckTripTime) ? HealthCheckResult.Healthy(msg) : HealthCheckResult.Degraded(msg);
					default:
						var message = $"Failed: {HOST} in {response.RoundtripTime}";
						return HealthCheckResult.Unhealthy(message);
				}
			}
			catch (Exception ex)
			{
				string message = $"Error: {HOST}, details: {ex.Message}";
				return HealthCheckResult.Unhealthy(message);
			}
		}
	}
}
