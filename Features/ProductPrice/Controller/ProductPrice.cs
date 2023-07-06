using ActivityLog.Business.ProductPrice;
using FNBLibrary.Attributes;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ActivityLog.Features.ProductPrice.Controller
{
    [Route("product-price")]
    [ApiController]
    public class ProductPrice : ControllerBase
    {
        private readonly IProductPriceBusiness _productPriceBusiness;

        public ProductPrice(IProductPriceBusiness productPriceBusiness)
        {
            _productPriceBusiness = productPriceBusiness;
        }

        [HttpGet]
        [AuthorizeHeaderKey]
        public async Task<ActionResult> GetListAsync(
            [FromHeader(Name = "x-api-key")] string apiKey)
        {
            DateTime startDate = DateTime.UtcNow.AddHours(7).Date;
            DateTime endDate = DateTime.UtcNow.AddDays(1).AddHours(7).Date;

            string siteCode = "";
            string sku = "";
            int size = 5;
            int page = 1;

            if (HttpContext.Request.QueryString.HasValue)
            {
                if (HttpContext.Request.Query.ContainsKey("startDate"))
                    DateTime.TryParse(HttpContext.Request.Query["startDate"].ToString(), out startDate);

                if (HttpContext.Request.Query.ContainsKey("endDate"))
                    DateTime.TryParse(HttpContext.Request.Query["endDate"].ToString(), out endDate);

                if (HttpContext.Request.Query.ContainsKey("siteCode"))
                    siteCode = HttpContext.Request.Query["siteCode"].ToString();

                if (HttpContext.Request.Query.ContainsKey("sku"))
                    sku = HttpContext.Request.Query["sku"].ToString();

                if (HttpContext.Request.Query.ContainsKey("page"))
                    int.TryParse(HttpContext.Request.Query["page"].ToString(), out page);

                if (HttpContext.Request.Query.ContainsKey("pageSize"))
                    int.TryParse(HttpContext.Request.Query["pageSize"].ToString(), out size);
            }

            var param = new Dto.ProductPrice.ProductPriceParam
            {
                StartDate = startDate,
                EndDate = endDate,
                SiteCode = siteCode,
                Sku = sku,
                Page = page,
                Size = size
            };

            var result = await _productPriceBusiness.GetListAsync(param);
            var response = JsonConvert.SerializeObject(result);

            return Ok(response);
        }
    }
}
