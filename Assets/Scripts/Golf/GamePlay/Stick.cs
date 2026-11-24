
using UnityEngine;
using Golf.Golf.GamePlay;

namespace Golf.GamePlay
{
    public class Stick : MonoBehaviour
    {
        public bool IsSwinging => m_isSwinging;

        private const string SelectedModelKey = "SelectedStickModel";

        [Header("Stick Position")]
        [SerializeField] private Transform m_stickPoint;

        [Header("Stick Models")]
        [SerializeField] private GameObject m_defaultModel;
        [SerializeField] private GameObject m_pickaxeModel;
        [SerializeField] private GameObject m_hammerModel;

        [Header("Audio")]
        [SerializeField] private AudioSource m_hitAudioSource;

        [Header("Hit Settings")]
        [SerializeField] private float m_hitForce = 8f;
        [SerializeField] private float m_upwardForce = 2f;

        private Quaternion m_pointA;
        private Quaternion m_pointB;

        private bool m_isSwinging;
        private Rigidbody m_rb;

        private void Awake()
        {
            m_rb = GetComponent<Rigidbody>();

            if (m_stickPoint != null)
                MoveToPoint(m_stickPoint);
            else
                Debug.LogWarning("Ошибка мотыжная!");

            LoadSelectedModel();
        }

        public void MoveToPoint(Transform point)
        {
            if (point == null)
                return;

            CancelInvoke(nameof(StopSwing));
            m_isSwinging = false;

            transform.SetPositionAndRotation(
                point.position,
                point.rotation
            );

            if (m_rb != null)
            {
                m_rb.position = point.position;
                m_rb.rotation = point.rotation;
                m_rb.linearVelocity = Vector3.zero;
                m_rb.angularVelocity = Vector3.zero;
            }

            m_pointA = point.rotation;
            m_pointB = m_pointA * Quaternion.Euler(100f, 0f, 0f);

            m_stickPoint = point;
        }

        private void LoadSelectedModel()
        {
            int selectedModel = PlayerPrefs.GetInt(
                SelectedModelKey, 0
            );

            if (selectedModel == 1)
                SelectPickaxeModel();
            else if (selectedModel == 2)
                SelectHammerModel();
            else
                SelectDefaultModel();
        }

        public void SelectDefaultModel()
        {
            SetModel(m_defaultModel);
        }

        public void SelectPickaxeModel()
        {
            SetModel(m_pickaxeModel);
        }

        public void SelectHammerModel()
        {
            SetModel(m_hammerModel);
        }

        private void SetModel(GameObject selectedModel)
        {
            if (m_defaultModel != null)
                m_defaultModel.SetActive(
                    selectedModel == m_defaultModel
                );

            if (m_pickaxeModel != null)
                m_pickaxeModel.SetActive(
                    selectedModel == m_pickaxeModel
                );

            if (m_hammerModel != null)
                m_hammerModel.SetActive(
                    selectedModel == m_hammerModel
                );
        }

        public void StartSwing()
        {
            if (m_isSwinging)
                return;

            m_isSwinging = true;

            CancelInvoke(nameof(StopSwing));
            Invoke(nameof(StopSwing), 0.2f);
        }

        private void StopSwing()
        {
            m_isSwinging = false;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!m_isSwinging)
                return;

            Stone stone = collision.gameObject.GetComponentInParent<Stone>();

            if (stone != null)
            {
                Vector3 direction =
                    (stone.transform.position - transform.position).normalized;

                Vector3 force =
                    direction * m_hitForce +
                    Vector3.up * m_upwardForce;

                stone.Hit(force);
            }

            if (m_hitAudioSource != null)
                m_hitAudioSource.Play();
        }

        private void FixedUpdate()
        {
            if (m_rb == null)
                return;

            Quaternion targetRotation = m_isSwinging
                ? m_pointB
                : m_pointA;

            float speed = m_isSwinging ? 1500f : 500f;

            m_rb.MoveRotation(
                Quaternion.RotateTowards(
                    m_rb.rotation,
                    targetRotation,
                    speed * Time.fixedDeltaTime
                )
            );
        }
    }
}
