
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

        [Header("UI")]
        [SerializeField] private MainMenuView m_menuView;
        [SerializeField] private HudView m_hudView;
        [SerializeField] private HitButton m_hitButton;

        private IGameState m_currentState;
        private IUpdatable m_currentUpdatable;

        private MainMenuState m_menuState;
        private GamePlayState m_playState;
        private GameOverState m_gameOverState;

        private ScoreService m_score;

        private void Start()
        {
            if (m_spawner == null ||
                m_stick == null ||
                m_menuView == null ||
                m_hudView == null ||
                m_hitButton == null)
            {
                Debug.LogError(
                    "Не назначены!"
                );

                enabled = false;
                return;
            }

            HighScoreStorage storage = new HighScoreStorage();
            m_score = new ScoreService(storage);

            m_menuState = new MainMenuState(
                m_menuView,
                m_score,
                GoToGameplay
            );

            m_playState = new GamePlayState(
                 m_spawner,
                 m_stick,
                 m_score,
                 m_hudView,
                m_hitButton
            );

            m_gameOverState = new GameOverState(
                m_score,
                ReturnToMenu
            );

            ChangeState(m_menuState);
        }

        private void Update()
        {
            m_currentUpdatable?.Update(Time.deltaTime);
        }

        private void ChangeState(IGameState newState)
        {
            if (m_currentState != null)
                m_currentState.Exit();

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
            ChangeState(m_gameOverState);
        }

        private void ReturnToMenu()
        {
            ChangeState(m_menuState);
        }
    }
}
