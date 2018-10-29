// -----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// -----------------------------------------------------------------------


using Api.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        [Route("")]
        [HttpPost]
        public Task<ActionResult<ProductInfo>> CreateProductAsync(ProductInfo productInfo)
        {
            return Task.FromResult<ActionResult<ProductInfo>>(new OkObjectResult(productInfo));
        }

        [Route("")]
        [Route("{productId}")]
        [HttpPut]
        public Task<ActionResult<ProductInfo>> UpdateProductAsync(string productName, ProductInfo productInfo)
        {
            return Task.FromResult<ActionResult<ProductInfo>>(new OkObjectResult(productInfo));
        }

        [Route("")]
        [HttpGet]
        public Task<ActionResult<IList<ProductInfo>>> GetProductsAsync()
        {
            IList<ProductInfo> products = new List<ProductInfo>();
            return Task.FromResult(new ActionResult<IList<ProductInfo>>(products));
        }

        [Route("{productId}")]
        [HttpGet]
        public Task<ActionResult<ProductInfo>> GetProductAsync(string productId)
        {
            return Task.FromResult<ActionResult<ProductInfo>>(new OkObjectResult(new ProductInfo()));
        }

        [Route("")]
        [Route("{productId}")]
        [HttpDelete]
        public Task<ActionResult> DeleteProductAsync(string productId)
        {
            return Task.FromResult<ActionResult>(new OkResult());
        }
    }
}
