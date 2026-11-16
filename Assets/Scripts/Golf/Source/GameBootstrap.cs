using UnityEngine;
using UnityEngine.UI;
using TMPro;
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
        [SerializeField] private AudioSource m_shopMusic;

        [Header("Cameras")]
        [SerializeField] private Camera m_mainCamera;
        [SerializeField] private Camera m_shopCamera;

        [Header("UI")]
        [SerializeField] private MainMenuView m_menuView;
        [SerializeField] private HudView m_hudView;
        [SerializeField] private HitButton m_hitButton;
        [SerializeField] private GameOverView m_gameOverView;

        [Header("Shop UI")]
        [SerializeField] private GameObject m_shopHud;
        [SerializeField] private Button m_backToMenuButton;
        [SerializeField] private TMP_Text m_shopBalanceText;

        [Header("Shop Items")]
        [SerializeField] private Button m_pickaxeButton;
        [SerializeField] private Button m_hammerButton;
        [SerializeField] private TMP_Text m_pickaxeButtonText;
        [SerializeField] private TMP_Text m_hammerButtonText;

        [Header("Hoe")]
        [SerializeField] private Button m_hoeButton;
        [SerializeField] private TMP_Text m_hoeButtonText;

        private IGameState m_currentState;
        private IUpdatable m_currentUpdatable;

        private MainMenuState m_menuState;
        private GamePlayState m_playState;
        private GameOverState m_gameOverState;
        private ShopState m_shopState;

        private ScoreService m_score;
        private BalanceService m_balance;

        private void Start()
        {
            if (m_spawner == null ||
                m_stick == null ||
                m_basket == null ||
                m_music == null ||
                m_mainCamera == null ||
                m_shopCamera == null ||
                m_menuView == null ||
                m_hudView == null ||
                m_hitButton == null ||
                m_gameOverView == null ||
                m_shopHud == null ||
                m_backToMenuButton == null ||
                m_shopBalanceText == null ||
                m_pickaxeButton == null ||
                m_hammerButton == null ||
                m_pickaxeButtonText == null ||
                m_hammerButtonText == null ||
                m_hoeButton == null ||
                m_hoeButtonText == null)
            {
                Debug.LogError(
                    "GameBootstrap: Не назначены необходимые ссылки!"
                );

                enabled = false;
                return;
            }

            m_mainCamera.enabled = true;
            m_shopCamera.enabled = false;
            m_shopHud.SetActive(false);

            if (m_shopMusic != null)
                m_shopMusic.Stop();

            HighScoreStorage storage = new HighScoreStorage();

            m_score = new ScoreService(storage);
            m_balance = new BalanceService();

            m_menuState = new MainMenuState(
                m_menuView,
                m_score,
                GoToGameplay,
                GoToShop
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

            m_shopState = new ShopState(
                m_mainCamera,
                m_shopCamera,
                m_shopHud,
                m_backToMenuButton,
                m_pickaxeButton,
                m_hammerButton,
                m_shopBalanceText,
                m_pickaxeButtonText,
                m_hammerButtonText,
                m_hoeButton,
                m_hoeButtonText,
                m_balance,
                m_stick,
                m_shopMusic,
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
                m_currentState.Exit();

            m_currentState = newState;
            m_currentUpdatable = newState as IUpdatable;

            m_currentState.Enter();
        }

        private void GoToGameplay()
        {
            ChangeState(m_playState);
        }

        private void GoToShop()
        {
            ChangeState(m_shopState);
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
                m_basket.OnStoneHitBasket -= EndGame;
        }
    }
}
