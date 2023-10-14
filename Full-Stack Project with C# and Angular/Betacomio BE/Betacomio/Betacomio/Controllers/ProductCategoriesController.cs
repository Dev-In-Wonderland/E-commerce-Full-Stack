using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Betacomio.Models;

namespace Betacomio.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoriesController : ControllerBase
    {
        private readonly AdventureWorksLt2019Context _context;

        public ProductCategoriesController(AdventureWorksLt2019Context context)
        {
            _context = context;
        }

        // GET: api/ProductCategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductCategory>>> GetProductCategories()
        {
            try
            {
                if (_context.ProductCategories == null)
                {
                    return NotFound();
                }
                return await _context.ProductCategories.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }


        /// <summary>
        /// SECONDO CONTROLLER /// PRELEVA LE CATEGORIE IN BASE A PARENT CATEGORIES ( sottocategorie di bikes, accessories...)
        /// </summary>

        // GET: api/ProductCategories/5
        [HttpGet("{ParentProductCategory}")]
        public async Task<ActionResult<IEnumerable<ProductCategory>>> GetByParentPC(int ParentProductCategory)
        {
            try
            {
                if (_context.ProductCategories == null)
                {
                    return NotFound();
                }
                return await _context.ProductCategories.Where(e => e.ParentProductCategoryId == ParentProductCategory).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }

        // PUT: api/ProductCategories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductCategory(int id, ProductCategory productCategory)
        {
            if (id != productCategory.ProductCategoryId)
            {
                return BadRequest();
            }

            _context.Entry(productCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductCategoryExists(id))
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

        // POST: api/ProductCategories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductCategory>> PostProductCategory(ProductCategory productCategory)
        {
            try
            {
                if (_context.ProductCategories == null)
                {
                    return Problem("Entity set 'AdventureWorksLt2019Context.ProductCategories'  is null.");
                }
                _context.ProductCategories.Add(productCategory);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetProductCategory", new { id = productCategory.ProductCategoryId }, productCategory);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }

        // DELETE: api/ProductCategories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductCategory(int id)
        {
            try
            {
                if (_context.ProductCategories == null)
                {
                    return NotFound();
                }
                var productCategory = await _context.ProductCategories.FindAsync(id);
                if (productCategory == null)
                {
                    return NotFound();
                }

                _context.ProductCategories.Remove(productCategory);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }

        private bool ProductCategoryExists(int id)
        {
            return (_context.ProductCategories?.Any(e => e.ProductCategoryId == id)).GetValueOrDefault();
        }
    }
}
