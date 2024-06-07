using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URM.Core.Models;

namespace URM.Repository
{
	public class AppDbContext : IdentityDbContext<AppUser, AppRole, string>
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
		
	}
}
