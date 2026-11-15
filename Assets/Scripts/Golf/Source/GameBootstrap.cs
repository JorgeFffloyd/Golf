
using UnityEngine;
using Golf.GamePlay;
using Golf.Golf.GamePlay;
using Golf.Golf.States;
using Golf.Golf.States.Interfaces;
using Golf.Golf.View;

namespace Golf.Golf.Source
{
    public class GameBootstrap : MonoBehaviour
    {
        [Header("Gameplay Components")]
        [SerializeField] private StoneSpawner m_spawner;
        [SerializeField] private Stick m_stick;
        [SerializeField] private Basket m_basket;

        [Header("Audio")]
        [SerializeField] private AudioSource m_music;

        [Header("UI")]
        [SerializeField] private MainMenuView m_menuView;
        [SerializeField] private HudView m_hudView;
        [SerializeField] private HitButton m_hitButton;
        [SerializeField] private GameOverView m_gameOverView;

        private IGameState m_currentState;
        private IUpdatable m_currentUpdatable;

        private MainMenuState m_menuState;
        private GamePlayState m_playState;
        private GameOverState m_gameOverState;

        private ScoreService m_score;
        private BalanceService m_balance;

        private void Start()
        {
            if (m_spawner == null ||
                m_stick == null ||
                m_basket == null ||
                m_music == null ||
                m_menuView == null ||
                m_hudView == null ||
                m_hitButton == null ||
                m_gameOverView == null)
            {

                enabled = false;
                return;
            }

            HighScoreStorage storage = new HighScoreStorage();

            m_score = new ScoreService(storage);
            m_balance = new BalanceService();

            m_menuState = new MainMenuState(
                m_menuView,
                m_score,
                GoToGameplay
            );

            m_playState = new GamePlayState(
                m_spawner,
                m_stick,
                m_score,
                m_balance,
                m_basket,
                m_hudView,
                m_hitButton,
                m_music
            );

            m_gameOverState = new GameOverState(
                m_score,
                m_gameOverView,
                ReturnToMenu
            );

            m_basket.OnStoneHitBasket += EndGame;

            ChangeState(m_menuState);
        }

        private void Update()
        {
            m_currentUpdatable?.Update(Time.deltaTime);
        }

        private void ChangeState(IGameState newState)
        {
            if (m_currentState != null)
            {
                m_currentState.Exit();
            }

            m_currentState = newState;
            m_currentUpdatable = newState as IUpdatable;

            m_currentState.Enter();
        }

        private void GoToGameplay()
        {
            ChangeState(m_playState);
        }

        private void EndGame()
        {
            if (m_currentState == m_gameOverState)
                return;

            ChangeState(m_gameOverState);
        }

        private void ReturnToMenu()
        {
            ChangeState(m_menuState);
        }

        private void OnDestroy()
        {
            if (m_basket != null)
            {
                m_basket.OnStoneHitBasket -= EndGame;
            }
        }
    }
}
