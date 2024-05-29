using API.Image;
using Application.Finds;
using Application.Images;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FindsController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet("id/{id}")]
        public async Task<IResult> GetFind(Guid id, IFindData findData)
        {
            try
            {
                var results = await findData.GetFind(id);

                if (results == null)
                    return Results.NotFound();

                return Results.Ok(results);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("search/{term}")]
        public async Task<IResult> GetFindsWithTerm(string term, IFindData findData)
        {
            try
            {
                var results = await findData.GetFindsWithTerm(term);

                if (results.IsNullOrEmpty())
                    return Results.NotFound();

                return Results.Ok(results);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        [AllowAnonymous]
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
            [FromForm] FindCreateDto find,
            IFindData findData,
            IImageData imageData,
            IImageAccessor imageAccessor
        )
        {
            try
            {
                Guid newFindId = Guid.NewGuid();
                find.FindId = newFindId;
                find.AuthorObjectId = Guid.Parse(User.GetObjectId());
                find.DateCreated = DateTime.UtcNow;
                await findData.InsertFind(find);

                //Upload image to Cloudinary
                var uploadResults = await imageAccessor.AddPhoto(find.ImageFile);
                var image = new Domain.Image
                {
                    FindId = newFindId,
                    PublicId = uploadResults.PublicId,
                    Url = uploadResults.SecureUrl.ToString()
                };

                await imageData.InsertImage(image);

                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IResult> UpdateFind([FromForm] FindUpdateDto find, IFindData findData)
        {
            var userObjectId = Guid.Parse(User.GetObjectId());

            if (userObjectId != find.AuthorObjectId)
            {
                return Results.Problem("Not authorized to edit.");
            }

            try
            {
                int rowsAffected = await findData.UpdateFind(find);

                if (rowsAffected == 0)
                {
                    return Results.Problem("There was an issue updating the find");
                }

                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        [HttpDelete("delete/id/{findId}")]
        public async Task<IResult> DeleteFind(
            Guid findId,
            IFindData findData,
            IImageData imageData,
            IImageAccessor imageAccessor
        )
        {
            var userObjectId = Guid.Parse(User.GetObjectId());
            var find = await findData.GetFind(findId);

            if (userObjectId != find.User.ObjectId)
            {
                return Results.Problem("Not authorized to edit.");
            }

            try
            {
                await findData.DeleteFind(findId);
                var image = await imageData.GetImage(findId);
                await imageAccessor.DeletePhoto(image.PublicId);

                return Results.Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Debug.WriteLine(ex.Message);
                return Results.Problem(ex.Message);
            }
        }
    }
}
