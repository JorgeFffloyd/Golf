using Golf.Golf.Source;
using Golf.Golf.States.Interfaces;
using UnityEngine;

namespace Golf.Golf.States
{
    public class GameOverState : IGameState, IUpdatable
    {
        private readonly ScoreService m_score;
        private readonly System.Action m_returnToMenu;

        private float m_timer;
        private const float WaitTime = 1.5f;

        public GameOverState(
            ScoreService score,
            System.Action returnToMenu)
        {
            m_score = score;
            m_returnToMenu = returnToMenu;
        }

        public void Enter()
        {
            m_score.SaveRecord();
            m_timer = WaitTime;
        }

        public void Exit()
        {
        }

        public void Update(float deltaTime)
        {
            m_timer -= deltaTime;

            if (m_timer <= 0f)
                m_returnToMenu?.Invoke();
        }
    }
}