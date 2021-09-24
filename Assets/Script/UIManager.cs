using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

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

    [SerializeField]
    private Button btnInfo;

    public void UpdateDisplayScore(int score)
    {
        txtScore.text = score.ToString();
    }

    public void DisplayGameOverInfo()
    {
        canvasGroupInfo.DOFade(1.0f, 1.0f);

        txtInfo.DOText("Game Over...", 1.0f);

        btnInfo.onClick.AddListener(RestartGame);
    }

    public void GenerateResultPopUp(int score)
    {
        ResultPopUp resultPopUp = Instantiate(resultPopUpPrefab, canvasTran, false);

        resultPopUp.SetUpResultPopUp(score);
    }

    public void RestartGame()
    {
        btnInfo.onClick.RemoveAllListeners();

        string sceneName = SceneManager.GetActiveScene().name;

        canvasGroupInfo.DOFade(0f, 1.0f)
            .OnComplete(() =>
            {
                Debug.Log("Restart");
                SceneManager.LoadScene(sceneName);
            });
    }
}
