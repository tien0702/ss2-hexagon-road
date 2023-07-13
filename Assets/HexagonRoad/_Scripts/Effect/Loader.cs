using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    Animator animator;
    [SerializeField] private float t = 0.5f;
    void Start()
    {
        animator = transform.Find("Crossfade").GetComponent<Animator>();
    }

    public void LoadSceneByIndex(int index)
    {
        StartCoroutine(Load(index));
    }

    public void LoadSceneByName(string name)
    {
        StartCoroutine(Load(name));
    }

    IEnumerator Load(int index)
    {
        yield return new WaitForSeconds(t);
        animator.SetTrigger("start");
        SceneManager.LoadScene(index);
    }

    IEnumerator Load(string name)
    {
        yield return new WaitForSeconds(t);
        animator.SetTrigger("start");
        SceneManager.LoadScene( "HexagonRoad/Scenes/" + name);
    }
}
