using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject aerialFloorPrefab;

    [SerializeField]
    private Transform generateTran;

    [Header("生成までの待機時間")]
    public float waitTime;
    private float timer;

    private GameDirector gameDirector;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= waitTime)
        {
            timer = 0;

            GenerateFloor();
        }
    }

    private void GenerateFloor()
    {
        GameObject obj = Instantiate(aerialFloorPrefab, generateTran);

        float randomPoy = Random.Range(-4.0f, 4.0f);

        obj.transform.position = new Vector2(obj.transform.position.x,
            obj.transform.position.y + randomPoy);

        gameDirector.GenerateCount++;
    }

    public void SetUpGenerator(GameDirector gameDirector)
    {
        this.gameDirector = gameDirector;
    }
}
