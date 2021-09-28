using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarsRoverApi.Models.RequestModels
{
    public class MoveRoverModel
    {
        public string PlateauCordinates { get; set; }
        public List<RoverPositionModel> Rovers { get; set; }
        public MoveRoverModel()
        {
            Rovers = new List<RoverPositionModel>();
        }
    }
}
