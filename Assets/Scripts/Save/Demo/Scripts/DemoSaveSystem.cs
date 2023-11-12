using System.Text;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Save.Demo.Scripts
{
    public class DemoSaveSystem : MonoBehaviour
    {
        public DemoSaveData demoSaveData1;
        public TextMeshProUGUI demoSaveData1TMP;
        public DemoSaveData demoSaveData2;
        public TextMeshProUGUI demoSaveData2TMP;
        public DemoSaveDataWithUserData demoSaveWithUserData1;
        public TextMeshProUGUI demoSaveDataModel1TMP;
        public TextMeshProUGUI demoSaveUserDataModel1TMP;
        public Button changeUserData1Button;
        public DemoSaveDataWithUserData demoSaveWithUserData2;
        public TextMeshProUGUI demoSaveDataModel2TMP;
        public TextMeshProUGUI demoSaveUserDataModel2TMP;
        public Button changeUserData2Button;

        [Header("Async")]
        public DemoSaveAndLoadAsyncSaveDataWithSaveAndLoadUserData demoSaveAndLoadAsyncSaveWithSaveAndLoadUserData;
        public TextMeshProUGUI demoAsyncSaveDataModelTMP;
        public Button[] demoAsyncSaveDataButtons;

        private StringBuilder _sb;

        private void Awake()
        {
            UpdateText(demoSaveData1TMP, demoSaveData1);
            UpdateText(demoSaveData2TMP, demoSaveData2);
            UpdateText(demoSaveDataModel1TMP, demoSaveUserDataModel1TMP, changeUserData1Button, demoSaveWithUserData1);
            UpdateText(demoSaveDataModel2TMP, demoSaveUserDataModel2TMP, changeUserData2Button, demoSaveWithUserData2);

            _sb = new StringBuilder();
            UpdateText(_sb, demoAsyncSaveDataModelTMP, demoSaveAndLoadAsyncSaveWithSaveAndLoadUserData);
        }

        private async void Start()
        {
            await demoSaveAndLoadAsyncSaveWithSaveAndLoadUserData.InitializeAsync();
            var unityEvent = UniTask.UnityAction(async () => { await IncreaseAttemptAndSave(); });
            foreach (var button in demoAsyncSaveDataButtons)
            {
                button.onClick.AddListener(unityEvent);
            }
        }

        private async UniTask IncreaseAttemptAndSave()
        {
            for (int i = 0; i < 1000; i++)
            {
                demoSaveAndLoadAsyncSaveWithSaveAndLoadUserData.UserData.attemptCount++;
                await demoSaveAndLoadAsyncSaveWithSaveAndLoadUserData.SaveUserData();
                UpdateText(_sb, demoAsyncSaveDataModelTMP, demoSaveAndLoadAsyncSaveWithSaveAndLoadUserData);
                Debug.Log(demoSaveAndLoadAsyncSaveWithSaveAndLoadUserData.UserData.attemptCount);
            }
        }

        private void UpdateText(TextMeshProUGUI tmp, DemoSaveData saveData)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(saveData.DataModel.ID.uniqueID);
            sb.AppendLine();
            sb.Append(saveData.DataModel.ID.worldName.ToString());
            sb.AppendLine();
            sb.Append(saveData.DataModel.ID.regionName.ToString());
            sb.AppendLine();
            sb.Append("year: " + saveData.DataModel.year);
            sb.AppendLine();
            sb.Append("alias: " + saveData.DataModel.alias);
            sb.AppendLine();
            sb.Append("isWorking: " + saveData.DataModel.isWorking);
            sb.AppendLine();
            sb.Append("timer: " + saveData.DataModel.timer);
            sb.AppendLine();
            sb.Append("unlock price: " + saveData.DataModel.baro.unlockPrice);
            sb.AppendLine();

            tmp.SetText(sb.ToString());
        }

        private void UpdateText(TextMeshProUGUI tmp, TextMeshProUGUI userDataTmp, Button button,
            DemoSaveDataWithUserData saveData)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(saveData.DataModel.ID.uniqueID);
            sb.AppendLine();
            sb.Append(saveData.DataModel.ID.worldName.ToString());
            sb.AppendLine();
            sb.Append(saveData.DataModel.ID.regionName.ToString());
            sb.AppendLine();
            sb.Append("birthPlace: " + saveData.DataModel.birthPlace);
            sb.AppendLine();
            sb.Append("age: " + saveData.DataModel.age);
            sb.AppendLine();
            sb.Append("initialMoney: " + saveData.DataModel.initialMoney);
            sb.AppendLine();

            tmp.SetText(sb.ToString());

            UpdateUserDataText();

            button.onClick.AddListener(() =>
            {
                saveData.UserData.currentMoney += 100;
                saveData.SaveUserData();
                UpdateUserDataText();
            });

            void UpdateUserDataText()
            {
                sb.Clear();
                sb.Append(saveData.UserData.ID.uniqueID);
                sb.AppendLine();
                sb.Append(saveData.UserData.ID.worldName.ToString());
                sb.AppendLine();
                sb.Append(saveData.UserData.ID.regionName.ToString());
                sb.AppendLine();
                sb.Append("userName: " + saveData.UserData.userName);
                sb.AppendLine();
                sb.Append("currentMoney: " + saveData.UserData.currentMoney);
                sb.AppendLine();
                userDataTmp.SetText(sb.ToString());
            }
        }

        private void UpdateText(StringBuilder sb, TextMeshProUGUI tmp, DemoSaveAndLoadAsyncSaveDataWithSaveAndLoadUserData saveData)
        {
            sb.Clear();
            sb.Append(saveData.DataModel.ID.uniqueID);
            sb.AppendLine();
            sb.Append(saveData.DataModel.ID.worldName.ToString());
            sb.AppendLine();
            sb.Append(saveData.DataModel.ID.regionName.ToString());
            sb.AppendLine();
            sb.Append("alias: " + saveData.DataModel.alias);
            sb.AppendLine();
            sb.Append("age: " + saveData.DataModel.age);
            sb.AppendLine();
            sb.Append("isWorking: " + saveData.UserData.attemptCount);
            sb.AppendLine();

            tmp.SetText(sb.ToString());
        }
    }
}