using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text txtScore;

    [SerializeField]
    private Text txtInfo;

    [SerializeField]
    private CanvasGroup canvasGroupInfo;

    [SerializeField]
    private ResultPopUp resultPopUpPrefab;

    [SerializeField]
    private Transform canvasTran;

    public void UpdateDisplayScore(int score)
    {
        txtScore.text = score.ToString();
    }

    public void DisplayGameOverInfo()
    {
        canvasGroupInfo.DOFade(1.0f, 1.0f);

        txtInfo.DOText("Game Over...", 1.0f);
    }

    public void GenerateResultPopUp(int score)
    {
        ResultPopUp resultPopUp = Instantiate(resultPopUpPrefab, canvasTran, false);

        resultPopUp.SetUpResultPopUp(score);
    }
}
