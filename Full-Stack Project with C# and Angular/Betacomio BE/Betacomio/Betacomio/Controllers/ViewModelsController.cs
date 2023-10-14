﻿using Betacomio.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Betacomio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViewModelsController : ControllerBase
    {
        private readonly AdventureWorksLt2019Context _context;

        public ViewModelsController(AdventureWorksLt2019Context context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<ViewModel>>> GetModels()
        {
            try
            {
                if (_context.ViewModels == null)
                {
                    return NotFound();
                }
                return await _context.ViewModels.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }

        /// <summary>
        /// TERZO CONTROLLER /// PRELEVA I MODELS IN BASE AL NOME DELLA SOTTOCATEGORIA ( mountain bikes 100 / 200 / 300 / 400 / 500 ) ==> manda a ViewProductsOverviewController
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>

        [Route("modelsName/{name}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ViewModel>>> GetModelsByName(string name)
        {
            try
            {
                if (_context.ViewModels == null)
                {
                    return NotFound();
                }
                return await _context.ViewModels.Where(m => m.Name == name).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }


        [Route("modelsID/{id}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ViewModel>>> GetModelsById(int id)
        {
            try
            {
                if (_context.ViewModels == null)
                {
                    return NotFound();
                }
                return await _context.ViewModels.Where(m => m.ProductCategoryId == id).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }

    }
}
