using TodoWebService.Models.DTOs.Pagination;
using TodoWebService.Models.DTOs.Todo;

namespace TodoWebService.Services.Todo
{
    public interface ITodoService
    {
        Task<TodoItemDto?> GetTodoItem(int id);
        Task<TodoItemDto?> CreateTodo(CreateTodoItemRequest request,string userId);
        Task<TodoItemDto?> ChangeTodoItemStatus(int id, bool isCompleted,string userId);
        Task<bool> DeleteTodo(int id);
        Task<PaginatedListDto<TodoItemDto>> GetAll(int page, int pageSize, bool? isCompleted); 
        Task<PaginatedListDto<TodoItemDto>> GetAllByUserId(int page, int pageSize, bool? isCompleted,string userId); 
    }
}
