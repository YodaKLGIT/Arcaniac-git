using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject Cutscene;
    public GameObject BackroundMusic;
    private AudioSource backgroundMusicSource;

    private void Start()
    {
        backgroundMusicSource = BackroundMusic.GetComponent<AudioSource>();
    }

    public void startGame()
    {
        StartCoroutine(FadeOutMusic());
        Cutscene.SetActive(true);
    }

    private IEnumerator FadeOutMusic()
    {
        float startVolume = backgroundMusicSource.volume;

        for (float t = 0; t < 2f; t += Time.deltaTime)
        {
            backgroundMusicSource.volume = Mathf.Lerp(startVolume, 0, t / 2f);
            yield return null;
        }

        backgroundMusicSource.volume = 0;
        BackroundMusic.SetActive(false);
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
