using API.Image;
using Application.Finds;
using Application.Images;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;

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
                return Results.Problem("There was a problem retrieving find");
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
                return Results.Problem("There was a problem retrieving finds");
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
                return Results.Problem("There was a problem retrieving finds");
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
                return Results.Problem("There was a problem retrieving finds");
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
                return Results.Problem("There was a problem retrieving finds");
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
                return Results.Problem("There was a problem submitting the find");
            }
        }

        [HttpPut]
        public async Task<IResult> UpdateFind(
            [FromForm] FindUpdateDto findUpdate,
            IFindData findData
        )
        {
            var userObjectId = Guid.Parse(User.GetObjectId());

            try
            {
                var find = await findData.GetFind(findUpdate.FindId);

                if (find == null)
                {
                    return Results.NotFound();
                }

                if (userObjectId != find.User.ObjectId)
                {
                    return Results.Forbid();
                }
            }
            catch (Exception ex)
            {
                return Results.Problem("There was a problem identifying the find");
            }

            try
            {
                int rowsAffected = await findData.UpdateFind(findUpdate);

                if (rowsAffected == 0)
                {
                    return Results.Problem("There was an issue updating the find");
                }

                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.Problem("There was a problem updating the find");
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

            try
            {
                var find = await findData.GetFind(findId);

                if (find == null)
                {
                    return Results.NotFound();
                }

                if (userObjectId != find.User.ObjectId)
                {
                    return Results.Forbid();
                }
            }
            catch (Exception ex)
            {
                return Results.Problem("There was a problem identifying the find");
            }

            try
            {
                var image = await imageData.GetImage(findId);
                await imageAccessor.DeletePhoto(image.PublicId);
                await findData.DeleteFind(findId);

                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.Problem("There was a problem deleting the find");
            }
        }
    }
}
