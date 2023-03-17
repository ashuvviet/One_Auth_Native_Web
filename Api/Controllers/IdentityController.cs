// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("identity")]
    [Authorize]
    public class IdentityController : ControllerBase
    {
        public async Task<IActionResult> Get()
        {
            var list = new List<Claim> { new Claim() { Type = "Sciex", Value = "Welcome to Enterprise Solution" } };
            var a = await HttpContext.AuthenticateAsync();
            foreach (var item in a.Properties.Items)
            {
                list.Add(new Claim() { Type = item.Key, Value = item.Value });
            }
            
            //list.AddRange(User.Claims.Select(c => new Claim () { Type = c.Type, Value = c.Value }));

            return new JsonResult(list);
        }
    }

    public class Claim
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }
}