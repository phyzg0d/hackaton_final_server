using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using ServerAspNetCoreLinux.Commands;
using ServerAspNetCoreLinux.Commands.Registration;
using ServerAspNetCoreLinux.Commands.SignIn_SignOut;
using ServerAspNetCoreLinux.ServerCore.Commands.Base;

namespace ServerAspNetCoreLinux.ServerCore.Utilities
{
    public class Factory
    {
        public Dictionary<string, Func<IFormCollection, HttpContext, IExecuteCommand>> CommandFactory;
        private ServerContext _context;

        public Factory(ServerContext context)
        {
            _context = context;
            CommandFactory = new Dictionary<string, Func<IFormCollection, HttpContext, IExecuteCommand>>
            {
                {nameof(UserSignInCommand), (form, httpContext) => new UserSignInCommand(form, httpContext)},
                {nameof(SignOutCommand), (form, httpContext) => new SignOutCommand(form, httpContext)},
                {nameof(RegistrationCommand), (form, httpContext) => new RegistrationCommand(form, httpContext)},
                {nameof(UserConnectionCommand), (form, httpContext) => new UserConnectionCommand(form, httpContext)},
                {nameof(GetTracksCommand), (form, httpContext) => new GetTracksCommand(form, httpContext)},
                {nameof(GetYouTubeVideoInfoCommand), (form, httpContext) => new GetYouTubeVideoInfoCommand(form, httpContext)},
                {nameof(AddTrackToHistoryCommand), (form, httpContext) => new AddTrackToHistoryCommand(form, httpContext)},
                {nameof(FindTrackCommand), (form, httpContext) => new FindTrackCommand(form, httpContext)},
            };
        }
    }
}