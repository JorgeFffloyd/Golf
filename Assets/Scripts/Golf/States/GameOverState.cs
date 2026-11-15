
using System;
using Golf.Golf.Source;
using Golf.Golf.States.Interfaces;
using Golf.Golf.View;

namespace Golf.Golf.States
{
    public class GameOverState : IGameState
    {
        private readonly ScoreService m_score;
        private readonly GameOverView m_view;
        private readonly Action m_returnToMenu;

        public GameOverState(
            ScoreService score,
            GameOverView view,
            Action returnToMenu)
        {
            m_score = score;
            m_view = view;
            m_returnToMenu = returnToMenu;
        }

        public void Enter()
        {
            m_score.SaveRecord();

            m_view.Show();
            m_view.SetMenuAction(m_returnToMenu);
        }

        public void Exit()
        {
            m_view.Hide();
        }
    }
}
