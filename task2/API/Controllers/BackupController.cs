using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;
using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using AppContext = DAL.AppContext;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    public class BackupController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly AppContext _dbContext;

        public BackupController(IConfiguration configuration, AppContext dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> BackupDatabase()
        {
            try
            {
                string backupFilePath = _configuration.GetSection("BackupSettings:BackupPath").Value;
                if (string.IsNullOrEmpty(backupFilePath))
                {
                    return BadRequest("Backup path is not configured.");
                }

                string backupQuery = $"BACKUP DATABASE travelsync TO DISK = '{backupFilePath}' WITH FORMAT, MEDIANAME = 'travelsync_backup', NAME = 'Full Backup of travelsync database';";

                await _dbContext.Database.ExecuteSqlRawAsync(backupQuery);

                return Ok($"Backup created successfully at {backupFilePath}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating backup: {ex.Message}");
            }
        }

    }
}