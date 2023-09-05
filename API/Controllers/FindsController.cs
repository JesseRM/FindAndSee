using API.Image;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using Persistence.Data;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FindsController : ControllerBase
    {
        [HttpGet("id/{id}")]
        public async Task<IResult> GetFind(Guid findId, IFindData findData)
        {
            try
            {
                var results = await findData.GetFind(findId);

                if (results.Title == null)
                    return Results.NotFound();

                return Results.Ok(results);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        [HttpGet("search/{term}")]
        public async Task<IResult> GetFindsWithTerm(string term, IFindData findData)
        {
            try
            {
                var results = await findData.GetFindsWithTerm(term);

                return Results.Ok(results);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        [HttpGet("recent")]
        public async Task<IResult> GetRecentFinds(IFindData findData)
        {
            try
            {
                return Results.Ok(await findData.GetRecentFinds());
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        [HttpGet("liked")]
        public async Task<IResult> GetLikedFinds(IFindData findData)
        {
            try
            {
                var userObjectId = Guid.Parse(User.GetObjectId());
                var results = await findData.GetLikedFinds(userObjectId);

                return Results.Ok(results);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        [HttpGet("user")]
        public async Task<IResult> GetUserFinds(IFindData findData)
        {
            try
            {
                var userObjectId = Guid.Parse(User.GetObjectId());
                var results = await findData.GetUserFinds(userObjectId);

                return Results.Ok(results);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IResult> InsertFind(
            Find find,
            IFindData findData,
            ImageAccessor imageAccessor
        )
        {
            try
            {
                //Upload image to Cloudinary
                var uploadResults = await imageAccessor.AddPhoto(find.ImageFile);
                find.ImageUrl = uploadResults.SecureUrl.ToString();
                find.ImagePubicId = uploadResults.PublicId;
                find.AuthorObjectId = Guid.Parse(User.GetObjectId());

                await findData.InsertFind(find);

                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IResult> UpdateFind(Find find, IFindData findData)
        {
            try
            {
                await findData.UpdateFind(find);

                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        [HttpDelete("delete/id/{id}")]
        public async Task<IResult> DeleteFind(Guid id, IFindData findData)
        {
            try
            {
                await findData.DeleteFind(id);

                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }
    }
}
