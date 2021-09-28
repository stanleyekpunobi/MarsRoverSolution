using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarsRoverApi.Classes
{
    public class MoveRoverClass
    {
        private RoverDirection Direction { get; set; }

        //xaxis coordinate of rover
        private int XAxis { get; set; }

        //yaxis cordibate of rover
        private int YAxis { get; set; }

        //rover movements
        private readonly Dictionary<char, Action> _moveRover;

        public MoveRoverClass()
        {
            _moveRover = new Dictionary<char, Action>
            {
                {'m', MoveForward },
                {'l', Spin90DegreesLeft },
                {'r', Spin90DegreesRight },
            };

            XAxis = 0;

            YAxis = 0;

            Direction = RoverDirection.N;
        }

        public string MoveRover(string[] plauteauUpperAxis, string[] roverPosition, string instructions)
        {
            XAxis = Convert.ToInt32(roverPosition[0]);

            YAxis = Convert.ToInt32(roverPosition[1]);

            Direction = (RoverDirection)Convert.ToInt32(roverPosition[2]);

            foreach (var command in instructions)
            {
                _moveRover[command]();
            }

            if (XAxis < 0 || XAxis > Convert.ToInt32(plauteauUpperAxis[0]))
            {
                return $"XAxis position is out of plateau boundries XAxis = {XAxis} plauteau XAxis = {plauteauUpperAxis[0]}";
            }

            if (YAxis < 0 || YAxis > Convert.ToInt32(plauteauUpperAxis[1]))
            {
                return $"YAxis position is out of plateau boundries YAxis = {YAxis} plauteau YAxis = {plauteauUpperAxis[1]}";
            }

            return string.Join(' ', XAxis, YAxis, Direction);
        }

        private void MoveForward()
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

        private void Spin90DegreesLeft()
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

        private void Spin90DegreesRight()
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
