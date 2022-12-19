using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APICORE.DAL;
using System.Data;
using APICORE.Model;
using System.Data.Common;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APICORE.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class apiController : ControllerBase
    {
        mafia_access da = new mafia_access();
        
        // GET: api/<apiController>
        [HttpGet]
        public string Get()
        {
            string sql = "select * from test";
            DataTable dt = da.GetDataTableByCommand(sql);
            return JsonConvert.SerializeObject(dt);
        }

        // GET api/<apiController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<apiController>
        [HttpPost]
        public string Post([FromBody] test testModel)
        {
            Hashtable ht = new Hashtable();
            ht.Add("name", testModel.name);
            ht.Add("contact_no", testModel.contact_no);
            DataTable dt = da.ExecuteStoredProcedure("test_insert", ht);
            if (dt.Rows.Count > 0)
            {
                return "Done";
            }
            {
                return "Failed";
            }
        }

        // PUT api/<apiController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<apiController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
