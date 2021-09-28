using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsRover.Core.Interfaces
{
    public interface IDataInterface
    {
        Task<int> SaveDataAsync(string sp, object parameters, string constring);
        Task<IEnumerable<T>> GetDataListAsync<T, U>(string sp, U parameters, string constring);
        Task<T> GetDataAsync<T, U>(string sp, U parameters, string constring);
    }
}
