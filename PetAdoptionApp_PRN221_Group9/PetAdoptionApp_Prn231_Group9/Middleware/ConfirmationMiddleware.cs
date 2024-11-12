using BusinessLogicLayer;
using DataAccessObjects;

namespace PetAdoptionApp_Prn231_Group9.Middleware
{
    public class ConfirmationMiddleware
    {
        private readonly RequestDelegate _next;


        public ConfirmationMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            //create request
            using (var scope = httpContext.RequestServices.CreateScope())
            {
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                var token = httpContext.Request.Query["token"];

                if (!string.IsNullOrEmpty(token))
                {
                    var user = await unitOfWork._userRepo.GetUserByConfirmationToken(token);

                    if (user != null && !user.IsConfirmed)
                    {
                        //verify user
                        user.IsConfirmed = true;
                        user.ConfirmationToken = null;
                        unitOfWork._userRepo.Update(user);


                        var IsSuccess = await unitOfWork.SaveChangeAsync() > 0;
                        if (IsSuccess)
                        {
                            await httpContext.Response.WriteAsync("Email has been confirmed successfully!");
                            return;
                        }
                        
                    }
                }
            }

            await _next(httpContext);
        }
    }
}
