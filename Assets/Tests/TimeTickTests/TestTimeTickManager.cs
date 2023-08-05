using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Installers;
using NUnit.Framework;
using TimeTick;
using Zenject;

namespace Tests.TimeTickTests
{
    [TestFixture]
    public class TestTimeTickManager : ZenjectUnitTestFixture
    {
        private const string SettingsInstallerPath = "Installers/SettingsInstaller";

        private List<TimeTickControllerData> _tickControllerDatas;
        private TimeTickControllerData _defaultControllerData;

        [SetUp]
        public void BindSettings()
        {
            Container.BindInterfacesAndSelfTo(typeof(TimeTickManager)).AsTransient();
            SettingsInstaller.InstallFromResource(SettingsInstallerPath, Container);

            _defaultControllerData = new TimeTickControllerData
            {
                isAutomated = true,
                tickTimer = 0,
                tickDuration = 3,
                timeIdentifier = TimeTickIdentifier.Custom
            };
        }

        [Test]
        public void Should_Have_All_Default_Controllers_Except_CustomIdentifier()
        {
            var manager = Container.Resolve<TimeTickManager>();

            foreach (TimeTickIdentifier identifier in Enum.GetValues(typeof(TimeTickIdentifier)))
            {
                if (identifier == TimeTickIdentifier.Custom)
                {
                    Assert.False(manager.GetPreDefinedTickController(identifier, out TimeTickController tickController),
                        "Custom identifier shouldn't be added to the list on construction");
                }
                else
                {
                    Assert.True(manager.GetPreDefinedTickController(identifier, out TimeTickController tickController),
                        $"There's missing identifier: {identifier.ToString()}");
                }
            }
        }

        [Test]
        public void Should_Default_Controllers_Initiated_As_Automated()
        {
            var manager = Container.Resolve<TimeTickManager>();

            foreach (var tickController in manager.TimeTickControllers)
            {
                if (tickController.TimeIdentifier != TimeTickIdentifier.Custom)
                {
                    Assert.AreEqual(true, tickController.IsAutomated,
                        "Default controllers has to be automated.");
                }
            }
        }

        [Test]
        public void Should_Have_Correct_Amount_Controller()
        {
            var manager = Container.Resolve<TimeTickManager>();
            int totalIdentifierCount = Enum.GetValues(typeof(TimeTickIdentifier)).Length;
            int expectedCount = totalIdentifierCount - 1; // extracting the custom identifier

            Assert.AreEqual(expectedCount, manager.TimeTickControllers.Count,
                "Identifier count and manager's default controller's count not the same");
        }

        [Test]
        public void Should_Add_New_Unique_Controller()
        {
            var manager = Container.Resolve<TimeTickManager>();

            TimeTickController newController = new TimeTickController(_defaultControllerData);
            manager.AddNewCustomTickController(newController);

            Assert.Contains(newController, manager.TimeTickControllers,
                "Custom controller couldn't added for no reason");
        }

        [Test]
        public void Should_Remove_Existed_Controller()
        {
            var manager = Container.Resolve<TimeTickManager>();

            TimeTickController newController = new TimeTickController(_defaultControllerData);
            manager.AddNewCustomTickController(newController);
            manager.RemoveCustomTickController(newController);

            Assert.False(manager.TimeTickControllers.Contains(newController),
                "Custom controller couldn't be removed for no reason");
        }

        [Test]
        public void Can_Not_Add_NonCustomIdentifier_Controller()
        {
            var manager = Container.Resolve<TimeTickManager>();

            foreach (TimeTickIdentifier identifier in Enum.GetValues(typeof(TimeTickIdentifier)))
            {
                if (identifier == TimeTickIdentifier.Custom) continue;

                var controllerData = _defaultControllerData;
                controllerData.timeIdentifier = identifier;
                TimeTickController newController = new TimeTickController(controllerData);
                manager.AddNewCustomTickController(newController);

                int identifierCountInList = manager.TimeTickControllers.Count(x => x.TimeIdentifier == identifier);
                Assert.AreEqual(1, identifierCountInList, "There can only 1 controller for the same identifier");
            }
        }

        [Test]
        public void Can_Not_Remove_NonCustomIdentifier_Controller()
        {
            var manager = Container.Resolve<TimeTickManager>();

            foreach (TimeTickController controller in manager.TimeTickControllers)
            {
                if (controller.TimeIdentifier == TimeTickIdentifier.Custom) continue;

                manager.RemoveCustomTickController(controller);

                Assert.Contains(controller, manager.TimeTickControllers,
                    "Non Custom identified controllers can't be removed");
            }
        }

        [Test]
        public void Should_Not_Add_Same_Controller()
        {
            var manager = Container.Resolve<TimeTickManager>();

            TimeTickController newController = new TimeTickController(_defaultControllerData);
            manager.AddNewCustomTickController(newController);
            manager.AddNewCustomTickController(newController);
            manager.AddNewCustomTickController(newController);

            int newControllerCountInList = manager.TimeTickControllers.Count(x => x == newController);
            Assert.AreEqual(1, newControllerCountInList, "You can't add the same element");
        }

        [Test]
        public void Should_Update_All_Controllers_Timer()
        {
            var manager = Container.Resolve<TimeTickManager>();

            float timeToIncrease = 0.01f;
            manager.UpdateTimers(timeToIncrease);

            foreach (TimeTickController controller in manager.TimeTickControllers)
            {
                if (controller.TimeIdentifier == TimeTickIdentifier.Custom) continue;

                Assert.AreEqual(timeToIncrease, controller.TickTimer,
                    "All the timers should be updated");
            }
        }

        [Test]
        public void Should_PreDefined_Controllers_Always_Automated()
        {
            var manager = Container.Resolve<TimeTickManager>();

            float timeToIncrease = 1.5f;
            manager.UpdateTimers(timeToIncrease);

            foreach (TimeTickController controller in manager.TimeTickControllers)
            {
                if (controller.TimeIdentifier == TimeTickIdentifier.Custom) continue;
                controller.SetAutomation(false);

                Assert.True(controller.IsAutomated, "Default controllers has to be automated.");
            }
        }
    }
}