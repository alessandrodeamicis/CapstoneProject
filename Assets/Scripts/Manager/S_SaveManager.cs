using Newtonsoft.Json;
using System.IO;
using UnityEngine;

namespace SGM
{
    [System.Serializable]
    public class TutorialData
    {
        [JsonProperty] public bool IsDone;
    }

    public static class S_SaveManager
    {

        private static string _pathTutorial = "Assets/Saves/tutorial.txt";

        public static void TutorialDone()
        {
            TutorialData data = new TutorialData();
            data.IsDone = true;

            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(_pathTutorial, json);
        }

        public static bool IsTutorialDone()
        {
            if (File.Exists(_pathTutorial))
            {
                string json = File.ReadAllText(_pathTutorial);
                TutorialData data = JsonConvert.DeserializeObject<TutorialData>(json);

                return data.IsDone;
            }
            else { return false; }
        }

    }
}
