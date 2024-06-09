﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URM.Core.Models
{
	public class AppRole: IdentityRole
	{
        public string Description { get; set; } // db de nullable değil o yüzden doldurulması zorunlu istersek değişebiliriz
    }

		
}
