using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Catalog.Api.Exceptions
{
    public class InvalidRequestException : Exception
    {
        public Dictionary<string, IReadOnlyCollection<string>> Errors { get; set; } = new Dictionary<string, IReadOnlyCollection<string>>();

        public InvalidRequestException(ModelStateDictionary modelStateDictionary)
        {
            foreach(var e in modelStateDictionary)
            {
                if(e.Value.ValidationState == ModelValidationState.Invalid)
                    Errors[e.Key] = new List<string>(e.Value.Errors.Select(x => x.ErrorMessage));
            }
        }
    }
}
