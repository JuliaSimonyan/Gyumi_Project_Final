namespace Gyumri.Middleware
{
    public class LocalizationMiddleware
    {
        private readonly RequestDelegate _next;

        public LocalizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            // Ստուգել լեզուն cookie-ից
            var lang = httpContext.Request.Cookies["UserCulture"];
            if (string.IsNullOrEmpty(lang))
            {
                lang = "en";  // Եթե չի ընտրվել, ապա մուտքը կլինի անգլերեն
            }

            // Կարգավորել CurrentCulture և CurrentUICulture
            var cultureInfo = new System.Globalization.CultureInfo(lang);
            System.Globalization.CultureInfo.CurrentCulture = cultureInfo;
            System.Globalization.CultureInfo.CurrentUICulture = cultureInfo;

            // Շարունակել պրոցեսը
            await _next(httpContext);
        }
    }

}
