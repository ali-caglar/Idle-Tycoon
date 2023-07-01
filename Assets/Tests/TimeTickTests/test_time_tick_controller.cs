using System;
using NSubstitute;
using NUnit.Framework;
using TimeTick;
using UnityEngine;
using UnityEngine.TestTools.Utils;

namespace Tests.TimeTickTests
{
    public class test_time_tick_controller
    {
        private float[] _expectedPercentages = { 0.1f, 0.25f, 0.5f, 0.6f, 0.99f };

        private float[] _totalTimesToTest =
        {
            0.05f, 0.075f, 0.1f, 0.15f, 0.35f, 0.475f, 0.5f, 0.65f, 0.75f, 0.95f, 1,
            1.5f, 2f, 2.1f, 1.35f, 2.75f, 2.18f, 3.5f, 4f, 5.1f, 5.35f, 4.75f, 3.18f,
        };

        private float[] _timesToAddToExceedTotalTimeOf1Second =
        {
            1.1f, 1.25f, 1.5f, 1.6f, 1.99f, 4.1f, 5.25f, 6.5f, 7.6f, 8.99f, 3.1f, 3.25f, 3.5f,
            3.6f, 3.99f, 4.3f, 5.4f, 6.2f, 7.26f, 8.69f
        };

        [Test]
        public void Should_Time_Updated()
        {
            var controller = new TimeTickController(0, 1);
            float timeToIncrease = 0.1f;

            controller.UpdateTimer(timeToIncrease);

            Assert.AreEqual(timeToIncrease, controller.TickTimer);
        }

        [Test]
        public void Should_Progress_Equal_To_Percentage()
        {
            var totalTime = 6.2f;

            foreach (var expectedPercentage in _expectedPercentages)
            {
                var controller = new TimeTickController(0, totalTime);
                float timeToIncrease = totalTime * expectedPercentage;

                controller.UpdateTimer(timeToIncrease);

                Assert.AreEqual(expectedPercentage, controller.GetProgress);
            }
        }

        [Test]
        public void Should_Exceeded_Timer_Reduced_By_Total_Duration()
        {
            var comparer = new FloatEqualityComparer(1e-6f);

            foreach (var totalTime in _totalTimesToTest)
            {
                foreach (var timeToIncrease in _timesToAddToExceedTotalTimeOf1Second)
                {
                    var controller = new TimeTickController(0, totalTime);
                    float expectedResult = timeToIncrease;

                    // Calculating with modulus is returning bullshit
                    while (expectedResult >= totalTime)
                    {
                        expectedResult -= totalTime;
                    }

                    controller.UpdateTimer(timeToIncrease);

                    Assert.That(controller.TickTimer, Is.EqualTo(expectedResult).Using(comparer));
                }
            }
        }

        [Test]
        public void Should_Timer_Less_Than_Duration_On_Init()
        {
            var controller = new TimeTickController(1.5f, 1);

            Assert.Less(controller.TickTimer, controller.TickDuration);
        }

        [Test]
        public void Should_Timer_Greater_or_Equal_To_0()
        {
            var controller = new TimeTickController(-1f, 1);

            Assert.GreaterOrEqual(0, controller.TickTimer);
        }

        [Test]
        public void Should_Throw_Error_Initiating_Controller_With_0_Duration()
        {
            void CreateNewController()
            {
                var controller = new TimeTickController(0, 0);
            }

            Assert.Throws<Exception>(CreateNewController, "Should have throw error.");
        }

        [Test]
        public void Should_Throw_Error_Initiating_Controller_Less_Than_0_Duration()
        {
            void CreateNewController()
            {
                var controller = new TimeTickController(0, -1f);
            }

            Assert.Throws<Exception>(CreateNewController, "Should have throw error.");
        }

        [Test]
        public void Should_Throw_Error_Updating_Controller_With_0_Duration()
        {
            var controller = new TimeTickController(0, 1);

            void UpdateControllerDuration()
            {
                controller.UpdateTickDuration(0);
            }

            Assert.Throws<Exception>(UpdateControllerDuration, "Should have throw error.");
        }

        [Test]
        public void Should_Throw_Error_Updating_Controller_Less_Than_0_Duration()
        {
            var controller = new TimeTickController(0, 1f);

            void UpdateControllerDuration()
            {
                controller.UpdateTickDuration(-1f);
            }

            Assert.Throws<Exception>(UpdateControllerDuration, "Should have throw error.");
        }

        [Test]
        public void Should_Update_Tick_Duration()
        {
            var controller = new TimeTickController(0, 1);
            float newDuration = 0.5f;
            controller.UpdateTickDuration(newDuration);

            Assert.AreEqual(newDuration, controller.TickDuration);
        }

        [Test]
        public void Should_Time_Increase_Always_Greater_Than_0()
        {
            var controller = new TimeTickController(0, 1);
            float timeToSubtract = -0.5f;
            controller.UpdateTimer(timeToSubtract);

            Assert.LessOrEqual(0, controller.TickTimer);
        }

        [Test]
        public void Should_Reset_After_Updated_Tick_Duration_If_Exceeded()
        {
            var controller = new TimeTickController(0.6f, 1);
            float newDuration = 0.5f;
            controller.UpdateTickDuration(newDuration);

            Assert.AreEqual(0, controller.TickTimer);
        }
    }
}