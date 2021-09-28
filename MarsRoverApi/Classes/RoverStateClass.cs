using MarsRoverApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarsRoverApi.Classes
{
    public class RoverStateClass : IRoverStateInterface
    {

        //possible directions of rover from cardinal points
        public RoverDirection Direction { get; set; }

        //xaxis coordinate of rover
        public int XAxis { get; set; }

        //yaxis cordibate of rover
        public int YAxis { get; set; }

        public void MoveOneStepSameDirection()
        {
            switch (Direction)
            {
                case RoverDirection.N:
                    YAxis += 1;
                    break;
                case RoverDirection.S:
                    YAxis -= 1;
                    break;
                case RoverDirection.E:
                    XAxis += 1;
                    break;
                case RoverDirection.W:
                    XAxis -= 1;
                    break;
                default:
                    break;
            }
        }

        public void Spin90DegreesLeft()
        {
            switch (Direction)
            {
                case RoverDirection.N:
                    Direction = RoverDirection.W;
                    break;
                case RoverDirection.S:
                    Direction = RoverDirection.E;
                    break;
                case RoverDirection.E:
                    Direction = RoverDirection.N;
                    break;
                case RoverDirection.W:
                    Direction = RoverDirection.S;
                    break;
                default:
                    break;
            }
        }

        public void Spin90DegreesRight()
        {
            switch (Direction)
            {
                case RoverDirection.N:
                    Direction = RoverDirection.E;
                    break;
                case RoverDirection.S:
                    Direction = RoverDirection.W;
                    break;
                case RoverDirection.E:
                    Direction = RoverDirection.S;
                    break;
                case RoverDirection.W:
                    Direction = RoverDirection.N;
                    break;
                default:
                    break;
            }
        }
    }
}
