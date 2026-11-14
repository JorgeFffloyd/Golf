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
        private readonly HudView m_hud;
        private readonly HitButton m_hitButton;

        public GamePlayState(
            StoneSpawner spawner,
            Stick stick,
            ScoreService score,
            HudView hud,
            HitButton hitButton)
        {
            m_spawner = spawner;
            m_stick = stick;
            m_score = score;
            m_hud = hud;
            m_hitButton = hitButton;
        }

        public void Enter()
        {
            m_score.Reset();

            m_hud.Show();
            m_hud.SetScore(m_score.CurrentScore);

            m_hitButton.Show();
            m_hitButton.OnHit += HandleHit;

            m_spawner.StartSpawning();
        }

        public void Exit()
        {
            m_spawner.StopSpawning();

            m_hitButton.OnHit -= HandleHit;

            m_hitButton.Hide();
            m_hud.Hide();
        }

        public void Update(float deltaTime)
        {
        }

        private void HandleHit()
        {
            m_stick.StartSwing();
        }
    }
}