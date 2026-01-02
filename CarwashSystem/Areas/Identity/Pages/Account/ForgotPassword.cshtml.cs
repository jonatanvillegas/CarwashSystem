// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace CarwashSystem.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;

        public ForgotPasswordModel(UserManager<IdentityUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code },
                    protocol: Request.Scheme);

                var emailBody = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"">
                    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                    <title>Restablecer Contraseña</title>
                </head>
                <body style=""font-family: Arial, sans-serif; line-height: 1.5; color: #333333; margin: 0; padding: 0;"">
                    <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"">
                        <tr>
                            <td align=""center"" style=""padding: 20px 0;"">
                                <table width=""100%"" max-width=""600px"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background-color: #ffffff;"">
                                    <tr>
                                        <td style=""padding: 30px;"">
                                            <h1 style=""color: #2c3e50; font-size: 24px; margin-top: 0;"">Restablecer su contraseña</h1>
                                            <p style=""margin-bottom: 20px;"">Hemos recibido una solicitud para restablecer la contraseña de su cuenta.</p>
                                            <p style=""margin-bottom: 25px;"">Por favor, haga clic en el siguiente botón para continuar con el proceso:</p>
                                            
                                            <table cellpadding=""0"" cellspacing=""0"" border=""0"" align=""center"" style=""margin: 25px 0;"">
                                                <tr>
                                                    <td align=""center"" bgcolor=""#007bff"" style=""border-radius: 4px;"">
                                                        <a href=""{HtmlEncoder.Default.Encode(callbackUrl)}"" 
                                                           style=""display: inline-block; padding: 12px 24px; font-family: Arial, sans-serif; font-size: 16px; color: #ffffff; text-decoration: none; font-weight: bold;"">
                                                            Restablecer contraseña
                                                        </a>
                                                    </td>
                                                </tr>
                                            </table>
                                            
                                            <p style=""margin-bottom: 5px;"">Si no puede hacer clic en el botón, copie y pegue esta URL en su navegador:</p>
                                            <p style=""margin-top: 0; word-break: break-all;""><small>{callbackUrl}</small></p>
                                            
                                            <hr style=""border: 0; height: 1px; background-color: #eaeaea; margin: 25px 0;"">
                                            
                                            <p style=""margin-bottom: 5px; font-size: 14px; color: #777777;"">Si no solicitó este restablecimiento, ignore este mensaje. Su contraseña permanecerá sin cambios.</p>
                                            <p style=""margin-top: 5px; font-size: 12px; color: #777777;"">Este enlace expirará en 24 horas por motivos de seguridad.</p>
                                            
                                            <p style=""font-size: 12px; color: #777777; margin-top: 30px;"">© {DateTime.Now.Year} ERP - Loyola. Todos los derechos reservados.</p>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </body>
                </html>";

                await _emailSender.SendEmailAsync(
                    Input.Email,
                    "Restablecimiento de contraseña - ERP - Loyola",
                    emailBody);

                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }
    }
}
