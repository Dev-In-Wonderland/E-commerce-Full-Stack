using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Text;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using Betacomio.Models;

namespace Betacomio.Authentication
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly UsersBetacomioContext _context;

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            UsersBetacomioContext context
            ) : base(options, logger, encoder, clock)
            
        { _context = context; }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            Response.Headers.Add("WWW-Authenticate", "Basic");

            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return Task.FromResult(AuthenticateResult.Fail("Autorizzazione mancante"));
            }

            var authorizationHeader = Request.Headers["Authorization"].ToString();

            var authoHeaderRegEx = new Regex("Basic (.*)");

            if (!authoHeaderRegEx.IsMatch(authorizationHeader))
            {
                return Task.FromResult(AuthenticateResult.Fail("Authorization Code, not properly formatted"));
            }

            var authBase64 = Encoding.UTF8.GetString(Convert.FromBase64String(authoHeaderRegEx.Replace(authorizationHeader, "$1")));
            var authSplit = authBase64.Split(Convert.ToChar(":"), 2);

            var authUser = authSplit[0];
            var authPassword = authSplit.Length > 1 ? authSplit[1] : throw new Exception("Unable to get Password");

            // IL CONTROLLO NON VA, NATURLAMENTE, FATTO HARDCODED, SICURAMENTE SUBENTRA UN CONTROLLO
            // SUGLI USER NEL DB? NEL FILE? IL TUTTO PREVIO ENCRYPT/DECRYPT
            // if (checkCredentias(authuser,authpassword)
            if ((!_context.Users.Where(e => e.EmailAddress.Equals(authUser)).Any()) && (!_context.Users.Where(e => e.PasswordHash.Equals(authPassword)).Any()))
            {
                return Task.FromResult(AuthenticateResult.Fail("User e/o password errati !!!"));
            }

            bool isAdmin = _context.Users.Where(e => e.EmailAddress.Equals(authUser)).Select(a => a.IsAdmin).First();      // controllare cosa ha preso
            Console.WriteLine(isAdmin);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, authUser),
                new Claim(ClaimTypes.Role, isAdmin ? "Admin" : "User")
            };

            var claimsIdentity = new ClaimsIdentity(claims, Scheme.Name);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name)));

            //var roles = new List<string>();  /// ADMIN NON PUO ACQUISTARE?????? SFIGATO
            //if (isAdmin)
            //{
            //    roles.Add("Admin");
            //}
            //else
            //{
            //    roles.Add("User");
            //}

            //var authenticatedUser = new AuthenticatedUser("BasicAuthentication", true, authUser, roles);

            //var claimsIdentity = new ClaimsIdentity(authenticatedUser, Scheme.Name);
            //var claims = new List<Claim>();
            //claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, authUser));
            //claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, isAdmin ? "Admin" : "User"));

            //var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            //return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name)));
        }
    }
}
