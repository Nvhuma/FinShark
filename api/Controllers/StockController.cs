using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IStockRepository _stockRepo;
        public StockController(ApplicationDBContext context, IStockRepository stockRepo)
        {
            _stockRepo = stockRepo;
            _context  = context;
        }
       [HttpGet]
       public async  Task<IActionResult> GetAll()
       {
        var Stocks = await _stockRepo.GetAllAysnc();
        var stockDto = Stocks.Select(s => s.ToStockDto());

        return Ok(Stocks);
       }

       [HttpGet("{id}")]
       public  async Task<IActionResult> getbyId([FromRoute] int id) 
       {
        var stock =  await _stockRepo.GetByIdAysnc(id);

        if(stock == null)
        {
            return NotFound();
        }

        return Ok(stock.ToStockDto());
       }

       [HttpPost]
       public  async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDtos)
       {
            var stockModel = stockDtos.ToStockFromCreateDTO();
              
              await _stockRepo.CreateAysnc(stockModel);

            return CreatedAtAction(nameof(getbyId), new { id = stockModel.Id}, stockModel.ToStockDto());
       }
       [HttpPut]
       [Route("{id}")]
       public  async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto UpdateDto)
       {
         var stockModel =  await _stockRepo.UpdateAsync(id, UpdateDto);
         if (stockModel == null)
         {
            return NotFound();
         }


         return Ok(stockModel.ToStockDto());
       }
       [HttpDelete]
       [Route("{id}")]
       public async Task<IActionResult> Delete([FromRoute] int id )
       {
        var stockModel = await _stockRepo.DeleteAsync(id);
        if(stockModel == null)
        {
            return NotFound();
        }
         

           return NoContent();
       }
      
    }
}