using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NRestfulSampleWebApi.Models;

namespace NRestfulSampleWebApi.Controllers {
    public class SampleController : ApiController {
        // GET api/<controller>
        public IEnumerable<string> Get() {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public User Get(int id) {
            return new User {
                Email = $"account{id}@test.com",
                FirstName = "Joe",
                LastName = "Test"
            };
        }

        // POST api/<controller>
        public void Post([FromBody]User value) {

        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value) {
        }

        // DELETE api/<controller>/5
        public void Delete(int id) {

        }
    }
}