using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using MarsRover.Core.Interfaces;
using MarsRoverApi.Interfaces;
using MarsRoverApi.Models.RequestModels;
using MarsRoverApi.Models.ResponseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace MarsRoverApi.Controllers
{
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [ApiController]
    public class PlateauController : ControllerBase
    {
        private readonly IRoverInterface _roverInterface;

        private readonly IDataInterface _dataInterface;

        private readonly IConfiguration _configuration;

        private readonly ILogger<PlateauController> Logger;

        public PlateauController(IRoverInterface roverInterface, IDataInterface dataInterface, IConfiguration configuration)
        {
            _roverInterface = roverInterface;
            _dataInterface = dataInterface;
            _configuration = configuration;

        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("CreateHistory")]
        [HttpPost]
        public async Task<IActionResult> CreateHistoryAsync(CreateHistoryModel model)
        {
            try
            {
                var s3Client = new AmazonS3Client(
                _configuration["Aws:s3keyid"],
                 _configuration["Aws:s3accesskey"],
                 RegionEndpoint.USEast2
                 );

                byte[] bytes = Convert.FromBase64String(model.PlateauScreenshot);

                Guid imagename = Guid.NewGuid();

                model.PlateauScreenshot = $"https://marsroverscreenshots.s3.us-east-2.amazonaws.com/marsroverscreenshots/{imagename}.png";

                using (s3Client)
                {
                    var request = new PutObjectRequest
                    {
                        BucketName = "marsroverscreenshots",
                        CannedACL = S3CannedACL.PublicRead,
                        Key = string.Format("marsroverscreenshots/{0}", $"{imagename}.png")
                    };

                    using (var ms = new MemoryStream(bytes))
                    {
                        request.InputStream = ms;
                       await s3Client.PutObjectAsync(request);
                    }
                }

                int historyAddResult = await _roverInterface.AddHistoryAsync(model, _configuration.GetConnectionString("marsroverdb"), _dataInterface);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message ?? "Unable to save history");
            }

            return StatusCode(StatusCodes.Status200OK, "Plateau saved");
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("MoveRover")]
        [HttpPost]
        public async Task<IActionResult> MoveRoverAsync(MoveRoverModel model)
        {
            string response = string.Empty;

            try
            {
                var historyList = await _roverInterface.GetHistoryByKeyAsync(model, _dataInterface, _configuration.GetConnectionString("marsroverdb"));

                if (historyList.Count() > 1)
                {
                    response = historyList.FirstOrDefault().RouteResult;
                }

                if (!string.IsNullOrEmpty(response))
                {
                    return StatusCode(StatusCodes.Status200OK, response);
                }

                response = await _roverInterface.MoveAsync(model, _dataInterface, _configuration.GetConnectionString("marsroverdb"));

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message ?? "Unable to get rover direction");
            }

            return StatusCode(StatusCodes.Status200OK, response);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("GetAllRoute")]
        [HttpGet]
        public async Task<IActionResult> GetAllRouteHistoryAsync()
        {
            List<RoverHistoryResponseModel> result = new List<RoverHistoryResponseModel>();

            try
            {
                result = await _roverInterface.GetHistoryListAsync(_dataInterface, _configuration.GetConnectionString("marsroverdb"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message ?? "Unable to get history records");
            }

            return StatusCode(StatusCodes.Status200OK, result);
        }

    }
}
