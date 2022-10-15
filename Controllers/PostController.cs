using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using new2me_api.Data.Query;
using new2me_api.Dtos;
using new2me_api.Models;

namespace new2me_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IQuery query;
        private readonly IMapper mapper;
        public PostController(IQuery query, IMapper mapper){
            this.mapper = mapper;
            this.query = query;
        }

        // GET api/post
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostDto>>> GetActivePosts(){
            var result = await this.query.GetActivePosts();

            var postDtos = mapper.Map<IEnumerable<PostDto>>(result);

            // var postDtos = from p in result
            //     select new PostDto{
            //         Id = p.Id,
            //         Title = p.Title,
            //         Location = p.Location,
            //         Condition = p.Condition,
            //         Description = p.Description,
            //         Tag = p.Tag,
            //         ContactEmail = p.ContactEmail,
            //         ContactPhone = p.ContactPhone,
            //         Status = p.Status
            //     };
            
            return Ok(postDtos);
        }

        // GET api/post/:id
        [HttpGet("{id}")]
        public async Task<ActionResult<PostDto>> GetPost(int id){
            var result = await this.query.GetPost(id);
            
            if (result==null){
                return NotFound();
            }

            var postDto = this.mapper.Map<PostDto>(result); 

            return Ok(postDto);
        }

        // POST api/post
        [HttpPost]
        public async Task<ActionResult<PostDto>> CreatePost(PostDto postDto){
            throw new UnauthorizedAccessException();
            var post = this.mapper.Map<Post>(postDto);

            var result = await this.query.CreatePost(post);

            postDto.Id = result.Id;
            return CreatedAtAction(nameof(CreatePost), new {id=postDto.Id}, postDto);
        }


        // DELETE api/post/:id
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePost(int id){
            var result = await this.query.GetPost(id);

            if (result==null){
                return NotFound();
            }

            await this.query.DeletePost(result);
            return NoContent();
        }

        // PUT api/post/:id
        [HttpPut("{id}")]
        public async Task<ActionResult<PostDto>> UpdatePost(int id, PostDto postDto){
            if (id!=postDto.Id){
                return BadRequest();
            }

            var postFromDb = await this.query.GetPost(id);
            if (postFromDb==null){
                return NotFound();
            }

            mapper.Map(postDto, postFromDb);
            await this.query.UpdatePost(postFromDb);

            return NoContent();
        }
    }
}