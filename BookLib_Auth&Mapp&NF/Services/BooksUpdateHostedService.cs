namespace BookLib_Auth_Mapp_NF.Services
{
    public class BooksUpdateHostedService : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly IServiceProvider _serviceProvider;

        public BooksUpdateHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(UpdateBooks, null, TimeSpan.Zero, TimeSpan.FromDays(1));
            return Task.CompletedTask;
        }

        private async void UpdateBooks(object? state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var booksUpdateService = scope.ServiceProvider.GetRequiredService<BooksUpdateService>();
                await booksUpdateService.UpdateBooksASync("Books.txt");
            }               
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
