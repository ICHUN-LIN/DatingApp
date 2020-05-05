using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DatingApp.api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace DatingApp.API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {//control base :  without view support
        private readonly DataContext _dataContext;

        public ValuesController(DataContext dataContext)
        {
            this._dataContext = dataContext;

        }

        // Get data by async => improve speed
        // GET api/values
        [HttpGet]
        [Authorize(Roles="Members, Admin")] //Only Roles is member can do that
        public async Task<IActionResult> Get()
        {
            var values = await _dataContext.Values.ToListAsync();
            return Ok(values);
        }

        // GET api/values/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var value = await _dataContext.Values.FirstOrDefaultAsync(x=>x.id == id);
            return Ok(value);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {

        }
    }
}