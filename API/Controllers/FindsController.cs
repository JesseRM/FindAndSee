using Domain;
using Microsoft.AspNetCore.Mvc;
using Persistence.Data;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FindsController : ControllerBase
    {
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

        [HttpGet("search/{term}")]
        public async Task<IResult> GetFindsWithTerm(string term, IFindData findData)
        {
            try
            {
                var results = await findData.GetFindsWithTerm(term);

                if (results == null)
                    return Results.NotFound();

                return Results.Ok(results);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IResult> InsertFind(Find find, IFindData findData)
        {
            try
            {
                await findData.InsertFind(find);

                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        [HttpPut("update")]
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
