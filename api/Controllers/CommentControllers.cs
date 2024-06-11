using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/Comment")]
    [ApiController]
    public class CommentControllers : ControllerBase
    { 
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;
         public CommentControllers(ICommentRepository commentRepo, IStockRepository stockRepo)
        {
    
           {  
             _commentRepo = commentRepo;
             _stockRepo = stockRepo;
           }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if(!ModelState.IsValid)
            return BadRequest(ModelState);

            var Comments = await _commentRepo.GetAllAsync();
        
            var CommentDto = Comments.Select(s => s.ToCommentDto());

            return Ok(CommentDto);
        }

        
         [HttpGet("{id:int}")]
         public  async Task<IActionResult> getbyId([FromRoute] int id) 
       {
             if(!ModelState.IsValid)
            return BadRequest(ModelState);

        var comment =  await _commentRepo.GetByIdAysnc(id);

        if(comment == null)
        {
            return NotFound();
        }

        return Ok(comment.ToCommentDto());
       }

       [HttpPost("{stockId:int}")]
       public async Task<IActionResult> create([FromRoute] int stockId, CreateCommentDto commentDto)
       { 
               if(!ModelState.IsValid)
            return BadRequest(ModelState);

            if(!await _stockRepo.StockExist(stockId))
            {
                return BadRequest("stock does not exist");
            }
            var commentModel = commentDto.ToCommentFromCreate(stockId);
            await _commentRepo.CreateAysnc(commentModel);
            return CreatedAtAction(nameof(getbyId), new { id = commentModel.Id}, commentModel.ToCommentDto());

       }
      [HttpPut]
      [Route("{id:int}")]
      public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentRequestDto UpdateDto)
      { 
           if(!ModelState.IsValid)
            return BadRequest(ModelState);

        var comment = await _commentRepo.UpdateAsync(id, UpdateDto.ToCommentFromUpdate());

        if(comment == null)
        {
            return NotFound("comment not found");
        }

        return Ok(comment.ToCommentDto());
     
      }
      [HttpDelete]
      [Route("{id:int}")]
       public async Task<IActionResult> Delete([FromRoute] int id)
       {   
           if(!ModelState.IsValid)
            return BadRequest(ModelState);

          var commentModel = await _commentRepo.DeleteAsync(id);

          if(commentModel == null)
          {
            return NotFound("Comment does not exist");
          }

          return Ok(commentModel);
       }
   }

   

}        