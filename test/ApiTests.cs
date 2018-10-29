// -----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// -----------------------------------------------------------------------

using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Api.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Api.Tests
{
    [TestClass]
    public class ApiTests
    {
        private static IWebHost apiHost;
        private readonly string productsBaseUri = "http://127.0.0.1:5889/products";
        private readonly string apiVersion = "api-version=2019-01-01-preview";

        [ClassInitialize]
        public static async Task SetupAsync(TestContext context)
        {
            apiHost = ApiHostBuilder.Build();
            await apiHost.StartAsync();
        }

        [ClassCleanup]
        public static async Task CleanupAsync()
        {
            await apiHost.StopAsync();
        }

        [TestMethod]
        public async Task CanCreateProduct()
        {
            ProductInfo productInfo = new ProductInfo();
            productInfo.ProductId = 1;
            productInfo.ProductDescription = "Test descr";

            HttpClient httpClient = new HttpClient();
            var result = await httpClient.PostAsJsonAsync<ProductInfo>(productsBaseUri, productInfo);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }
    }
}
