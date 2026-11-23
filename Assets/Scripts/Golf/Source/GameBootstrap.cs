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
        [Header("Gameplay Areas")]
        [SerializeField] private GameplayArea m_firstArea;
        [SerializeField] private GameplayArea m_secondArea;

        [Header("Shared Stick")]
        [SerializeField] private Stick m_stick;

        [Header("Shop Audio")]
        [SerializeField] private AudioSource m_shopMusic;

        [Header("Shop Camera")]
        [SerializeField] private Camera m_shopCamera;

        [Header("UI")]
        [SerializeField] private MainMenuView m_menuView;
        [SerializeField] private HudView m_hudView;
        [SerializeField] private HitButton m_hitButton;
        [SerializeField] private GameOverView m_gameOverView;

        [Header("Pause")]
        [SerializeField] private PauseView m_pauseView;

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

        [Header("Map")]
        [SerializeField] private Button m_secondMapButton;
        [SerializeField] private TMP_Text m_secondMapButtonText;

        private const string SelectedMapKey = "SelectedMap";
        private const string SecondMapOwnedKey = "SecondMapOwned";

        private GameplayArea m_activeArea;

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
            if (m_firstArea == null ||
                m_secondArea == null ||
                !m_firstArea.IsConfigured() ||
                !m_secondArea.IsConfigured() ||
                m_stick == null ||
                m_shopCamera == null ||
                m_menuView == null ||
                m_hudView == null ||
                m_hitButton == null ||
                m_gameOverView == null ||
                m_pauseView == null ||
                m_shopHud == null ||
                m_backToMenuButton == null ||
                m_shopBalanceText == null ||
                m_pickaxeButton == null ||
                m_hammerButton == null ||
                m_pickaxeButtonText == null ||
                m_hammerButtonText == null ||
                m_hoeButton == null ||
                m_hoeButtonText == null ||
                m_secondMapButton == null ||
                m_secondMapButtonText == null)
            {
                Debug.LogError(
                    "GameBootstrap: Не назначены необходимые ссылки!"
                );

                enabled = false;
                return;
            }

            Time.timeScale = 1f;

            m_shopCamera.enabled = false;
            m_shopHud.SetActive(false);

            if (m_shopMusic != null)
                m_shopMusic.Stop();

            int selectedMap = PlayerPrefs.GetInt(SelectedMapKey, 0);

            bool secondMapOwned = PlayerPrefs.GetInt(
                SecondMapOwnedKey, 0
            ) == 1;

            if (selectedMap == 1 && !secondMapOwned)
            {
                selectedMap = 0;
                PlayerPrefs.SetInt(SelectedMapKey, 0);
                PlayerPrefs.Save();
            }

            SelectMap(selectedMap);

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
                m_activeArea,
                m_stick,
                m_score,
                m_balance,
                m_hudView,
                m_hitButton,
                 m_pauseView,
                ReturnToMenu,
                EndGame
                );

            m_gameOverState = new GameOverState(
                m_score,
                m_gameOverView,
                ReturnToMenu
            );

            m_shopState = new ShopState(
                m_firstArea,
                m_secondArea,
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
                m_secondMapButton,
                m_secondMapButtonText,
                m_balance,
                m_stick,
                m_shopMusic,
                ReturnToMenu,
                SelectMap
            );

            ChangeState(m_menuState);
        }

        private void Update()
        {
            m_currentUpdatable?.Update(Time.deltaTime);
        }

        private void SelectMap(int mapIndex)
        {
            GameplayArea newArea =
                mapIndex == 1 ? m_secondArea : m_firstArea;

            if (newArea == null)
                return;

            if (m_activeArea != null)
                m_activeArea.AreaBasket.OnStoneHitBasket -= EndGame;

            m_firstArea.SetAreaActive(newArea == m_firstArea);
            m_secondArea.SetAreaActive(newArea == m_secondArea);

            m_activeArea = newArea;

            bool shopIsOpen = m_currentState is ShopState;

            m_firstArea.AreaCamera.enabled =
                !shopIsOpen && newArea == m_firstArea;

            m_secondArea.AreaCamera.enabled =
                !shopIsOpen && newArea == m_secondArea;

            m_stick.MoveToPoint(m_activeArea.StickPoint);

            m_activeArea.AreaBasket.OnStoneHitBasket += EndGame;

            if (m_playState != null)
                m_playState.SetGameplayArea(m_activeArea);

            PlayerPrefs.SetInt(SelectedMapKey, mapIndex);
            PlayerPrefs.Save();
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
            Time.timeScale = 1f;

            if (m_activeArea != null &&
                m_activeArea.AreaBasket != null)
            {
                m_activeArea.AreaBasket.OnStoneHitBasket -= EndGame;
            }
        }
    }
}
