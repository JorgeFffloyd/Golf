using UnityEngine;

public class Stick : MonoBehaviour
{
    private Quaternion m_pointA;
    private Quaternion m_pointB;
    private bool m_isSwinging = false;
    private Rigidbody m_rb;

    private void Start()
    {
        m_pointA = transform.localRotation;
        m_pointB = m_pointA * Quaternion.Euler(100f, 0f, 0f);

        m_rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) m_isSwinging = true;
        if (Input.GetMouseButtonUp(0)) m_isSwinging = false;
    }

    private void FixedUpdate()
    {
        if (m_rb == null) return;

        if (m_isSwinging)
        {
            Quaternion targetRotation = Quaternion.RotateTowards(m_rb.rotation, m_pointB, 1500f * Time.fixedDeltaTime);
            m_rb.MoveRotation(targetRotation);
        }
        else
        {
            Quaternion targetRotation = Quaternion.RotateTowards(m_rb.rotation, m_pointA, 500f * Time.fixedDeltaTime);
            m_rb.MoveRotation(targetRotation);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Столкновение с: " + collision.gameObject.name);

        if (collision.gameObject.name.Contains("falling_stone"))
        {
            Rigidbody stoneRb = collision.gameObject.GetComponent<Rigidbody>();
            if (stoneRb != null)
            {

                Vector3 forceDirection = (collision.transform.position - transform.position).normalized;
                forceDirection.y = 1.2f;

                stoneRb.AddForce(forceDirection * 10f, ForceMode.Impulse);
            }
        }
    }
}