using Azure.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;
using TodoWebService.Data;
using TodoWebService.Models.DTOs.Pagination;
using TodoWebService.Models.DTOs.Todo;
using TodoWebService.Models.Entities;

namespace TodoWebService.Services.Todo
{
    public class TodoService : ITodoService
    {
        private readonly TodoDbContext _context;
        private readonly ILogger<TodoService> _logger;
        public TodoService(TodoDbContext context,ILogger<TodoService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<TodoItemDto?> ChangeTodoItemStatus(int id, bool isCompleted, string userId)
        {
            var todo = await _context.TodoItems.FirstOrDefaultAsync(x => x.Id == id);
            var oldStatus = todo.IsCompleted;
            if (todo!.UserId == userId)
            {
                todo!.IsCompleted = isCompleted;
                _context.TodoItems.Update(todo);
                await _context.SaveChangesAsync();
                var todoDto = todo.Adapt<TodoItemDto>();
                _logger.LogInformation($"Id : {todoDto.Id} todo item status changed {oldStatus} -> {todoDto.IsCompleted}!");
                return todoDto;
            }
            return null;
        }

        public async Task<TodoItemDto?> CreateTodo(CreateTodoItemRequest request, string userId)
        {

            var newTodoItem = request.Adapt<TodoItem>();
            newTodoItem.UserId = userId;
            var entity = _context.TodoItems.Add(newTodoItem).Entity;
            await _context.SaveChangesAsync();
            var newTodoItemDto = new TodoItemDto(entity.Id, entity.Text, entity.IsCompleted, entity.CreatedTime);
            _logger.LogInformation($"Id : {newTodoItemDto.Id} todo item created!");
            return newTodoItemDto;
        }

        public Task<bool> DeleteTodo(int id)
        {
            throw new NotImplementedException();
        }
        public async Task<PaginatedListDto<TodoItemDto>> GetAllByUserId(int page, int pageSize, bool? isCompleted, string userId)
        {
            IQueryable<TodoItem> query = _context.TodoItems.Where(td => td.UserId == userId).AsQueryable();

            if (isCompleted.HasValue)
                query = query.Where(e => e.IsCompleted == isCompleted);

            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            var totalCount = await query.CountAsync();

            return new PaginatedListDto<TodoItemDto>(
                items.Select(e => new TodoItemDto(
                    id: e.Id,
                    text: e.Text,
                    isCompleted: e.IsCompleted,
                    createdTime: e.CreatedTime
                )),
                new PaginationMeta(page, pageSize, totalCount)
                );
        }

        public async Task<PaginatedListDto<TodoItemDto>> GetAll(int page, int pageSize, bool? isCompleted)
        {
            IQueryable<TodoItem> query = _context.TodoItems.AsQueryable();

            if (isCompleted.HasValue)
                query = query.Where(e => e.IsCompleted == isCompleted);

            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            var totalCount = await query.CountAsync();

            return new PaginatedListDto<TodoItemDto>(
                items.Select(e => new TodoItemDto(
                    id: e.Id,
                    text: e.Text,
                    isCompleted: e.IsCompleted,
                    createdTime: e.CreatedTime
                )),
                new PaginationMeta(page, pageSize, totalCount)
                );
        }

        public async Task<TodoItemDto?> GetTodoItem(int id)
        {
            var todoItem = await _context.TodoItems.FirstOrDefaultAsync(e => e.Id == id);

            return todoItem is not null
                ? new TodoItemDto(
                    id: todoItem.Id,
                    text: todoItem.Text,
                    isCompleted: todoItem.IsCompleted,
                    createdTime: todoItem.CreatedTime)
                : null;
        }
    }
}
