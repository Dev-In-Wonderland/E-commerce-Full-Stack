using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Betacomio.Models;
using Betacomio.Models.CustomModels;
using System.Text;

namespace Betacomio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly UsersBetacomioContext _context;

        public CartsController(UsersBetacomioContext context)
        {
            _context = context;
        }

        // GET: api/Carts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cart>>> GetCarts()
        {
            try
            {
                if (_context.Carts == null)
                {
                    return NotFound();
                }
                return await _context.Carts.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }

        // GET: api/Carts/5
        [HttpGet("{userID}")]
        public async Task<ActionResult<Cart>> GetCart(int userID)
        {
            try
            {
                if (_context.Carts == null)
                {
                    return NotFound();
                }

                Cart? cart = null;

                if (_context.Carts.Where(c => c.UserId == userID).Any())
                {
                    cart = await _context.Carts.Where(c => c.UserId == userID).FirstAsync();
                }

                if (cart == null)
                {
                    return NotFound(cart);
                }

                return cart;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }


        }

        // PUT: api/Carts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCart(int id, Cart cart)
        {
            if (id != cart.CartId)
            {
                return BadRequest();
            }

            _context.Entry(cart).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Carts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Cart>> PostCart(CustomCart customCart)
        {
            try
            {
                if (_context.Carts == null)
                {
                    return Problem("Entity set 'UsersBetacomioContext.Carts'  is null.");
                }

                byte[] byteCart = Convert.FromBase64String(customCart.cartItems);

                if (_context.Carts.Where(i => i.UserId == customCart.userID).Any())
                {
                    Cart cart = new Cart
                    {
                        UserId = customCart.userID,
                        CartItems = byteCart,
                        CartId = _context.Carts.Where(c => c.UserId == customCart.userID).Select(c => c.CartId).First()
                    };

                    await PutCart(cart.CartId, cart);
                    return Ok(cart);
                }
                else
                {
                    Cart cart = new Cart
                    {
                        UserId = customCart.userID,
                        CartItems = byteCart
                    };

                    _context.Carts.Add(cart);
                    await _context.SaveChangesAsync();

                    return cart;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        // DELETE: api/Carts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCart(int id)
        {

            try
            {
                if (_context.Carts == null)
                {
                    return NotFound();
                }
                var cart = await _context.Carts.Where(u => u.UserId == id).FirstOrDefaultAsync();
                if (cart == null)
                {
                    return NotFound();
                }

                _context.Carts.Remove(cart);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private bool CartExists(int id)
        {
            return (_context.Carts?.Any(e => e.CartId == id)).GetValueOrDefault();
        }
    }
}
