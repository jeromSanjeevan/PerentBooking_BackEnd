using Microsoft.Data.SqlClient;
using ParentBookingAPI.Helper;

namespace ParentBookingAPI.HostService
{
    public class DatabaseUpdateService : IHostedService, IDisposable
    {
        private readonly ILogger<DatabaseUpdateService> _logger;
        private Timer _timer;
        private readonly IConfiguration _configuration;

        private readonly DatabaseHelper _dbHelper;


        public DatabaseUpdateService(ILogger<DatabaseUpdateService> logger, IConfiguration configuration, DatabaseHelper dbHelper)
        {
            _logger = logger;
            _configuration = configuration;
            _dbHelper = dbHelper;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("DatabaseUpdateService is starting.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(10));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            _logger.LogInformation("DatabaseUpdateService is doing work at: {time}", DateTimeOffset.Now);

            // Implement your database update logic here using Entity Framework or other database access methods
            // For example, you can use the _configuration to access your connection string and update the database

            // var connectionString = _configuration.GetConnectionString("DefaultConnection");
            // Implement your database update logic here

            _logger.LogInformation("DatabaseUpdateService is doing work at: {time}", DateTimeOffset.Now);


            // You can pass null or an empty array of parameters if your stored procedure doesn't require any.
            SqlParameter[] parameters = null;

            int rowsAffected = _dbHelper.ExecuteUpdateStoredProcedureAsync("SP_ReleaseLockAuto", parameters).Result;

            // You can log or handle the result as needed
            _logger.LogInformation("Stored procedure {0} executed with {1} rows affected.", "SP_ReleaseLockAuto", rowsAffected);

            // You can add error handling and additional logic here as well


        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("DatabaseUpdateService is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
