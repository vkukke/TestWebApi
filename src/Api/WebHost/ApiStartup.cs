// -----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// -----------------------------------------------------------------------

using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HostFiltering;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api
{
    public class ApiStartup : IStartup
    {
        private readonly IConfiguration configuration;
        private readonly IHostingEnvironment env;

        public ApiStartup(IConfiguration configuration, IHostingEnvironment env)
        {
            this.configuration = configuration;
            this.env = env;
        }

        // Note: the typical sample for aspnet has static methods for configureServices and configure, but has a non-static Startup class hosting them (and taking in stuff via constructor injection)
        // The WebHostBuilder ends up using reflection to call the COnfigureServices/Configure methods.
        // Its one-less-concept to just implement the IStartup interface and remove the static business.

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().ConfigureApplicationPartManager(p => p.ApplicationParts.Add(new AssemblyPart(this.GetType().Assembly))).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddHostFiltering(ConfigureOptions);
            return services.BuildServiceProvider();

            void ConfigureOptions(HostFilteringOptions options)
            {
                options.AllowEmptyHosts = false;
                if (options.AllowedHosts == null || options.AllowedHosts.Count == 0)
                {
                    // "AllowedHosts": "localhost;127.0.0.1"
                    string[] hosts = this.configuration["AllowedHosts"]?.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                    // Fall back to "*" to disable.
                    options.AllowedHosts = hosts?.Length > 0 ? hosts : new[] { "*" };
                }
            }
        }

        public void Configure(IApplicationBuilder app)
        {
            // TODO: Replace with proper api exception handling here, and make sure x-ms-request-id shows up on the response headers even when we have an exception.
            app.UseHsts();
            // TODO: Add SSL Redirection when we have SSL setup
            app.UseHostFiltering();
            // TODO: Add Connection: keep-alive for non-exceptional requests
            // TODO: Add api-level metrics reporting here (counts, latencies, exceptions)
            app.UseMvc();
        }
    }
}