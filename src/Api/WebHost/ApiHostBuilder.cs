// -----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// -----------------------------------------------------------------------

using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Api
{
    public static class ApiHostBuilder
    {
        public static IWebHost Build(params string[] args)
        {
            IConfigurationRoot config = BuildConfig(args);

            IWebHostBuilder builder = new WebHostBuilder()
                .UseConfiguration(config)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseKestrel((context, options) =>
                {
                    IConfigurationSection kestrelConfig = context.Configuration.GetSection("kestrel");
                    options.Configure(kestrelConfig);
                    options.AddServerHeader = kestrelConfig.GetValue("addServerHeader", false);
                    options.Limits.MaxRequestBodySize = kestrelConfig.GetValue("maxRequestBodySize", 1024 * 1034); // allow 10 extra KB over the 1 MB payload
                    options.Limits.KeepAliveTimeout = TimeSpan.FromSeconds(kestrelConfig.GetValue("keepAliveTimeoutInSeconds", 300)); // default of 120 seconds
                })
                .ConfigureLogging((WebHostBuilderContext context, ILoggingBuilder logging) =>
                {
                    logging
                    .AddConfiguration(context.Configuration.GetSection("logging"))
                    .AddConsole();
                    // TODO Logging: .AddEventSourceLogger
                })
                .ConfigureServices((WebHostBuilderContext context, IServiceCollection services) =>
                {
                })
                .UseStartup<ApiStartup>();
            // set this in the end to avoid any additional behaviors from being injected into the runtime host.
            // .UseSetting(WebHostDefaults.PreventHostingStartupKey, "1");

            return builder.Build();
        }

        /// <summary>
        /// SharedSettings, overriden by app-specific settings, overriden by commandline, overriden by AEG environment variables.
        /// ASPNETCORE_ environment variables are added by default by WebHostBuilder to the host config, so no way to opt out of that.
        ///
        /// Note that HostBuilder.UseConfiguration() is typically used for host configuration, whereas HostBUilder.ConfigureAppConfiguration() is used for
        /// app-specific configuration; the IConfiguration seen by the app is host config + override-with app-config.
        ///
        /// To reduce the number of moving parts (and enforce a more understandable override order) i'm creating a common IConfigurationRoot for host and app,
        /// especially since we're not hosting multiple "apps" in the same host.
        /// </summary>
        private static IConfigurationRoot BuildConfig(string[] args)
        {
            IConfigurationBuilder configBuilder = new ConfigurationBuilder()
                .AddJsonFile("apiSettings.json", optional: true, reloadOnChange: true);

            if (args != null)
            {
                configBuilder.AddCommandLine(args);
            }

            return configBuilder.Build();
        }
    }
}
