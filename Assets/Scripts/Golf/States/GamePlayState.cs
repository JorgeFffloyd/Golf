
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
        private readonly StoneSpawner m_spawner;
        private readonly Stick m_stick;
        private readonly ScoreService m_score;
        private readonly BalanceService m_balance;
        private readonly Basket m_basket;
        private readonly HudView m_hud;
        private readonly HitButton m_hitButton;
        private readonly AudioSource m_music;

        private StickCollision[] m_stickCollisions;

        public GamePlayState(
            StoneSpawner spawner,
            Stick stick,
            ScoreService score,
            BalanceService balance,
            Basket basket,
            HudView hud,
            HitButton hitButton,
            AudioSource music)
        {
            m_spawner = spawner;
            m_stick = stick;
            m_score = score;
            m_balance = balance;
            m_basket = basket;
            m_hud = hud;
            m_hitButton = hitButton;
            m_music = music;
        }

        public void Enter()
        {
            m_score.Reset();
            m_basket.ResetBasket();

            m_balance.OnBalanceChanged += m_hud.SetBalance;

            m_hud.Show();
            m_hud.SetScore(m_score.CurrentScore);
            m_hud.SetBalance(m_balance.CurrentBalance);

            m_hitButton.Show();
            m_hitButton.OnHit += HandleHit;

            m_basket.OnGoldOreCollected += HandleGoldOreCollected;

            m_stickCollisions =
                m_stick.GetComponentsInChildren<StickCollision>(true);

            foreach (StickCollision collision in m_stickCollisions)
            {
                collision.OnStoneHit += HandleStoneHit;
            }

            m_spawner.ResetSpawner();
            m_spawner.StartSpawning();

            if (m_music != null && !m_music.isPlaying)
            {
                m_music.Play();
            }
        }

        public void Exit()
        {
            m_spawner.StopSpawning();

            m_hitButton.OnHit -= HandleHit;

            m_basket.OnGoldOreCollected -= HandleGoldOreCollected;
            m_balance.OnBalanceChanged -= m_hud.SetBalance;

            if (m_stickCollisions != null)
            {
                foreach (StickCollision collision in m_stickCollisions)
                {
                    if (collision != null)
                        collision.OnStoneHit -= HandleStoneHit;
                }
            }

            m_hitButton.Hide();
            m_hud.Hide();

            if (m_music != null)
            {
                m_music.Stop();
            }
        }

        public void Update(float deltaTime)
        {
        }

        private void HandleHit()
        {
            m_stick.StartSwing();
        }

        private void HandleStoneHit()
        {
            m_score.AddPoint();
            m_hud.SetScore(m_score.CurrentScore);
        }

        private void HandleGoldOreCollected()
        {
            m_balance.Add(1);
            m_hud.SetBalance(m_balance.CurrentBalance);
        }
    }
}
