using Canaa.DataContracts.Videos;
using Canaa.Infra.ExternalServices.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Canaa.WorkFlow
{
    public class ProcessController: ControllerBase
    {
        [HttpGet("progress/{processId}")]
        public ActionResult<ProgressResponse> GetProgress(string processId)
        {
            var progress = ProgressService.GetProgress(processId);
            return Ok(progress);
        }
    }
}
