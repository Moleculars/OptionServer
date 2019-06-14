using Bb.OptionServer.Exceptions;
using Bb.OptionService.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Authentication;

namespace Bb.Controllers
{

    internal static class ControllerExtension
    {

        /// <summary>
        /// Executes the specified function and return action result.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self">The self.</param>
        /// <param name="func">The business function.</param>
        /// <param name="authenticated">if set to <c>true</c> must be authenticated or throw an exception.</param>
        /// <returns></returns>
        internal static ActionResult<RootResultModel<T>> Execute<T>(this ControllerBase self, Func<ControllerBase, string, T> func, bool authenticated = true)
        {

            if (!self.ModelState.IsValid)
                return self.BadRequest(self.ModelState);

            string username = self.User?.Identity.Name ?? string.Empty;
            if (authenticated && string.IsNullOrEmpty(username))
                return self.Unauthorized();

            ObjectResult datasResult = null;

            try
            {

                var result = func(self, username);

                if (result != null)
                    datasResult = self.Ok(new RootResultModel<T>() { Valid = true, Datas = result });

                else
                    datasResult = self.BadRequest(new RootResultModel<T>() { Valid = false, Message = "failed to execute.", });

            }
            catch(NotEnoughtRightException e1)
            {
                datasResult = self.Unauthorized(new RootResultModel<T>() { Valid = false, Message = e1.Message });
            }
            catch (AuthenticationException e2)
            {
                datasResult = self.Unauthorized(new RootResultModel<T>() { Valid = false, Message = e2.Message });
            }
            catch (Exception e3)
            {
                datasResult = self.BadRequest(new RootResultModel<T>() { Valid = false, Message = e3.Message });
            }

            return datasResult;

        }
    }

}
