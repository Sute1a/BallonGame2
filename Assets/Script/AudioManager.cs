using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource[] audioSources;

    public IEnumerator PlayBGM(int index)
    {
        if (index != 0)
        {
            audioSources[index - 1].DOFade(0, 0.75f);
            Debug.Log("前の曲のボリュームを下げる");
        }
        if (index == 3)
        {
            audioSources[index - 2].DOFade(0, 0.75f);
        }
        yield return new WaitForSeconds(0.45f);

        audioSources[index].Play();

        Debug.Log("新しい曲を再生し、ボリュームを上げる");

        audioSources[index].DOFade(0.1f, 0.75f);

        if(index != 0)
        {
            yield return new WaitUntil(() => audioSources[index - 1].
            volume == 0);

            audioSources[index - 1].Stop();
            Debug.Log("前の曲を停止");
        }
    }
}
