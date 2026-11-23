
using System;
using UnityEngine;
using Golf.GamePlay;
using Golf.Golf.GamePlay;
using Golf.Golf.Source;
using Golf.Golf.States.Interfaces;
using Golf.Golf.View;

namespace Golf.Golf.States
{
    public class GamePlayState : IGameState, IUpdatable
    {
        private GameplayArea m_area;

        private readonly Stick m_stick;
        private readonly ScoreService m_score;
        private readonly BalanceService m_balance;
        private readonly HudView m_hud;
        private readonly HitButton m_hitButton;

        private readonly PauseView m_pauseView;
        private readonly Action m_returnToMenu;
        private readonly Action m_endGame;

        private StoneSpawner m_spawner;
        private Basket m_basket;
        private AudioSource m_music;

        private StickCollision[] m_stickCollisions;

        public GamePlayState(
            GameplayArea area,
            Stick stick,
            ScoreService score,
            BalanceService balance,
            HudView hud,
            HitButton hitButton,
            PauseView pauseView,
            Action returnToMenu,
            Action endGame)
        {
            m_area = area;
            m_stick = stick;
            m_score = score;
            m_balance = balance;
            m_hud = hud;
            m_hitButton = hitButton;

            m_pauseView = pauseView;
            m_returnToMenu = returnToMenu;
            m_endGame = endGame;
        }

        public void SetGameplayArea(GameplayArea area)
        {
            m_area = area;
        }

        public void Enter()
        {
            Time.timeScale = 1f;

            if (m_area == null)
            {
                Debug.LogError("GamePlayState: Игровая область не назначена!");
                return;
            }

            m_spawner = m_area.Spawner;
            m_basket = m_area.AreaBasket;
            m_music = m_area.Music;

            m_area.AreaCamera.enabled = true;

            m_score.Reset();
            m_basket.ResetBasket();

            m_balance.OnBalanceChanged += m_hud.SetBalance;

            m_hud.Show();
            m_hud.SetScore(m_score.CurrentScore);
            m_hud.SetBalance(m_balance.CurrentBalance);

            m_hitButton.Show();
            m_hitButton.OnHit += HandleHit;

            m_basket.OnGoldOreCollected += HandleGoldOreCollected;
            m_basket.OnBombCollected += HandleBombCollected;

            m_stickCollisions =
                m_stick.GetComponentsInChildren<StickCollision>(true);

            foreach (StickCollision collision in m_stickCollisions)
            {
                collision.OnStoneHit += HandleStoneHit;
                collision.OnBombHit += HandleBombHit;
            }

            m_spawner.ResetSpawner();
            m_spawner.StartSpawning();

            if (m_music != null && !m_music.isPlaying)
            {
                m_music.Play();
            }

            m_pauseView.Hide();
            m_pauseView.SetPauseAction(PauseGame);
            m_pauseView.SetResumeAction(ResumeGame);
            m_pauseView.SetMenuAction(ReturnToMenu);
        }

        public void Exit()
        {
            if (m_music != null)
            {
                m_music.Stop();
            }

            Time.timeScale = 1f;
            m_pauseView.Hide();

            if (m_spawner != null)
            {
                m_spawner.StopSpawning();
            }

            m_hitButton.OnHit -= HandleHit;

            if (m_basket != null)
            {
                m_basket.OnGoldOreCollected -= HandleGoldOreCollected;
                m_basket.OnBombCollected -= HandleBombCollected;
            }

            m_balance.OnBalanceChanged -= m_hud.SetBalance;

            if (m_stickCollisions != null)
            {
                foreach (StickCollision collision in m_stickCollisions)
                {
                    if (collision != null)
                    {
                        collision.OnStoneHit -= HandleStoneHit;
                        collision.OnBombHit -= HandleBombHit;
                    }
                }
            }

            m_hitButton.Hide();
            m_hud.Hide();
        }

        public void Update(float deltaTime)
        {
        }

        private void PauseGame()
        {
            Time.timeScale = 0f;

            if (m_music != null && m_music.isPlaying)
            {
                m_music.Pause();
            }

            m_pauseView.Show();
        }

        private void ResumeGame()
        {
            Time.timeScale = 1f;

            if (m_music != null)
            {
                m_music.UnPause();
            }

            m_pauseView.Hide();
        }

        private void ReturnToMenu()
        {
            Time.timeScale = 1f;
            m_pauseView.Hide();
            m_returnToMenu?.Invoke();
        }

        private void HandleHit()
        {
            if (Time.timeScale == 0f) return;

            m_stick.StartSwing();
        }

        private void HandleStoneHit()
        {
            m_score.AddPoint();
            m_hud.SetScore(m_score.CurrentScore);
        }

        private void HandleBombHit()
        {
            if (Time.timeScale == 0f) return;

            m_endGame?.Invoke();
        }

        private void HandleGoldOreCollected()
        {
            m_balance.Add(1);
            m_hud.SetBalance(m_balance.CurrentBalance);
        }

        private void HandleBombCollected()
        {
            m_score.AddPoint();
            m_hud.SetScore(m_score.CurrentScore);
        }
    }
}
