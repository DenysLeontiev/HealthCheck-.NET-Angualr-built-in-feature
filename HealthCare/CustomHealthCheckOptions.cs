using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;

namespace HealthCare
{
	public class CustomHealthCheckOptions : HealthCheckOptions
	{
		public CustomHealthCheckOptions() : base()
		{
			JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
			{
				WriteIndented = true
			};

			ResponseWriter = async (context, response) =>
			{
				context.Response.ContentType = MediaTypeNames.Application.Json;
				context.Response.StatusCode = StatusCodes.Status200OK;

				var result = JsonSerializer.Serialize(new
				{
					checks = response.Entries.Select(e => new // Entries contain results from each check(its key refers to object,where info about HeathCheck is stored)
					{
						name = e.Key,
						responseTime = e.Value.Duration.TotalMilliseconds,
						status = e.Value.Status.ToString(),
						description = e.Value.Description
					}),

					totalStatus = response.Status,
					totalResponseTime = response.TotalDuration
				},jsonSerializerOptions);

				await context.Response.WriteAsync(result);
			};
		}
	}
}
