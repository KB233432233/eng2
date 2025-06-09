using System;
using System.Collections.Generic;
using TaskManagementSystem.Core.Interfaces;
using TaskManagementSystem.Core.Models;

namespace TaskManagementSystem.Business.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
        }

        public IEnumerable<Task> GetAllTasks()
        {
            return _taskRepository.GetAllTasks();
        }

        public Task GetTaskById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid task ID", nameof(id));

            var task = _taskRepository.GetTaskById(id);
            
            if (task == null)
                throw new KeyNotFoundException($"Task with ID {id} not found");

            return task;
        }

        public void AddTask(Task task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            if (string.IsNullOrWhiteSpace(task.Title))
                throw new ArgumentException("Task title is required", nameof(task.Title));

            if (task.DueDate < DateTime.Today)
                throw new ArgumentException("Due date cannot be in the past", nameof(task.DueDate));

            _taskRepository.AddTask(task);
        }

        public void UpdateTask(Task task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            if (task.Id <= 0)
                throw new ArgumentException("Invalid task ID", nameof(task.Id));

            if (string.IsNullOrWhiteSpace(task.Title))
                throw new ArgumentException("Task title is required", nameof(task.Title));

            if (task.DueDate < DateTime.Today)
                throw new ArgumentException("Due date cannot be in the past", nameof(task.DueDate));

            var existingTask = _taskRepository.GetTaskById(task.Id);
            if (existingTask == null)
                throw new KeyNotFoundException($"Task with ID {task.Id} not found");

            _taskRepository.UpdateTask(task);
        }

        public void DeleteTask(int taskId)
        {
            if (taskId <= 0)
                throw new ArgumentException("Invalid task ID", nameof(taskId));

            var taskToDelete = _taskRepository.GetTaskById(taskId);
            if (taskToDelete == null)
                throw new KeyNotFoundException($"Task with ID {taskId} not found");

            _taskRepository.DeleteTask(taskId);
        }

        public IEnumerable<Task> GetTasksDueBefore(DateTime date)
        {
            return _taskRepository.GetTasksDueBefore(date);
        }

        public IEnumerable<Task> GetCompletedTasks()
        {
            return _taskRepository.GetCompletedTasks();
        }

        public void MarkTaskAsCompleted(int taskId)
        {
            if (taskId <= 0)
                throw new ArgumentException("Invalid task ID", nameof(taskId));

            var task = _taskRepository.GetTaskById(taskId);
            if (task == null)
                throw new KeyNotFoundException($"Task with ID {taskId} not found");

            task.IsCompleted = true;
            _taskRepository.UpdateTask(task);
        }
    }
}
