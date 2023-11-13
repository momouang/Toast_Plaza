using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoad : MonoBehaviour
{
    public Animator transitions;
    public float transitionTime;
    int currentIndex = 0;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        currentIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public void LoadLevel(int index)
    {
        StartCoroutine(loadLevelTime(index));
        
    }

    IEnumerator loadLevelTime(int levelIndex)
    {
        transitions.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
        transitions.SetTrigger("LoadComplete");
    }

    
}
