using WebOS.Data;
using WebOS.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using WebOS.Services;

namespace WebOS.Services
{
    public static class BulkDBWorkService
    {
        public static Task BulkDBUpdateAsync(this IBulkDBUpdater bulkDBUpdater, 
            ApplicationDbContext _context, UserManager<ApplicationUser> _userManager)
        {
            return bulkDBUpdater.BulkDBUpdateAsync(_context, _userManager);
        }
    }
}