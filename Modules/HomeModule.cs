using Nancy;
using System.Collections.Generic;

namespace ToDoList //replace with your desired namespace
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
      return View["index.cshtml"];
      };
      Get["/tasks"] = _ => {
        List<Task> AllTasks = Task.GetAll();
        return View["tasks.cshtml", AllTasks];
      };
      Get["/categories"] = _ => {
        List<Category> AllCategories = Category.GetAll();
        return View["categories.cshtml", AllCategories];
      };
      Get["/tasks/new"] = _ => {
      return View["tasks_form.cshtml"];
      };
      Post["/tasks/new"] = _ => {
        Task newTask = new Task(Request.Form["task-description"]);
        newTask.Save();
        return View["success.cshtml"];
      };
      Get["/categories/new"] = _ => {
        return View["categories_form.cshtml"];
      };
      Post["/categories/new"] = _ => {
        Category newCategory = new Category(Request.Form["category-name"]);
        newCategory.Save();
        return View["success.cshtml"];
      };
      Get["tasks/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Task SelectedTask = Task.Find(parameters.id);
        List<Category> TaskCategories = SelectedTask.GetCategories();
        List<Category> AllCategories = Category.GetAll();
        model.Add("task", SelectedTask);
        model.Add("taskCategories", TaskCategories);
        model.Add("allCategories", AllCategories);
        return View["task.cshtml", model];
      };

      Get["categories/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Category SelectedCategory = Category.Find(parameters.id);
        List<Task> CategoryTasks = SelectedCategory.GetTasks();
        List<Task> AllTasks = Task.GetAll();
        model.Add("category", SelectedCategory);
        model.Add("categoryTasks", CategoryTasks);
        model.Add("allTasks", AllTasks);
        return View["category.cshtml", model];
      };
      Post["task/add_category"] = _ => {
        Category category = Category.Find(Request.Form["category-id"]);
        Task task = Task.Find(Request.Form["task-id"]);
        task.AddCategory(category);
        return View["success.cshtml"];
      };
      Post["category/add_task"] = _ => {
        Category category = Category.Find(Request.Form["category-id"]);
        Task task = Task.Find(Request.Form["task-id"]);
        category.AddTask(task);
        return View["success.cshtml"];
      };
      Get["tasks/update/{id}"] = parameters =>
      {
        Task foundTask = Task.Find(parameters.id);
        return View["task_update.cshtml", foundTask];
      };
      Patch["tasks/update/{id}"] = parameters =>
      {
        Task foundTask = Task.Find(parameters.id);
        foundTask.Update(Request.Form["new-description"], Request.Form["completed"]);
        return View["success.cshtml"];
      };
    }
  }
}
