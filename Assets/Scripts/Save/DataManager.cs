using System;
using System.IO;
using System.Text;
using Save.DataServices;
using UnityEngine;

namespace Save
{
    /// <summary>
    /// Saves, loads and deletes all data in the game
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class DataManager<T>
    {
        /// <summary>
        /// Json serialize service
        /// </summary>
        private static readonly IDataService DataService = new NewtonsoftDataService();

        /// <summary>
        /// Directory name of the saves
        /// </summary>
        private const string FolderName = "Saves";

        /// <summary>
        /// File name of the save
        /// </summary>
        private static readonly string FileName = typeof(T).Name;

        /// <summary>
        /// Save data to a file (overwrite completely)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        public static void Save(T data)
        {
            // get the data path of this save data
            string dataPath = GetFilePath(FolderName, FileName);

            string jsonData = DataService.ConvertToJson(data);
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
                Debug.Log("Save data to: " + dataPath);
#if UNITY_EDITOR
                // refreshing unity to show files
                UnityEditor.AssetDatabase.Refresh();
#endif
            }
            catch (Exception e)
            {
                // write out error here
                Debug.LogError("Failed to save data to: " + dataPath);
                Debug.LogError("Error " + e.Message);
            }
        }

        /// <summary>
        /// Load all data at a specified file and folder location
        /// </summary>
        /// <returns></returns>
        public static T Load()
        {
            // get the data path of this save data
            string dataPath = GetFilePath(FolderName, FileName);

            // if the file path or name does not exist, return the default SO
            if (!Directory.Exists(Path.GetDirectoryName(dataPath)))
            {
                Debug.LogWarning("File or path does not exist! " + dataPath);
                return default(T);
            }

            // load in the save data as byte array
            byte[] jsonDataAsBytes;

            try
            {
                jsonDataAsBytes = File.ReadAllBytes(dataPath);
                Debug.Log("<color=green>Loaded all data from: </color>" + dataPath);
            }
            catch (Exception e)
            {
                Debug.LogWarning("Failed to load data from: " + dataPath);
                Debug.LogWarning("Error: " + e.Message);
                return default(T);
            }

            // convert the byte array to json
            string jsonData = Encoding.UTF8.GetString(jsonDataAsBytes);

            // convert to the specified object type
            T returnedData = DataService.ConvertFromJson<T>(jsonData);

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
                Debug.Log("<color=green>Deleted all data under: </color>" + dataPath);
            }
            catch (Exception e)
            {
                Debug.LogWarning("Failed to delete data under: " + dataPath);
                Debug.LogWarning("Error: " + e.Message);
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
            filePath = Path.Combine(Application.streamingAssetsPath, ("data/" + folderName));

            if (fileName != "")
                filePath = Path.Combine(filePath, (fileName + ".txt"));
#elif UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        // windows
        filePath = Path.Combine(Application.persistentDataPath, ("data/" + folderName));

        if(fileName != "")
            filePath = Path.Combine(filePath, (fileName + ".txt"));
#elif UNITY_ANDROID
        // android
        filePath = Path.Combine(Application.persistentDataPath, ("data/" + folderName));

        if(fileName != "")
            filePath = Path.Combine(filePath, (fileName + ".txt"));
#elif UNITY_IOS
        // ios
        filePath = Path.Combine(Application.persistentDataPath, ("data/" + folderName));

        if(fileName != "")
            filePath = Path.Combine(filePath, (fileName + ".txt"));
#endif
            return filePath;
        }
    }
}