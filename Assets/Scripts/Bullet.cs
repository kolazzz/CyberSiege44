using System.Collections;
using UnityEngine;

public class Bullet1 : MonoBehaviour
{
    public float speed = 10f; // Bullet speed
    public float lifeTime = 5f; // Bullet lifetime

    void Start()
    {
        // Start the coroutine to destroy the bullet after lifeTime seconds
        StartCoroutine(DestroyAfterTime());
    }

    void Update()
    {
        // Move the bullet forward in the 3D space
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Destroy the bullet upon collision with any object
        Destroy(gameObject);
    }

    IEnumerator DestroyAfterTime()
    {
        // Wait for lifeTime seconds
        yield return new WaitForSeconds(lifeTime);
        // Destroy the bullet
        Destroy(gameObject);
    }
}
