//using Microsoft.AspNetCore.Authentication;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Options;
//using System;
//using System.IdentityModel.Tokens.Jwt;
//using System.IO;
//using System.Text;
//using System.Text.Encodings.Web;
//using System.Threading.Tasks;

//namespace Richviet.API.Common.Middlewares.Authentication
//{
//    public class ValidateRichvietAuthenticationHandler : AuthenticationHandler<ValidateRichvietAuthenticationSchemeOptions>
//    {
//        public ValidateRichvietAuthenticationHandler(
//            IOptionsMonitor<ValidateRichvietAuthenticationSchemeOptions> options,
//            ILoggerFactory logger,
//            UrlEncoder encoder,
//            ISystemClock clock)
//            : base(options, logger, encoder, clock)
//        {
//        }
//        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
//        {
//            if (!Request.Headers.ContainsKey("X-Richviet-Token"))
//            {
//                return Task.FromResult(AuthenticateResult.Fail("X-Richviet-Token Not Found."));
//            }

//            var token = Request.Headers["X-Richviet-Token"].ToString();

//            try
//            {
//                // convert the input string into byte stream
//                using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(token)))
//                {
//                    // deserialize stream into token model object
//                    var model = Serializer.
//                }
//            }
//            catch (System.Exception ex)
//            {
//                Console.WriteLine("Exception Occured while Deserializing: " + ex);
//                return Task.FromResult(AuthenticateResult.Fail("TokenParseException"));
//            }
//        }
//    }
//}
