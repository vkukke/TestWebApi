// -----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Api;
using Microsoft.AspNetCore.Hosting;

namespace Host.Console
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            IWebHost apiHost = ApiHostBuilder.Build();
            apiHost.Start();
            apiHost.WaitForShutdown();
        }
    }
}
