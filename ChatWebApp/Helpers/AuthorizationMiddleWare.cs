﻿using ChatAppAPI.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using System.Runtime.CompilerServices;
using static Dapper.SqlMapper;
using System.Security.Claims;

namespace ChatAppAPI.Helpers
{
    public class AuthorizationMiddleWare : IAuthorizationMiddlewareResultHandler
    {
        private readonly AuthorizationMiddlewareResultHandler defaultHandler = new();
        private readonly IJwtUtils _jwtUtils;

        public AuthorizationMiddleWare(IJwtUtils jwtUtils)
        {
            _jwtUtils = jwtUtils;
        }


        public async Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
        {
            var allowAnonymous = context.GetEndpoint().Metadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
            {
                await next(context);
                return;
            }
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = token != null ? _jwtUtils.ValidateToken(token) : null;
            
            if (userId != null)
            {
                var identity = new ClaimsIdentity(new List<Claim>
                {
                    new Claim("UserId", userId.ToString(), ClaimValueTypes.String)
                }, "Custom");
                context.User = new ClaimsPrincipal(identity);
                await next(context);
                return;
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }
        }
    }
}
