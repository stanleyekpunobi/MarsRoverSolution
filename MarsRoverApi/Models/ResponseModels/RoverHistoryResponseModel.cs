using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarsRoverApi.Models.ResponseModels
{
    public class RoverHistoryResponseModel
    {
        public int RouteHistoryID { get; set; }
        public int PlateauID { get; set; }
        public string RouteHistoryKey { get; set; }
        public string PlateauDimension { get; set; }
        public string SnapShotUrl { get; set; }
        public string RoverStartingPoint { get; set; }
        public string RouteResult { get; set; }
    }
}
