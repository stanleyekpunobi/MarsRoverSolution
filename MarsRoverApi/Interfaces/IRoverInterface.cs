using MarsRover.Core.Interfaces;
using MarsRoverApi.Models.RequestModels;
using MarsRoverApi.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarsRoverApi.Interfaces
{
    public interface IRoverInterface
    {
        public Task<string> MoveAsync(MoveRoverModel model, IDataInterface data, string connectionstring);
        public Task<List<RoverHistoryResponseModel>> GetHistoryByKeyAsync(MoveRoverModel model, IDataInterface data, string connectionstring);

        public Task<int> AddHistoryAsync(CreateHistoryModel model, string connectionstring, IDataInterface data);

        Task<List<RoverHistoryResponseModel>> GetHistoryListAsync(IDataInterface data, string connectionstring);
    }
}
