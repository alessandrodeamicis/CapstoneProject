using Newtonsoft.Json;
using System.IO;
using UnityEngine;

namespace SGM
{
    [System.Serializable]
    public class SaveData
    {
        //[JsonProperty] private Transform _playerPosition;

        [JsonProperty] private int _currentLevel;
        [JsonProperty] private int _characterChosen;
        //[JsonProperty] private Weapon _weapon;
    }
    public static class S_SaveManager
    {

        private static string _pathSaveData = Application.persistentDataPath + "/save-data.txt";

        public static SaveData GetSaveData()
        {
            if (File.Exists(_pathSaveData))
            {
                try
                {
                    string stringSaveData;
                    SaveData saveData;
                    stringSaveData = File.ReadAllText(_pathSaveData);
                    saveData = JsonConvert.DeserializeObject<SaveData>(stringSaveData);
                    return saveData;
                }
                catch
                {
                    Debug.LogError("Qualcosa è andato storto.");
                    return null;
                }
            }
            else { return null; }
        }

        public static void SaveSaveData()
        {
            SaveData saveData;
            saveData = GetSaveData();

            string stringLeaderboard = JsonConvert.SerializeObject(saveData, Formatting.Indented);
            File.WriteAllText(_pathSaveData, stringLeaderboard);
        }
    }
}
