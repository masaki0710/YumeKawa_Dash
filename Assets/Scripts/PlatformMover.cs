using UnityEngine;

public class PlatformMover : MonoBehaviour
{
    public float speed = 1.0f;

    public Vector3 direction = new Vector3(-1, 0, 0);

    void Update()
    {
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);

        if (transform.position.x < -15f)
        {
            Destroy(gameObject);
        }
    }
}
