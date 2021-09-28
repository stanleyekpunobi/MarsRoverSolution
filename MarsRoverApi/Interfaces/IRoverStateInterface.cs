using MarsRoverApi.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarsRoverApi.Interfaces
{
    public interface IRoverStateInterface
    {
        void Spin90DegreesRight();
        void Spin90DegreesLeft();
        void MoveOneStepSameDirection();
    }
}
