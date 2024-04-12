using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System.Net;


namespace AMS.SHARED.Extensions
{
    public static class ValidationResultExtensions
    {
        public static ProblemDetails ToProblemDetails(this IEnumerable<ValidationFailure> validationFailures)
        {
            var errors = validationFailures.ToDictionary(x => x.PropertyName, x => x.ErrorMessage);

            var problemDetails = new ProblemDetails
            {
                Type = "ValidationError",
                Detail = "please check the error list for more details",
                Status = (int)(HttpStatusCode.UnprocessableEntity),
                Title = "Validation Failed"
            };

            problemDetails.Extensions.Add("errors", errors);
            return problemDetails;
        }
    }
}
