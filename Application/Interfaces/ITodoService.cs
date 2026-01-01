using Domain.Entities;

namespace Application.Interfaces;

public interface ITodoService
{
    Task<List<TodoTask>> GetAllTasksAsync();
    
    Task AddTaskAsync(TodoTask task);
    
    Task UpdateTaskAsync(TodoTask task);
    
    Task DeleteTaskAsync(string taskId);
}