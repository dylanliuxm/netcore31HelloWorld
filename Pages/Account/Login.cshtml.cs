using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace netcore.Pages.Account
{
    /// <summary>
    /// 演示用登录页：凭据来自 appsettings DemoAuth（明文比较，勿用于生产）。
    /// </summary>
    public class LoginModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(IConfiguration configuration, ILogger<LoginModel> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "请输入用户名")]
            [Display(Name = "用户名")]
            public string Username { get; set; }

            [Required(ErrorMessage = "请输入密码")]
            [DataType(DataType.Password)]
            [Display(Name = "密码")]
            public string Password { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl ?? Url.Content("~/");
            if (User?.Identity?.IsAuthenticated == true)
            {
                // 已登录则直接回业务页
            }
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ReturnUrl = returnUrl;

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var demoUser = _configuration["DemoAuth:Username"] ?? "admin";
            var demoPass = _configuration["DemoAuth:Password"] ?? "admin123";

            if (Input.Username == demoUser && Input.Password == demoPass)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, Input.Username),
                    new Claim("demo", "true")
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    new AuthenticationProperties
                    {
                        IsPersistent = true,
                        AllowRefresh = true
                    });

                _logger.LogInformation("Demo login succeeded for user {Username}", Input.Username);

                if (Url.IsLocalUrl(returnUrl))
                {
                    return LocalRedirect(returnUrl);
                }

                return RedirectToPage("/Index");
            }

            ErrorMessage = "用户名或密码错误。";
            _logger.LogWarning("Demo login failed for user {Username}", Input.Username);
            return Page();
        }
    }
}
