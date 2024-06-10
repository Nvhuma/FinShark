using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public StockController(ApplicationDBContext context)
        {
            _context  = context;
        }
       [HttpGet]
       public IActionResult GetAll()
       {
        var Stocks = _context.Stocks.ToList()
        .Select(s => s.ToStockDto());

        return Ok(Stocks);
       }

       [HttpGet("{id}")]
       public IActionResult getbyId([FromRoute] int id) 
       {
        var stock = _context.Stocks.Find(id);

        if(stock == null)
        {
            return NotFound();
        }

        return Ok(stock.ToStockDto());
       }

       [HttpPost]
       public IActionResult Create([FromBody] CreateStockRequestDto stockDtos)
       {
            var stockModel = stockDtos.ToStockFromCreateDTO();
            _context.Stocks.Add(stockModel);
            _context.SaveChanges();
            return CreatedAtAction(nameof(getbyId), new { id = stockModel.Id}, stockModel.ToStockDto());
       }
       [HttpPost]
       [Route("id")]
       public IActionResult Update([FromRoute] int id, [FromBody] UpdateStockRequestDto UpdateDto)
       {
         var stockModel = _context.Stocks.FirstOrDefault( x => x.Id == id);
         if (stockModel == null)
         {
            return NotFound();
         }

         stockModel.Symbol = UpdateDto.Symbol;
         stockModel.CompanyName = UpdateDto.CompanyName;
         stockModel.Purchase = UpdateDto.Purchase;
         stockModel.LastDiv = UpdateDto.LastDiv;
         stockModel.Industry = UpdateDto.Industry;
         stockModel.MarketCap = UpdateDto.MarketCap;
       }
    }
}