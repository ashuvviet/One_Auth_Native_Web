// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using AutoMapper.Configuration.Annotations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;

namespace IdentityServerAspNetIdentity.Models;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    public string FavoriteColor { get; set; }

    [NotMapped]
    public List<Claim> Claims { get; set; }
}
