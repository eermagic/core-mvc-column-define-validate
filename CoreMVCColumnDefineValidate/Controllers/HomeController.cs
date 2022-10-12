using CoreMVCColumnDefineValidate.Models;
using CoreMVCColumnDefineValidate.ProjectClass;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using static CoreMVCColumnDefineValidate.Models.HomeViewModel;

namespace CoreMVCColumnDefineValidate.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // 回傳 ColumnDD 格式
            List<ColumnDefine> ColumnDefines = ColumnDefine.GetList();
            ViewData["ColumnDefines"] = JsonConvert.SerializeObject(ColumnDefines);

            return View();
        }

        [ValidateAntiForgeryToken]
        public IActionResult AddSave(Student inModel)
        {
            AddSaveOut outModel = new AddSaveOut();

            // 已移至 Filter 所以刪除
            //if (ModelState.IsValid == false)
            //{
            //    // 檢查失敗訊息
            //    outModel.ErrMsg = string.Join("\n", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            //    return Json(outModel);
            //}

            // 省略資料庫動作...

            // 模擬成功訊息
            outModel.ResultMsg = "新增完成";

            return Json(outModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}