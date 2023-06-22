using System;
using System.Linq;
using Enums;
using NUnit.Framework;
using TimeTick;

namespace Tests
{
    public class test_time_tick_manager
    {
        [Test]
        public void Should_Have_All_Default_Controllers_Except_CustomIdentifier()
        {
            var manager = new TimeTickManager();

            foreach (TimeTickIdentifier identifier in Enum.GetValues(typeof(TimeTickIdentifier)))
            {
                if (identifier == TimeTickIdentifier.Custom)
                {
                    Assert.False(manager.GetTickController(identifier, out TimeTickController tickController),
                        "Custom identifier shouldn't be added to the list on construction");
                }
                else
                {
                    Assert.True(manager.GetTickController(identifier, out TimeTickController tickController),
                        $"There's missing identifier: {identifier.ToString()}");
                }
            }
        }

        [Test]
        public void Should_Have_Correct_Amount_Controller()
        {
            var manager = new TimeTickManager();
            int totalIdentifierCount = Enum.GetValues(typeof(TimeTickIdentifier)).Length;
            int expectedCount = totalIdentifierCount - 1; // extracting the custom identifier

            Assert.AreEqual(expectedCount, manager.TimeTickControllers.Count,
                "Identifier count and manager's default controller's count not the same");
        }

        [Test]
        public void Should_Add_New_Unique_Controller()
        {
            var manager = new TimeTickManager();

            TimeTickController newController = new TimeTickController(TimeTickIdentifier.Custom, 0, 3);
            manager.AddNewTickController(newController);

            Assert.Contains(newController, manager.TimeTickControllers,
                "Custom controller couldn't added for no reason");
        }

        [Test]
        public void Should_Remove_Existed_Controller()
        {
            var manager = new TimeTickManager();

            TimeTickController newController = new TimeTickController(TimeTickIdentifier.Custom, 0, 3);
            manager.AddNewTickController(newController);
            manager.RemoveTickController(newController);

            Assert.False(manager.TimeTickControllers.Contains(newController),
                "Custom controller couldn't be removed for no reason");
        }

        [Test]
        public void Can_Not_Add_NonCustomIdentifier_Controller()
        {
            var manager = new TimeTickManager();

            foreach (TimeTickIdentifier identifier in Enum.GetValues(typeof(TimeTickIdentifier)))
            {
                if (identifier == TimeTickIdentifier.Custom) continue;

                TimeTickController newController = new TimeTickController(identifier, 0, 3);
                manager.AddNewTickController(newController);

                int identifierCountInList = manager.TimeTickControllers.Count(x => x.TimeIdentifier == identifier);
                Assert.AreEqual(1, identifierCountInList, "There can only 1 controller for the same identifier");
            }
        }

        [Test]
        public void Can_Not_Remove_NonCustomIdentifier_Controller()
        {
            var manager = new TimeTickManager();

            foreach (TimeTickController controller in manager.TimeTickControllers)
            {
                if (controller.TimeIdentifier == TimeTickIdentifier.Custom) continue;

                manager.RemoveTickController(controller);

                Assert.Contains(controller, manager.TimeTickControllers,
                    "Non Custom identified controllers can't be removed");
            }
        }

        [Test]
        public void Should_Not_Add_Same_Controller()
        {
            var manager = new TimeTickManager();

            TimeTickController newController = new TimeTickController(TimeTickIdentifier.Custom, 0, 3);
            manager.AddNewTickController(newController);
            manager.AddNewTickController(newController);
            manager.AddNewTickController(newController);

            int newControllerCountInList = manager.TimeTickControllers.Count(x => x == newController);
            Assert.AreEqual(1, newControllerCountInList, "You can't add the same element");
        }

        [Test]
        public void Should_Update_All_Controllers_Timer()
        {
            var manager = new TimeTickManager();

            float timeToIncrease = 0.01f;
            manager.UpdateTimers(timeToIncrease);
            
            foreach (TimeTickController controller in manager.TimeTickControllers)
            {
                if (controller.TimeIdentifier == TimeTickIdentifier.Custom) continue;

                Assert.AreEqual(timeToIncrease, controller.TickTimer,
                    "All the timers should be updated");
            }
        }
    }
}