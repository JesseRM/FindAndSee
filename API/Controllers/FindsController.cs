using API.Image;
using Application.Core;
using Application.Finds;
using Application.Images;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;

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
                var find = await findData.GetFind(id);

                if (find == null)
                    return Results.NotFound();

                return Results.Ok(find);
            }
            catch (Exception ex)
            {
                return Results.Problem("There was a problem retrieving find");
            }
        }

        [AllowAnonymous]
        [HttpGet("search/{term}")]
        public async Task<IResult> GetFindsWithTerm(string term, IFindData findData, IMapper mapper)
        {
            try
            {
                var finds = await findData.GetFindsWithTerm(term);

                if (!finds.Any())
                    return Results.NotFound();

                var findBasicDtos = mapper.FindToFindBasicDto(finds);

                return Results.Ok(findBasicDtos);
            }
            catch (Exception ex)
            {
                return Results.Problem("There was a problem retrieving finds");
            }
        }

        [AllowAnonymous]
        [HttpGet("recent")]
        public async Task<IResult> GetRecentFinds(IFindData findData, IMapper mapper)
        {
            try
            {
                var finds = await findData.GetRecentFinds();
                var findBasicDtos = mapper.FindToFindBasicDto(finds);

                return Results.Ok(findBasicDtos);
            }
            catch (Exception ex)
            {
                return Results.Problem("There was a problem retrieving finds");
            }
        }

        [HttpGet("liked")]
        public async Task<IResult> GetLikedFinds(IFindData findData, IMapper mapper)
        {
            try
            {
                var userObjectId = Guid.Parse(User.GetObjectId());
                var finds = await findData.GetLikedFinds(userObjectId);
                var findBasicDtos = mapper.FindToFindBasicDto(finds);

                return Results.Ok(findBasicDtos);
            }
            catch (Exception ex)
            {
                return Results.Problem("There was a problem retrieving finds");
            }
        }

        [HttpGet("user")]
        public async Task<IResult> GetUserFinds(IFindData findData, IMapper mapper)
        {
            try
            {
                var userObjectId = Guid.Parse(User.GetObjectId());
                var finds = await findData.GetUserFinds(userObjectId);
                var findBasicDtos = mapper.FindToFindBasicDto(finds);

                return Results.Ok(findBasicDtos);
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

                var cloudinaryUploadResult = await imageAccessor.AddPhoto(find.ImageFile);
                if (cloudinaryUploadResult.Error != null)
                {
                    return Results.Problem("There was a problem saving the image");
                }

                int findRowsAffected = await findData.InsertFind(find);
                if (findRowsAffected != 1)
                {
                    return Results.Problem("There was a problem saving the find data");
                }

                var image = new Domain.Image
                {
                    FindId = newFindId,
                    PublicId = cloudinaryUploadResult.PublicId,
                    Url = cloudinaryUploadResult.SecureUrl.ToString()
                };
                int imageRowsAffected = await imageData.InsertImage(image);
                if (imageRowsAffected != 1)
                {
                    findData.DeleteFind(find.FindId);
                    return Results.Problem("There was a problem saving image data");
                }

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
                var findRowsAffected = await findData.DeleteFind(findId);

                if (findRowsAffected == 0)
                {
                    return Results.Problem("There was a problem deleting the find");
                }
                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.Problem("There was a problem deleting the find");
            }
        }
    }
}
