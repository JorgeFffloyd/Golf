using UnityEngine;

public class SparkOnCollision : MonoBehaviour
{
    public GameObject Particle;   

    private void OnCollisionEnter(Collision collision)
    {
        GameObject effect = Instantiate(Particle, collision.contacts[0].point, Quaternion.identity);
        effect.GetComponent<ParticleSystem>().Play();
        Destroy(effect, 0.2f);
    }
}