using System;
using Golf.Golf.Source;
using Golf.Golf.States.Interfaces;
using Golf.Golf.View;

namespace Golf.Golf.States
{
    public class MainMenuState : IGameState
    {
        private readonly MainMenuView m_view;
        private readonly ScoreService m_score;
        private readonly Action m_startGame;

        public MainMenuState(
            MainMenuView view,
            ScoreService score,
            Action startGame)
        {
            m_view = view;
            m_score = score;
            m_startGame = startGame;
        }

        public void Enter()
        {
            m_view.Show();
            m_view.SetPlayAction(m_startGame);
        }

        public void Exit()
        {
            m_view.Hide();
        }
    }
}