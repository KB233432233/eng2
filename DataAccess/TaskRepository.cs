using System;
using System.Collections.Generic;
using System.Linq;
using TaskManagementSystem.Core.Models;

namespace TaskManagementSystem.DataAccess.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly List<Task> _tasks;
        private int _nextId = 1;

        public TaskRepository()
        {
            _tasks = new List<Task>();
            
            // Initialize with some sample data (optional)
            AddTask(new Task { Title = "Sample Task 1", Description = "Description 1", DueDate = DateTime.Now.AddDays(7) });
            AddTask(new Task { Title = "Sample Task 2", Description = "Description 2", DueDate = DateTime.Now.AddDays(14) });
        }

        public IEnumerable<Task> GetAllTasks()
        {
            return _tasks.OrderBy(t => t.DueDate).ToList();
        }

        public Task GetTaskById(int id)
        {
            return _tasks.FirstOrDefault(t => t.Id == id);
        }

        public void AddTask(Task task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            task.Id = _nextId++;
            _tasks.Add(task);
        }

        public void UpdateTask(Task updatedTask)
        {
            if (updatedTask == null)
                throw new ArgumentNullException(nameof(updatedTask));

            var existingTask = _tasks.FirstOrDefault(t => t.Id == updatedTask.Id);
            if (existingTask == null)
                throw new KeyNotFoundException($"Task with ID {updatedTask.Id} not found");

            existingTask.Title = updatedTask.Title;
            existingTask.Description = updatedTask.Description;
            existingTask.DueDate = updatedTask.DueDate;
            existingTask.IsCompleted = updatedTask.IsCompleted;
        }

        public void DeleteTask(int taskId)
        {
            var taskToDelete = _tasks.FirstOrDefault(t => t.Id == taskId);
            if (taskToDelete == null)
                throw new KeyNotFoundException($"Task with ID {taskId} not found");

            _tasks.Remove(taskToDelete);
        }

        public IEnumerable<Task> GetTasksDueBefore(DateTime date)
        {
            return _tasks.Where(t => t.DueDate < date).ToList();
        }

        public IEnumerable<Task> GetCompletedTasks()
        {
            return _tasks.Where(t => t.IsCompleted).ToList();
        }
    }
}
