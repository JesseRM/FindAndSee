using Application.Finds;
using Application.Likes;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LikesController : ControllerBase
    {
        [HttpGet("liked/{findId}")]
        public async Task<IResult> IsLiked(Guid findId, ILikeData likeData)
        {
            try
            {
                var userObjectId = Guid.Parse(User.GetObjectId());
                var result = await likeData.GetLike(userObjectId, findId);

                if (result == null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return Results.Problem("There was a problem processing your request");
            }
        }

        [HttpPost]
        public async Task<IResult> InsertLike(Like like, ILikeData likeData, IFindData findData)
        {
            try
            {
                var userObjectId = Guid.Parse(User.GetObjectId());
                var result = await findData.GetFind(like.FindId);

                if (result == null)
                {
                    return Results.BadRequest();
                }

                await likeData.InsertLike(userObjectId, like.FindId);

                return Results.Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return Results.Problem("There was a problem processing your request");
            }
        }

        [HttpDelete("delete/{findId}")]
        public async Task<IResult> DeleteLike(Guid findId, ILikeData likeData)
        {
            try
            {
                var userObjectId = Guid.Parse(User.GetObjectId());

                await likeData.DeleteLike(userObjectId, findId);

                return Results.Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return Results.Problem("There was a problem processing your request");
            }
        }
    }
}
