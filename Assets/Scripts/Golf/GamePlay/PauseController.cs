using System;
using UnityEngine;

namespace Golf.Golf.GamePlay
{
    public class PauseController : MonoBehaviour
    {
        public bool IsPaused { get; private set; }

        public event Action OnPaused;
        public event Action OnResumed;

        public void Pause()
        {
            if (IsPaused)
                return;

            IsPaused = true;
            Time.timeScale = 0f;
            OnPaused?.Invoke();
        }

        public void Resume()
        {
            if (!IsPaused)
                return;

            IsPaused = false;
            Time.timeScale = 1f;
            OnResumed?.Invoke();
        }

        public void ResetPause()
        {
            IsPaused = false;
            Time.timeScale = 1f;
        }
    }
}
