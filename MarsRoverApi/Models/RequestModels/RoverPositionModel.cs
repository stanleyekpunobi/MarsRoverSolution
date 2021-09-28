using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarsRoverApi.Models.RequestModels
{
    public class RoverPositionModel
    {
        public string CurrentPosition { get; set; }
        [Required]
        public string Instructions { get; set; }
    }
}
