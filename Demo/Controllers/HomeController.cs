using Demo.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Demo.Controllers
{
    public class HomeController : Controller
    {
        public InitialData _list;

        public HomeController(InitialData list)
        {
            _list = list;
        }

        public IActionResult Index()
        {
            return View(_list.InitialDataList);
        }

        public IActionResult GetData(bool isTreeData, int? id)
        {
            return isTreeData
                ? id == null 
                    ? Ok(_list.InitialDataList.Where(x => x.HasChildren).ToList())
                    : Ok(_list.InitialDataList.Where(x => x.ParentId == id).ToList())
                : Ok(_list.InitialDataList);
        }

        [HttpPost("api/update")]
        public IActionResult Update(UpdateObj updateObj)
        {
            DemoClass movingObj = _list.InitialDataList.First(x => x.Id == updateObj.SourceId);
            if(movingObj.HasChildren)
            {
                return BadRequest("Node contains child nodes");
            }

            _list.InitialDataList.First(x => x.Id == updateObj.SourceId).ParentId = updateObj.DestinationId;
            return Ok();
        }

        [HttpDelete("api/delete")]
        public IActionResult Delete(int id)
        {
            DemoClass deletingObj = _list.InitialDataList.First(x => x.Id == id);
            if (deletingObj.HasChildren)
            {
                return BadRequest("Node contains child nodes");
            }

            _list.InitialDataList.Remove(deletingObj);
            return Ok();
        }
    }

    public class UpdateObj
    {
        public int SourceId { get; set; }
        public int DestinationId { get; set; }
    }
}
