using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Betacomio.Models;
using Betacomio.Paginators;

namespace Betacomio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly AdventureWorksLt2019Context _context;

        public AddressesController(AdventureWorksLt2019Context context)
        {
            _context = context;
        }

        // GET: api/Addresses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Address>>> GetAddresses()
        {
            try
            {
                if (_context.Addresses == null)
                {
                    return NotFound();
                }
                return await _context.Addresses.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }

        // GET: api/Addresses/5
        [HttpGet("{addressID}")]
        public async Task<ActionResult<Address>> GetAddressByID(int addressID)
        {

            try
            {
                if (_context.Addresses == null)
                {
                    return NotFound();
                }
                var address = await _context.Addresses.FindAsync(addressID);

                if (address == null)
                {
                    return NotFound();
                }

                return address;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }


        }




        // PUT: api/Addresses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<Address>> PutAddress(int id, Address address)
        {
            if (id != address.AddressId)
            {
                return BadRequest();
            }

            _context.Entry(address).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AddressExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return address;
        }

        // POST: api/Addresses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Address>> PostAddress(Address address)
        {

            try
            {
                if (_context.Addresses == null)
                {
                    return Problem("Entity set 'AdventureWorksLt2019Context.Addresses'  is null.");
                }
                _context.Addresses.Add(address);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }



            return address;
        }

        // DELETE: api/Addresses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddress(int id)
        {

            try
            {
                if (_context.Addresses == null)
                {
                    return NotFound();
                }
                var address = await _context.Addresses.FindAsync(id);
                if (address == null)
                {
                    return NotFound();
                }

                _context.Addresses.Remove(address);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            return NoContent();
        }

        private bool AddressExists(int id)
        {
            return (_context.Addresses?.Any(e => e.AddressId == id)).GetValueOrDefault();
        }
    }
}
