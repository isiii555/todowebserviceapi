using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoWebService.Models.DTOs.Pagination;
using TodoWebService.Models.DTOs.Todo;
using TodoWebService.Providers;
using TodoWebService.Services.Todo;

namespace TodoWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;
        private readonly IRequestUserProvider _provider;
        public TodoController(ITodoService todoService, IRequestUserProvider provider)
        {
            _todoService = todoService;
            _provider = provider;
        }

        [HttpGet("get/{id}")]
        public async Task<ActionResult<TodoItemDto>> Get(int id)
        {
            UserInfo? userInfo = _provider.GetUserInfo();
            var item = await _todoService.GetTodoItem(id);

            return item is not null
                ? item
                : NotFound();
        }

        [HttpGet("all")]
        public async Task<PaginatedListDto<TodoItemDto>?> All(PaginationRequest request, bool? isCompleted)
        {
            UserInfo? userInfo = _provider.GetUserInfo();
            var result = await _todoService.GetAllByUserId(request.Page, request.PageSize, isCompleted,userInfo!.Id);
            return result is not null ? result : null;
        }

        [HttpPost("create")]
        public async Task<ActionResult<TodoItemDto?>> CreateTodo(CreateTodoItemRequest request) 
        {
            UserInfo? userInfo = _provider.GetUserInfo();
            var todoItem = await _todoService.CreateTodo(request,userInfo!.Id);
            return todoItem is null ? BadRequest("Todoitem is not added!") : todoItem;
        }

        [HttpPost("changeStatusTodo")]

        public async Task<ActionResult<TodoItemDto>> ChangeStatusTodo(ChangeTodoItemStatusRequest request)
        {
            UserInfo? userInfo = _provider.GetUserInfo();
            var result = await _todoService.ChangeTodoItemStatus(request.Id, request.Status,userInfo!.Id);
            return result is null ? BadRequest("You can only change your own todoitem's status!") : result;
        }

    }
}
