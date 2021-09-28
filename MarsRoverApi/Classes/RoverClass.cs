using Dapper;
using MarsRover.Core.Classes;
using MarsRover.Core.Interfaces;
using MarsRoverApi.Interfaces;
using MarsRoverApi.Models.RequestModels;
using MarsRoverApi.Models.ResponseModels;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsRoverApi.Classes
{
    public class RoverClass : IRoverInterface
    {
        private StringBuilder result = new StringBuilder();

        //possible rover direction
        private RoverDirection Direction { get; set; }

        //queue for rovers
        private ConcurrentQueue<RoverPositionModel> _rovers = new ConcurrentQueue<RoverPositionModel>();

        //xaxis coordinate of rover
        private int XAxis { get; set; }

        //yaxis cordibate of rover
        private int YAxis { get; set; }

        //rover movements
        private readonly Dictionary<char, Action> _moveRover;

        public RoverClass()
        {
            _moveRover = new Dictionary<char, Action>
            {
                {'M', MoveForward },
                {'L', Spin90DegreesLeft },
                {'R', Spin90DegreesRight },
            };

            XAxis = 0;

            YAxis = 0;

            Direction = RoverDirection.N;
        }

        public async Task<string> MoveAsync(MoveRoverModel model, IDataInterface data, string connectionstring)
        {
           
            //get plateau upper cordinates or size
            var cordinates = model.PlateauCordinates.Split(' ');

            //add rovers to concurrent queue
            QueueRovers(model.Rovers);

            //get last rover item
            var lastRover = _rovers.LastOrDefault();

            RoverPositionModel roverPositionModel = new RoverPositionModel();

            foreach (var rover in _rovers)
            {
                rover.CurrentPosition = rover.CurrentPosition.ToUpper();

                //convert instructions to lower case to maintain consistent string fromat
                rover.Instructions = rover.Instructions.ToUpper();

                //place rover position on plateau
                var roverPostion = rover.CurrentPosition.Split(' ');

                if (roverPostion.Count() != 3)
                {
                    return result.ToString();
                }

                if (rover == lastRover)
                {
                    result.Append(MoveRover(cordinates, roverPostion, rover.Instructions));
                }
                else
                {
                    result.Append(MoveRover(cordinates, roverPostion, rover.Instructions)).Append(" ");
                }

                _rovers.TryDequeue(out roverPositionModel);
            }

            EmptyQueue();

            return result.ToString();
        }

        public async Task<int> AddHistoryAsync(CreateHistoryModel model, string connectionstring, IDataInterface data)
        {
            var parameters = new DynamicParameters();

            string historyKey = GenerateHIstoryKey(model);

            //to save rovers in database
            //DataTable roversDataTable = new DataTable();
            //roversDataTable.Columns.Add("RoverStartingPoint", typeof(string));
            //roversDataTable.Columns.Add("RouteIntructions", typeof(string));
          
            //foreach (var item in model.Rovers)
            //{
            //    roversDataTable.Rows.Add(item.CurrentPosition, item.Instructions);
            //}

            //parameters.Add("Rovers", roversDataTable.AsTableValuedParameter("TVP_History"));

            parameters.Add("RouteHistoryKey", historyKey);

            parameters.Add("PlateauDimension", model.PlateauCordinates);

            parameters.Add("RouteResult", model.PlateauResult);

            parameters.Add("RouteResult", model.PlateauResult);

            parameters.Add("SnapshotUrl", model.PlateauScreenshot);

            int addResult = await data.SaveDataAsync(StoredProcedures.createhistory, parameters, connectionstring);

            return addResult;
        }

        public async Task<List<RoverHistoryResponseModel>> GetHistoryByKeyAsync(MoveRoverModel model, IDataInterface data, string connectionstring)
        {
            string result = string.Empty;

            //check if key exists in db and retrieve result
            string historyKey = GenerateHIstoryKey(model);

            var parameters = new DynamicParameters();

            parameters.Add("HistoryKey", historyKey);

            var historyList = await data.GetDataListAsync<RoverHistoryResponseModel, dynamic>(StoredProcedures.getallroutebykey, parameters, connectionstring);
                      
            return historyList.ToList();
        }

        public async Task<List<RoverHistoryResponseModel>> GetHistoryListAsync(IDataInterface data, string connectionstring)
        {
           
            var parameters = new DynamicParameters();

            var historyList = await data.GetDataListAsync<RoverHistoryResponseModel, dynamic>(StoredProcedures.getallroutehistory, parameters, connectionstring);

            return historyList.ToList();
        }

        private string MoveRover(string[] plauteauUpperAxis, string[] roverPosition, string instructions)
        {
            XAxis = Convert.ToInt32(roverPosition[0]);

            YAxis = Convert.ToInt32(roverPosition[1]);

            Direction = (RoverDirection)Enum.Parse(typeof(RoverDirection), roverPosition[2]);

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

        private void QueueRovers(List<RoverPositionModel> rovers)
        {
            foreach (var rover in rovers)
            {
                _rovers.Enqueue(rover);
            }
        }

        private void EmptyQueue()
        {
            _rovers.Clear();
        }

        private string GenerateHIstoryKey(MoveRoverModel model)
        {
            return string.Join('-', model.PlateauCordinates.Replace(" ", ""), string.Join('-', model.Rovers.Select(m => m.CurrentPosition.Replace(" ", "").ToLower()).ToList()),
                string.Join('-', model.Rovers.Select(m => m.Instructions.ToLower()).ToList()));
        }
    }
}
