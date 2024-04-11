using Andyy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Serilog;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Andyy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AndyyController : ControllerBase
    {
        private readonly AndyyContext _andyyContext;
        private readonly ILogger<AndyyController> _logger;
        private readonly IMemoryCache memoryCache;

        public AndyyController(AndyyContext aContext, IMemoryCache _memoryCache, ILogger<AndyyController> logger)
        {
            _andyyContext = aContext;
            _logger = logger;
            memoryCache = _memoryCache;
        }

        // GET: api/<AndyyController>
        [HttpGet]
        public async Task<IEnumerable<WorkSheet>> Get()
        {
            var isAvailable = memoryCache.TryGetValue("WorkSheet", out IEnumerable<WorkSheet> cachedWorkSheets);
            if (!isAvailable)
            {
                var allWorkSheets = await _andyyContext.WorkSheet.ToListAsync();
                memoryCache.Set("WorkSheet", allWorkSheets);
                return allWorkSheets;
            }
            else
            {
                return cachedWorkSheets;
            }
        }

        // GET api/<AndyyController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (id < 1)
            {
                return BadRequest();
            }           
            var product = await _andyyContext.WorkSheet.FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)                
                return NotFound();
            return Ok(product);
        }

        // POST api/<AndyyController>
        [HttpPost]
        public async Task<ActionResult<WorkSheet>> PostWorkSheet(WorkSheet value)
        {
            _andyyContext.WorkSheet.Add(value);
            _andyyContext.SaveChanges();
            return CreatedAtAction(nameof(Put), new { id = value.Id }, value);

        }

        // PUT api/<AndyyController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] WorkSheet value)
        {
            if (id != value.Id)
            {
                return BadRequest();
            }

            _andyyContext.Entry(value).State = EntityState.Modified;

            try
            {
                _andyyContext.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (!_andyyContext.WorkSheet.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode(500, "存取發生錯誤");
                }
            }
            return NoContent();
        }

        // DELETE api/<AndyyController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var delete = _andyyContext.WorkSheet.Find(id);
            if (delete == null)
            {
                return NotFound();
            }
            _andyyContext.WorkSheet.Remove(delete);
            _andyyContext.SaveChanges();
            return NoContent();
        }
    }
}
