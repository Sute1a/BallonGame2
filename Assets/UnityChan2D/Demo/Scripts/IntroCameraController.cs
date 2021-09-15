using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class IntroCameraController : MonoBehaviour
{
    public Transform target;

    private Vector3 pos;

    [SceneName]
    public string nextLevel;

    public static AudioSource startSource;

    private void Start()
    {
        startSource = gameObject.AddComponent<AudioSource>();
    }

    IEnumerator StartSource()
    {
        startSource.Play();
        pos = transform.position;

        yield return new WaitForSeconds(startSource.clip.length+1);

       SceneManager.LoadScene(nextLevel);
    }

    void Update()
    {
        float newPosition = Mathf.SmoothStep(pos.x, target.position.x, Time.timeSinceLevelLoad / startSource.clip.length);

        transform.position = new Vector3(newPosition, pos.y, pos.z);
    }
}
