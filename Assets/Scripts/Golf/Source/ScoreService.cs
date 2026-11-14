using System;


namespace Golf.Golf.Source
{
    public class ScoreService
    {
        public int CurrentScore { get; private set; }

        private readonly HighScoreStorage m_storage;

        public event Action<int> OnScoreChanged;

        public ScoreService(HighScoreStorage storage)
        {
            m_storage = storage;
        }

        public int HighScore => m_storage.Load();

        public void Reset()
        {
            CurrentScore = 0;
            OnScoreChanged?.Invoke(CurrentScore);
        }

        public void AddPoint()
        {
            CurrentScore++;
            OnScoreChanged?.Invoke(CurrentScore);
        }

        public void SaveRecord()
        {
            m_storage.Save(CurrentScore);
        }
    }
}