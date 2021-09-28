using MarsRoverApi.Classes;
using MarsRoverApi.Models.RequestModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsRover.Tests
{
    public class MarsRoverTest
    {
        [Test]
        public async Task Testing_12N_LMLMLMLMM_ShouldOutPut13N()
        {
            MoveRoverModel testModel = new MoveRoverModel();

            testModel.PlateauCordinates = "5 5";

            testModel.Rovers.Add(new RoverPositionModel
            {
                CurrentPosition = "1 2 N",
                Instructions = "LMLMLMLMM",
            });

            RoverClass rover = new RoverClass();

            string result = await rover.Move(testModel);

            string exprectedresult = "1 3 N";

            Assert.AreEqual(exprectedresult, result);
        }

        [Test]
        public async Task Testing_33E_MMRMMRMRRM_ShouldOutPut51E()
        {
            MoveRoverModel testModel = new MoveRoverModel();

            testModel.Rovers.Add(new RoverPositionModel
            {
                CurrentPosition = "3 3 E",
                Instructions = "MMRMMRMRRM",
            });

            testModel.PlateauCordinates = "5 5";

            RoverClass rover = new RoverClass();

            string result = await rover.Move(testModel);

            string exprectedresult = "5 1 E";

            Assert.AreEqual(exprectedresult, result);
        }

        [Test]
        public async Task TestingMultipleRovers_ShouldOutPut13N51E()
        {
            MoveRoverModel testModel = new MoveRoverModel();

            testModel.Rovers.Add(new RoverPositionModel
            {
                CurrentPosition = "1 2 N",
                Instructions = "LMLMLMLMM",
            });

            testModel.Rovers.Add(new RoverPositionModel
            {
                CurrentPosition = "3 3 E",
                Instructions = "MMRMMRMRRM",
            });

            testModel.PlateauCordinates = "5 5";

            RoverClass rover = new RoverClass();

            string result = await rover.Move(testModel);

            string exprectedresult = "1 3 N 5 1 E";

            Assert.AreEqual(exprectedresult, result);
        }

    }
}
