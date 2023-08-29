using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TDL.Models;

namespace TDL.Controllers
{
    public class HomeController : Controller
    {
        TLDEntities tdl = new TLDEntities();
        public ActionResult Index()
        {
            setStatus();
            var activeTasks = tdl.taskTables.Where(task => task.Status == "Active").ToList();
            return View(activeTasks);
        }
        public ActionResult Completed()
        {
            var compTasks = tdl.taskTables.Where(task => task.Status == "Completed").ToList();
            return View(compTasks);
        }

        public ActionResult Expired()
        {
            setStatus();
            var expTasks = tdl.taskTables.Where(task => task.Status == "Expired").ToList();
            return View(expTasks);
        }
        public ActionResult Delete(string returnUrl,int ? taskid)
        {
            Console.WriteLine(taskid);
            if (taskid == null)
            {               
                return RedirectToAction("Completed");
            }

            var itemDelete = tdl.taskTables.Find(taskid);

            if (itemDelete != null)
            {
                tdl.taskTables.Remove(itemDelete);
                tdl.SaveChanges();
            }

            return Redirect(returnUrl);
        }

        public ActionResult Done(string returnUrlx, int? taskid)
        {
            Console.WriteLine(taskid);
            if (taskid == null)
            {
                return Redirect(returnUrlx);
            }

            

            if (taskid != null)
            {

                var tasksToUpdate = tdl.taskTables.ToList();

                foreach (var task in tasksToUpdate)
                {
                    if (task.TaskId ==  taskid)
                        task.Status = "Completed";
                    task.CompDate = DateTime.Now;

                }
                tdl.SaveChanges();
            }

            return Redirect(returnUrlx);
        }



        public ActionResult AddTask()
        {
            return View();
        }



        [HttpPost]
        public ActionResult AddingTask(string tasktitle,DateTime expdate)
        {
            var date = DateTime.Now;

            using (var tdl = new TLDEntities())
            {
               
                var newTask = new taskTable
                {
                    Title = tasktitle,
                    AddDate = date,
                    ExpireDate = expdate,
                    Status = "Active" ,
                    CompDate=expdate
                };

               
                tdl.taskTables.Add(newTask);

                
                tdl.SaveChanges();
            }


            return RedirectToAction("AddTask");
        }









        public void setStatus()
        {
            var date = DateTime.Now;

            var tasksToUpdate = tdl.taskTables.ToList();

            foreach (var task in tasksToUpdate)
            {
                if(task.Status != "Completed"  &&  task.ExpireDate < date)
                task.Status = "Expired";
            }

            tdl.SaveChanges();

        }



    }


}
