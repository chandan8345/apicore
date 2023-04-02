using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Mafia.DAL;
using System.Collections;
using Mafia.Models;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using AspNetCore.Reporting;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mafia.Controllers
{
    [Microsoft.AspNetCore.Cors.EnableCors("CorsApi")]
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        data_access da = new data_access();
        private readonly IWebHostEnvironment webHostEnvironment;

        public TestController(IWebHostEnvironment webHostEnvironment) 
        {
            this.webHostEnvironment = webHostEnvironment;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }

        // GET: api/<TestController>
        [HttpGet]
        public string Get()
        {
            string sql = "select a.*,b.name as desig from employee as a left join designation b on a.designation=b.id";
            DataTable dt = da.GetDataTableByCommand(sql);
            return JsonConvert.SerializeObject(dt);
        }

        [HttpGet]
        [Route("Designation")]
        public string Designation()
        {
            string sql = "select * from designation";
            DataTable dt = da.GetDataTableByCommand(sql);
            return JsonConvert.SerializeObject(dt);
        }

        // GET api/<apiController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            string sql = "select * from employee where id='" + id + "'";
            DataTable dt = da.GetDataTableByCommand(sql);
            return JsonConvert.SerializeObject(dt);
        }

        [HttpGet("{id}")]
        [Route("prPrint")]
        public IActionResult prPrint(int id)
        {
            Hashtable ht = new Hashtable();
            ht.Add("id", id);
            string sql = "select * from employee";
            DataTable dt = da.GetDataTableByCommand(sql);

            var path = $"{this.webHostEnvironment.WebRootPath}\\Report1.rdlc";
            Dictionary<string, string> p = new Dictionary<string, string>();
            LocalReport localReport = new LocalReport(path);
            
            localReport.AddDataSource("employee", dt);

            var res = localReport.Execute(RenderType.Pdf,1,p,"");

            return File(res.MainStream, "application/pdf");
        }

        // POST api/<apiController>
        [HttpPost]
        [Route("employeeInsert")]
        public string Post([FromBody] Empolyee empolyee)
        {
            Hashtable ht = new Hashtable();
            ht.Add("name", empolyee.name);
            ht.Add("email", empolyee.email);
            ht.Add("age", empolyee.age);
            ht.Add("designation", empolyee.designation);
            DataTable dt = da.ExecuteStoredProcedure("insert_empoloyee", ht);
            return JsonConvert.SerializeObject(dt);
        }

        // PUT api/<apiController>/5
        [HttpPut("{id}")]
        public string Put(int id, [FromBody] Empolyee empolyee)
        {
            Hashtable ht = new Hashtable();
            ht.Add("id", id);
            ht.Add("name", empolyee.name);
            ht.Add("email", empolyee.email);
            ht.Add("age", empolyee.age);
            ht.Add("designation", empolyee.designation);
            DataTable dt = da.ExecuteStoredProcedure("update_empoloyee", ht);
            return JsonConvert.SerializeObject(dt);
        }

        // DELETE api/<apiController>/5
        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            string sql = "delete from employee where id='" + id + "'";
            da.ExecuteScalar(sql);
            DataTable dt = null;
            return JsonConvert.SerializeObject(dt);
        }

    }
}
