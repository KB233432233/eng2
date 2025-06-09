using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Business.Interfaces;
using TaskManagementSystem.Core.Models;

namespace TaskManagementSystem.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            ITaskService taskService,
            ILogger<HomeController> logger)
        {
            _taskService = taskService ?? throw new ArgumentNullException(nameof(taskService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IActionResult Index()
        {
            try
            {
                var tasks = _taskService.GetAllTasks();
                return View(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving tasks");
                return View("Error", new ErrorViewModel { 
                    RequestId = HttpContext.TraceIdentifier 
                });
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Task task)
        {
            if (!ModelState.IsValid)
            {
                return View(task);
            }

            try
            {
                _taskService.AddTask(task);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating task");
                ModelState.AddModelError("", "An error occurred while creating the task.");
                return View(task);
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            try
            {
                var task = _taskService.GetTaskById(id);
                if (task == null)
                {
                    return NotFound();
                }
                return View(task);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving task with ID {id}");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Task task)
        {
            if (id != task.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(task);
            }

            try
            {
                _taskService.UpdateTask(task);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating task with ID {id}");
                ModelState.AddModelError("", "An error occurred while updating the task.");
                return View(task);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteTask(int id)
        {
            try
            {
                _taskService.DeleteTask(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting task with ID {id}");
                TempData["ErrorMessage"] = "An error occurred while deleting the task.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public IActionResult MarkAsCompleted(int id)
        {
            try
            {
                _taskService.MarkTaskAsCompleted(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error marking task with ID {id} as completed");
                TempData["ErrorMessage"] = "An error occurred while marking the task as completed.";
                return RedirectToAction(nameof(Index));
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { 
                RequestId = HttpContext.TraceIdentifier 
            });
        }
    }
}
