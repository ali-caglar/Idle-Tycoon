using System;
using Extensions;
using Helpers;
using NUnit.Framework;
using TimeTick;
using UnityEngine.TestTools.Utils;

namespace Tests.TimeTickTests
{
    public class TestTimeTickController
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
            var controller = new TimeTickController(0, 1, true);
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
                var controller = new TimeTickController(0, totalTime, true);
                float timeToIncrease = totalTime * expectedPercentage;

                controller.UpdateTimer(timeToIncrease);

                Assert.AreEqual(expectedPercentage, controller.GetProgress);
            }
        }

        [Test]
        public void Should_Exceeded_Timer_Reduced_By_Total_Duration()
        {
            var comparer = new FloatEqualityComparer(Constants.FloatingPointTolerance);

            foreach (var totalTime in _totalTimesToTest)
            {
                foreach (var timeToIncrease in _timesToAddToExceedTotalTimeOf1Second)
                {
                    var controller = new TimeTickController(0, totalTime, true);
                    float expectedResult = timeToIncrease;

                    // Calculating with modulus is returning bullshit
                    while (expectedResult.IsExceeded(totalTime))
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
            var controller = new TimeTickController(1.5f, 1, true);

            Assert.Less(controller.TickTimer, controller.TickDuration);
        }

        [Test]
        public void Should_Timer_Greater_or_Equal_To_0()
        {
            var controller = new TimeTickController(-1f, 1, true);

            Assert.GreaterOrEqual(0, controller.TickTimer);
        }

        [Test]
        public void Should_Throw_Error_Initiating_Controller_With_0_Duration()
        {
            void CreateNewController()
            {
                var controller = new TimeTickController(0, 0, true);
            }

            Assert.Throws<Exception>(CreateNewController, "Should have throw error.");
        }

        [Test]
        public void Should_Throw_Error_Initiating_Controller_Less_Than_0_Duration()
        {
            void CreateNewController()
            {
                var controller = new TimeTickController(0, -1f, true);
            }

            Assert.Throws<Exception>(CreateNewController, "Should have throw error.");
        }

        [Test]
        public void Should_Throw_Error_Updating_Controller_With_0_Duration()
        {
            var controller = new TimeTickController(0, 1, true);

            void UpdateControllerDuration()
            {
                controller.UpdateTickDuration(0);
            }

            Assert.Throws<Exception>(UpdateControllerDuration, "Should have throw error.");
        }

        [Test]
        public void Should_Throw_Error_Updating_Controller_Less_Than_0_Duration()
        {
            var controller = new TimeTickController(0, 1f, true);

            void UpdateControllerDuration()
            {
                controller.UpdateTickDuration(-1f);
            }

            Assert.Throws<Exception>(UpdateControllerDuration, "Should have throw error.");
        }

        [Test]
        public void Should_Update_Tick_Duration()
        {
            var controller = new TimeTickController(0, 1, true);
            float newDuration = 0.5f;
            controller.UpdateTickDuration(newDuration);

            Assert.AreEqual(newDuration, controller.TickDuration);
        }

        [Test]
        public void Should_Time_Increase_Always_Greater_Than_0()
        {
            var controller = new TimeTickController(0, 1, true);
            float timeToSubtract = -0.5f;
            controller.UpdateTimer(timeToSubtract);

            Assert.LessOrEqual(0, controller.TickTimer);
        }

        [Test]
        public void Should_Reset_After_Updated_Tick_Duration_If_Exceeded()
        {
            var controller = new TimeTickController(0.6f, 1, true);
            float newDuration = 0.5f;
            controller.UpdateTickDuration(newDuration);

            Assert.AreEqual(0, controller.TickTimer);
        }

        [Test]
        public void Should_Notify_On_Update_Timer_Per_Duration_Exceeded()
        {
            foreach (var totalTime in _totalTimesToTest)
            {
                foreach (var timeToIncrease in _timesToAddToExceedTotalTimeOf1Second)
                {
                    int callCount = 0;
                    var controller = new TimeTickController(0, totalTime, true);
                    controller.OnTimeTick += () => callCount++;
                    int expectedResult = 0;
                    var temp = timeToIncrease;

                    while (temp.IsExceeded(totalTime))
                    {
                        temp -= totalTime;
                        expectedResult++;
                    }

                    controller.UpdateTimer(timeToIncrease);

                    Assert.That(callCount, Is.EqualTo(expectedResult),
                        $"increase: {timeToIncrease}, total: {totalTime}, timer: {controller.TickTimer}");
                }
            }
        }

        [Test]
        public void Should_Notify_Only_Once_On_Update_Duration_If_Duration_Exceeded()
        {
            foreach (var totalTime in _timesToAddToExceedTotalTimeOf1Second)
            {
                foreach (var newDuration in _totalTimesToTest)
                {
                    int callCount = 0;
                    float startTime = totalTime / 2;
                    var controller = new TimeTickController(totalTime / 2, totalTime, true);
                    controller.OnTimeTick += () => callCount++;
                    int expectedResult = startTime >= newDuration ? 1 : 0;

                    controller.UpdateTickDuration(newDuration);

                    Assert.That(callCount, Is.EqualTo(expectedResult),
                        $"newDuration: {newDuration}, total: {totalTime}, timer: {controller.TickTimer}");
                }
            }
        }

        [Test]
        public void Should_Notify_Only_Once_Hence_We_UnSubscribe_After_Notified_After_Once()
        {
            int callCount = 0;
            int totalTime = 1;
            var controller = new TimeTickController(0, totalTime, true);
            controller.OnTimeTick += () => callCount++;
            int expectedResult = 1;

            controller.UpdateTimer(totalTime);
            controller.RemoveAllListeners();
            controller.UpdateTimer(totalTime);

            Assert.That(callCount, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Should_Non_Automated_Controllers_Not_Renew_Timer_After_Duration_Update()
        {
            foreach (var totalTime in _timesToAddToExceedTotalTimeOf1Second)
            {
                foreach (var newDuration in _totalTimesToTest)
                {
                    int callCount = 0;
                    float startTime = totalTime / 2;
                    var controller = new TimeTickController(startTime, totalTime, false);
                    controller.OnTimeTick += () => callCount++;
                    int expectedResult = 0;

                    controller.UpdateTickDuration(newDuration);

                    Assert.That(callCount, Is.EqualTo(expectedResult),
                        $"newDuration: {newDuration}, total: {totalTime}, timer: {controller.TickTimer}");
                }
            }
        }

        [Test]
        public void Should_Non_Automated_Controllers_Not_Renew_Timer_After_Time_Update()
        {
            foreach (var totalTime in _totalTimesToTest)
            {
                foreach (var timeToIncrease in _timesToAddToExceedTotalTimeOf1Second)
                {
                    int callCount = 0;
                    float startTime = totalTime / 2;
                    var controller = new TimeTickController(startTime, totalTime, false);
                    controller.OnTimeTick += () => callCount++;
                    int expectedResult = 0;

                    controller.UpdateTimer(timeToIncrease);

                    Assert.That(callCount, Is.EqualTo(expectedResult),
                        $"timer: {controller.TickTimer}, total: {totalTime}, invoked time: {callCount}");
                }
            }
        }

        [Test]
        public void Should_Non_Automated_Controller_Not_Renew_Timer_If_Time_Not_Exceeds_After_Time_Update()
        {
            var controller = new TimeTickController(0, 1, false);
            int callCount = 0;
            controller.OnTimeTick += () => callCount++;
            int expectedResult = 0;

            controller.UpdateTimer(0.2f);
            controller.RenewTimer();

            Assert.That(callCount, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Should_Non_Automated_Controller_Not_Renew_Timer_If_Time_Not_Exceeds_After_Duration_Update()
        {
            var controller = new TimeTickController(0, 1, false);
            int callCount = 0;
            controller.OnTimeTick += () => callCount++;
            int expectedResult = 0;

            controller.UpdateTickDuration(0.5f);
            controller.RenewTimer();

            Assert.That(callCount, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Should_Non_Automated_Controller_Renew_Timer_If_Time_Exceeds_After_Time_Update()
        {
            var controller = new TimeTickController(0, 1, false);
            int callCount = 0;
            controller.OnTimeTick += () => callCount++;
            int expectedResult = 1;

            controller.UpdateTimer(1.5f);
            controller.RenewTimer();

            Assert.That(callCount, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Should_Non_Automated_Controller_Renew_Timer_If_Time_Exceeds_After_Duration_Update()
        {
            var controller = new TimeTickController(0.5f, 1, false);
            int callCount = 0;
            controller.OnTimeTick += () => callCount++;
            int expectedResult = 1;

            controller.UpdateTickDuration(0.2f);
            controller.RenewTimer();

            Assert.That(callCount, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Should_Set_Automation_Renew_Timer_After_Next_Time_Update()
        {
            var controller = new TimeTickController(0.5f, 1, false);
            int callCount = 0;
            controller.OnTimeTick += () => callCount++;
            int expectedResult = 1;

            controller.UpdateTimer(1f);
            controller.SetAutomation(true);
            controller.UpdateTimer(0.1f);

            Assert.That(callCount, Is.EqualTo(expectedResult));
        }
    }
}