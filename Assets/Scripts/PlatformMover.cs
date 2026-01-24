using UnityEngine;

public class PlatformMover : MonoBehaviour
{
    public float speed = 5f;

    public Vector3 direction = new Vector3(-1, 0, -1);

    void Update()
    {
        transform.Translate(direction.normalized * speed * Time.deltaTime);

        if (transform.position.x < -15f)
        {
            Destroy(gameObject);
        }
    }
}
