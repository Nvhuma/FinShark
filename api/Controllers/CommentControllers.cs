using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/Comment")]
    [ApiController]
    public class CommentControllers : ControllerBase
    { 
        private readonly ICommentRepository _commentRepo;
         public CommentControllers(ICommentRepository commentRepo)
        {
    
           {  
             _commentRepo = commentRepo;
           }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var Comments = await _commentRepo.GetAllAsync();
        
            var CommentDto = Comments.Select(s => s.ToCommentDto());

            return Ok(CommentDto);
        }
        
   }

}        