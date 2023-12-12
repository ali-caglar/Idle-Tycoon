using System;
using System.IO;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using Save.DataServices;
using UnityEngine;
using Logger = Utility.Logger;

namespace Save
{
    /// <summary>
    /// Saves, loads and deletes all data in the game
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class DataManager<T>
    {
        /// <summary>
        /// Path name of the save directory
        /// </summary>
        private static readonly string DataPath = $"{Application.companyName}/{Application.productName}/";

        /// <summary>
        /// Directory name of the saves
        /// </summary>
        private static readonly string FolderName = typeof(T).Name;

        /// <summary>
        /// Json serialize service
        /// </summary>
        private static readonly ISerializationService SerializationService = new NewtonsoftSerializationService();

        /// <summary>
        /// Save data to a file (overwrite completely)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="uniqueID"></param>
        public static void Save(T data, string uniqueID)
        {
            // get the data path of this save data
            string dataPath = GetFilePath(FolderName, uniqueID);

            string jsonData = SerializationService.ConvertToJson(data);
            byte[] byteData = Encoding.UTF8.GetBytes(jsonData);

            // create the file in the path if it doesn't exist
            // if the file path or name does not exist, return the default SO
            if (!Directory.Exists(Path.GetDirectoryName(dataPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(dataPath));
            }

            // attempt to save here data
            try
            {
                // save data here
                File.WriteAllBytes(dataPath, byteData);
                Logger.Log(LogType.Log, "Save data to: " + dataPath);
#if UNITY_EDITOR
                // refreshing unity to show files
                UnityEditor.AssetDatabase.Refresh();
#endif
            }
            catch (Exception e)
            {
                // write out error here
                Logger.Log(LogType.Error, "Failed to save data to: " + dataPath);
                Logger.Log(LogType.Error, "Error " + e.Message);
            }
        }

        /// <summary>
        /// Save data to a file (overwrite completely)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="uniqueID"></param>
        /// <param name="cancellationTokenSource"></param>
        public static async UniTask SaveAsync(T data, string uniqueID, CancellationTokenSource cancellationTokenSource)
        {
            if (cancellationTokenSource == null || cancellationTokenSource.IsCancellationRequested) return;

            // get the data path of this save data
            string dataPath = GetFilePath(FolderName, uniqueID);

            string jsonData = SerializationService.ConvertToJson(data);
            byte[] byteData = Encoding.UTF8.GetBytes(jsonData);

            // create the file in the path if it doesn't exist
            // if the file path or name does not exist, return the default SO
            if (!Directory.Exists(Path.GetDirectoryName(dataPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(dataPath));
            }

            // attempt to save here data
            try
            {
                // save data here
                await File.WriteAllBytesAsync(dataPath, byteData, cancellationTokenSource.Token).AsUniTask();
                Logger.Log(LogType.Log, "Save data to: " + dataPath);
#if UNITY_EDITOR
                // refreshing unity to show files
                UnityEditor.AssetDatabase.Refresh();
#endif
            }
            catch (Exception e)
            {
                // write out error here
                Logger.Log(LogType.Error, "Failed to save data to: " + dataPath);
                Logger.Log(LogType.Error, "Error " + e.Message);
            }
        }

        /// <summary>
        /// Load all data at a specified file and folder location
        /// </summary>
        /// <returns></returns>
        public static T Load(string uniqueID)
        {
            // get the data path of this save data
            string dataPath = GetFilePath(FolderName, uniqueID);

            // if the file path or name does not exist, return the default SO
            if (!Directory.Exists(Path.GetDirectoryName(dataPath)))
            {
                Logger.Log(LogType.Warning, "File or path does not exist! " + dataPath);
                return default(T);
            }

            // load in the save data as byte array
            byte[] jsonDataAsBytes;

            try
            {
                jsonDataAsBytes = File.ReadAllBytes(dataPath);
                Logger.Log(LogType.Log, "<color=green>Loaded all data from: </color>" + dataPath);
            }
            catch (Exception e)
            {
                Logger.Log(LogType.Warning, "Failed to load data from: " + dataPath);
                Logger.Log(LogType.Warning, "Error: " + e.Message);
                return default(T);
            }

            // convert the byte array to json
            string jsonData = Encoding.UTF8.GetString(jsonDataAsBytes);

            // convert to the specified object type
            T returnedData = SerializationService.ConvertFromJson<T>(jsonData);

            // return the casted json object to use
            return (T)Convert.ChangeType(returnedData, typeof(T));
        }

        /// <summary>
        /// Load all data at a specified file and folder location
        /// </summary>
        /// <returns></returns>
        public static async UniTask<T> LoadAsync(string uniqueID, CancellationTokenSource cancellationTokenSource)
        {
            if (cancellationTokenSource == null || cancellationTokenSource.IsCancellationRequested) return default(T);

            // get the data path of this save data
            string dataPath = GetFilePath(FolderName, uniqueID);

            // if the file path or name does not exist, return the default SO
            if (!Directory.Exists(Path.GetDirectoryName(dataPath)))
            {
                Logger.Log(LogType.Warning, "File or path does not exist! " + dataPath);
                return default(T);
            }

            // load in the save data as byte array
            byte[] jsonDataAsBytes;

            try
            {
                jsonDataAsBytes = await File.ReadAllBytesAsync(dataPath, cancellationTokenSource.Token).AsUniTask();
                Logger.Log(LogType.Log, "<color=green>Loaded all data from: </color>" + dataPath);
            }
            catch (Exception e)
            {
                Logger.Log(LogType.Warning, "Failed to load data from: " + dataPath);
                Logger.Log(LogType.Warning, "Error: " + e.Message);
                return default(T);
            }

            // convert the byte array to json
            string jsonData = Encoding.UTF8.GetString(jsonDataAsBytes);

            // convert to the specified object type
            T returnedData = SerializationService.ConvertFromJson<T>(jsonData);

            // return the casted json object to use
            return (T)Convert.ChangeType(returnedData, typeof(T));
        }

        /// <summary>
        /// Deletes all data underneath save folder
        /// </summary>
        public static void ClearAll()
        {
            // get the data path of save folder
            string dataPath = GetFilePath("");

            // if the path not exist, exit
            if (!Directory.Exists(Path.GetDirectoryName(dataPath)))
            {
                return;
            }

            try
            {
                DirectoryInfo folderToDelete = new DirectoryInfo(dataPath);
                folderToDelete.Delete(true);
                Logger.Log(LogType.Log, "<color=red>Deleted all data under: </color>" + dataPath);
            }
            catch (Exception e)
            {
                Logger.Log(LogType.Warning, "Failed to delete data under: " + dataPath);
                Logger.Log(LogType.Warning, "Error: " + e.Message);
                throw;
            }

#if UNITY_EDITOR
            // refreshing unity to update files
            UnityEditor.AssetDatabase.Refresh();
#endif
        }

        /// <summary>
        /// Create file path for where a file is stored on the specific platform given a folder name and file name
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static string GetFilePath(string folderName, string fileName = "")
        {
            // ReSharper disable once JoinDeclarationAndInitializer
            string filePath;
#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
            // mac
            filePath = Path.Combine(Application.streamingAssetsPath, (DataPath + folderName));

            if (fileName != "")
                filePath = Path.Combine(filePath, (fileName + ".txt"));
#elif UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        // windows
        filePath = Path.Combine(Application.persistentDataPath, (DataPath + folderName));

        if(fileName != "")
            filePath = Path.Combine(filePath, (fileName + ".txt"));
#elif UNITY_ANDROID
        // android
        filePath = Path.Combine(Application.persistentDataPath, (DataPath + folderName));

        if(fileName != "")
            filePath = Path.Combine(filePath, (fileName + ".txt"));
#elif UNITY_IOS
        // ios
        filePath = Path.Combine(Application.persistentDataPath, (DataPath + folderName));

        if(fileName != "")
            filePath = Path.Combine(filePath, (fileName + ".txt"));
#else
            filePath = Path.Combine(Application.streamingAssetsPath, (DataPath + folderName));

            if (fileName != "")
                filePath = Path.Combine(filePath, (fileName + ".txt"));
#endif
            return filePath;
        }
    }
}