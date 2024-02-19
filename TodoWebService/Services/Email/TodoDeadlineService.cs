
using Microsoft.EntityFrameworkCore;
using TodoWebService.Data;
using TodoWebService.Models.DTOs.Email;

namespace TodoWebService.Services.Email
{
    public class TodoDeadlineService : IHostedService
    {
        private readonly TodoDbContext _dbContext;
        private readonly EmailService _emailService;

        public Timer? timer;

        public TodoDeadlineService(TodoDbContext dbContext, EmailService emailService, Timer? timer)
        {
            _dbContext = dbContext;
            _emailService = emailService;
            this.timer = timer;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            SendEmails();
            return Task.CompletedTask;
        }

        public async void SendEmails() {
            var emails = await SelectTodosViaDeadlineAsync();
            foreach (var req in emails!)
            {
                await _emailService.SendEmailAsync(req);
            }
        }

        public async Task<List<EmailRequest>?> SelectTodosViaDeadlineAsync()
        {
            var todoList = await _dbContext.TodoItems.ToListAsync();
            var ad = await _dbContext.Users.ToListAsync();

            var emails = new List<EmailRequest>();

            foreach (var todo in todoList)
            {
                if (todo.DeadlineTime.Day == DateTime.Now.Day + 1 && !todo.DeadlineEmailSent)
                {
                    var req = new EmailRequest()
                    {
                        Body = todo.Id + todo.Text,
                        Subject = "Todo Deadline",
                        ToEmail = todo.User.Email!,
                    };
                    emails.Add(req);
                    todo.DeadlineEmailSent = true;
                    _dbContext.TodoItems.Update(todo);
                    await _dbContext.SaveChangesAsync();
                }
            }
            return emails.Count == 0 ? null : emails!;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
