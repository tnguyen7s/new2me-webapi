using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using new2me_api.Data.Query;
using new2me_api.Dtos;
using new2me_api.Models;

namespace new2me_api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IQuery query;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor contextAccessor;
        public PostController(IQuery query, IMapper mapper, IHttpContextAccessor contextAccessor){
            this.contextAccessor = contextAccessor;
            this.mapper = mapper;
            this.query = query;
        }

        // GET api/post
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<PostDto>>> GetActivePosts(){
            // query for Posts
            var result = await this.query.GetActivePosts();
            
            // map posts to PostDto
            var postDtos = mapper.Map<IEnumerable<PostDtoWithoutContact>>(result);

            // Extract actual pictures and save them to PostDto
            for (int i=0; i<result.Count(); i++){
                var pictures = result.ElementAt(i).PostPictures;
                var postDto = postDtos.ElementAt(i);

                postDto.Pictures = new List<string>();
                foreach (PostPicture pic in pictures){
                    postDto.Pictures.Add(Encoding.UTF32.GetString(pic.Picture));
                }
            }

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
        [AllowAnonymous]
        public async Task<ActionResult<PostDtoWithoutContact>> GetPost(int id){
            var result = await this.query.GetPost(id);
            
            if (result==null){
                return NotFound();
            }

            var postDto = this.mapper.Map<PostDtoWithoutContact>(result); 
            postDto.Pictures = new List<string>();
            foreach(PostPicture postPicture in result.PostPictures){
                postDto.Pictures.Add(Encoding.UTF32.GetString(postPicture.Picture));
            }
            
            return Ok(postDto);
        }

        // POST api/post
        [HttpPost]
        public async Task<ActionResult<PostDto>> CreatePost(PostDto postDto){
            var userId = int.Parse(this.contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var post = this.mapper.Map<Post>(postDto);

            var result = await this.query.CreatePost(post, postDto.Pictures, userId);

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
            var userId = int.Parse(this.contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            await this.query.UpdatePost(postFromDb, postDto.Pictures, userId);

            return NoContent();
        }

    }
}