using ActivityLog.Business.SoldOut;
using FNBLibrary.Attributes;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ActivityLog.Features.SoldOut.Controller
{
    [Route("sold-out")]
    [ApiController]
    public class SoldOut : ControllerBase
    {
        private readonly ISoldOutBusiness _soldOutBusiness;

        public SoldOut(ISoldOutBusiness soldOutBusiness)
        {
            _soldOutBusiness = soldOutBusiness;
        }

        [HttpGet]
        [AuthorizeHeaderKey]
        public async Task<ActionResult> GetListAsync(
            [FromHeader(Name = "x-api-key")] string apiKey)
        {
            DateTime startDate = DateTime.UtcNow.AddHours(7).Date;
            DateTime endDate = DateTime.UtcNow.AddDays(1).AddHours(7).Date;

            string search = "";
            int size = 5;
            int page = 1;
            bool isGlobal;
            bool? isGlobalParam = null;

            if (HttpContext.Request.QueryString.HasValue)
            {
                if (HttpContext.Request.Query.ContainsKey("startDate"))
                    DateTime.TryParse(HttpContext.Request.Query["startDate"].ToString(), out startDate);

                if (HttpContext.Request.Query.ContainsKey("endDate"))
                    DateTime.TryParse(HttpContext.Request.Query["endDate"].ToString(), out endDate);

                if (HttpContext.Request.Query.ContainsKey("search"))
                    search = HttpContext.Request.Query["search"].ToString();

                if (HttpContext.Request.Query.ContainsKey("page"))
                    int.TryParse(HttpContext.Request.Query["page"].ToString(), out page);

                if (HttpContext.Request.Query.ContainsKey("size"))
                    int.TryParse(HttpContext.Request.Query["size"].ToString(), out size);

                if (HttpContext.Request.Query.ContainsKey("isGlobal"))
                {
                    bool.TryParse(HttpContext.Request.Query["isGlobal"].ToString(), out isGlobal);
                    isGlobalParam = isGlobal;
                }
            }

            var param = new Dto.SoldOut.SoldOutParam
            {
                StartDate = startDate,
                EndDate = endDate,
                Search = search,
                Page = page,
                Size = size,
                IsGlobal = isGlobalParam,
            };

            var result = await _soldOutBusiness.GetListAsync(param);
            var response = JsonConvert.SerializeObject(result);

            return Ok(response);
        }
    }
}
