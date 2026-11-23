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
        private readonly Action m_openShop;

        public MainMenuState(
            MainMenuView view,
            ScoreService score,
            Action startGame,
            Action openShop)
        {
            m_view = view;
            m_score = score;
            m_startGame = startGame;
            m_openShop = openShop;
        }

        public void Enter()
        {
            m_view.Show();

            m_view.SetPlayAction(m_startGame);
            m_view.SetShopAction(m_openShop);

            m_view.SetHighScore(m_score.HighScore);
        }

        public void Exit()
        {
            m_view.Hide();
        }
    }
}
