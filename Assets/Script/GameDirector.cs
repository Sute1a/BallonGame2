using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    [SerializeField]
    private GoalChecker goalHousePrefab;

    [SerializeField]
    private PlayerController playerController;

    [SerializeField]
    private FloorGenerator[] floorGenerators;

    private bool isSetUp;

    private bool isGameUp;

    private int generateCount;

    public int GenerateCount
    {
        set
        {
            generateCount = value;

            Debug.Log("生成数/クリア目標数：" + generateCount + "/" + clearCount);

            if (generateCount >= clearCount)
            {
                GenerateGoal();

                GameUp();
            }
        }
        get
        {
            return generateCount;
        }
    }
    public int clearCount;

    private void Start()
    {
        isGameUp = false;
        isSetUp = false;

        SetUpFloorGenerators();

        Debug.Log("生成停止");
    }
    private void SetUpFloorGenerators()
    {
        for(int i =0; i<floorGenerators.Length; i++)
        {
            floorGenerators[i].SetUpGenerator(this);
        }
    }
    private void Update()
    {
        if(playerController.isFirstGenerateBallon && isSetUp == false)
        {
            isSetUp = true;

            Debug.Log("生成スタート");
        }
    }

    private void GenerateGoal()
    {
        GoalChecker goalHouse = Instantiate(goalHousePrefab);

        Debug.Log("ゴール地点　生成");
    }

    public void GameUp()
    {
        isGameUp = true;

        Debug.Log("生成停止");
    }
}
