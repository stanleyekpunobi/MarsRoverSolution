using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarsRoverApi.Models.RequestModels
{
    public class CreateHistoryModel : MoveRoverModel
    {
        public string PlateauScreenshot { get; set; }
        public string PlateauResult { get; set; }
    }
}
