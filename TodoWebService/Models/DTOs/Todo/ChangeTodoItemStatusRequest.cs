namespace TodoWebService.Models.DTOs.Todo
{
    public class ChangeTodoItemStatusRequest
    {
        public int Id { get; set; }

        public bool Status { get; set; }
    }
}
