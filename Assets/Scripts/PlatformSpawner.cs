using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject[] modulePrefabs
        ;
    public float spawnInterval = 1.0f;
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnPlatform();
            timer = 0;
        }
    }

    void SpawnPlatform()
    {
        if (modulePrefabs.Length == 0) return;

        // ランダムにプレハブを選択
        int index = Random.Range(0, modulePrefabs.Length);

        // 生成
        Instantiate(modulePrefabs[index], transform.position, Quaternion.identity);
    }
}
