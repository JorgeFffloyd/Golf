namespace Golf.Golf.Source
{
    public class HighScoreStorage
    {
        private const string Key = "HighScore";

        public int Load()
        {
            return UnityEngine.PlayerPrefs.GetInt(Key, 0);
        }

        public void Save(int score)
        {
            if (score > Load())
            {
                UnityEngine.PlayerPrefs.SetInt(Key, score);
                UnityEngine.PlayerPrefs.Save();
            }
        }
    }
}