using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text txtScore;

    public void UpdateDisplayScore(int score)
    {
        txtScore.text = score.ToString();
    }
}
