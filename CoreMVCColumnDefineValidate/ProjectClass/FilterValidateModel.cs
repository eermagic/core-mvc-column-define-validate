using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CoreMVCColumnDefineValidate.ProjectClass
{
    public class FilterValidateModel : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid == false)
            {
                string ErrMsg = string.Join("\n", context.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                context.Result = new BadRequestObjectResult(ErrMsg);
            }

            base.OnActionExecuting(context);
        }
    }
}
