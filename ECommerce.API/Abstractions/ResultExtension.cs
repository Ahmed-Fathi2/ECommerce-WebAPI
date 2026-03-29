using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ECommerce.BLL.Abstractions;

public static class ResultExtensions
{

    public static ObjectResult ToProblem(this Result result)
    {
        if (result.IsSuccess)
            throw new InvalidOperationException("Cannot convert success result to a problem");

        var firstError = result.Errors.FirstOrDefault();
        var statusCode = firstError?.StatusCode ?? StatusCodes.Status400BadRequest;

        var problem = Results.Problem(statusCode: statusCode);
        var problemDetails = problem.GetType().GetProperty(nameof(ProblemDetails))!.GetValue(problem) as ProblemDetails;

        var errorsDictionary = result.Errors
            .GroupBy(e => e.Code)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.Description).ToArray()
            );

        problemDetails!.Extensions["errors"] = errorsDictionary;
        problemDetails.Extensions["traceId"] = Activity.Current?.Id;

        return new ObjectResult(problemDetails) { StatusCode = statusCode };


    }
}


