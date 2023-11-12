using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoad : MonoBehaviour
{
    public Animator transitions;
    public float transitionTime;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void LoadLevel()
    {
        StartCoroutine(loadLevelTime(SceneManager.GetActiveScene().buildIndex + 1));
        
    }

    IEnumerator loadLevelTime(int levelIndex)
    {
        transitions.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
        transitions.SetTrigger("LoadComplete");
    }

    
}
