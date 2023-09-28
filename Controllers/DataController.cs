using Middleware.API.Real.Auth;
using Middleware.API.Real.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;


namespace Middleware.API.Real.Controllers
{
    [BasicAuthentication]
    public class DataController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetData([FromBody] DataModel data)
        {
            try
            {
                string SC_VALUE = "SC_20230928-0001";
                return Ok($"Amostra criada com sucesso. {data.ID}|{SC_VALUE}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Endpoint para enviar dados via POST (fictício)
        [HttpPost]
        public IHttpActionResult SendData([FromBody] DataModel data)
        {
            data = new DataModel();
            data.Number = 1;
            data.ID = 1;
            return Ok(data);
        }
    }
}