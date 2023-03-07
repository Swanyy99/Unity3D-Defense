using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BlackCover : MonoBehaviour
{
    private Animator anim;

    private string color;

    private float alpha;

    private float animProgress;

   

    private void Awake()
    {
        anim = GetComponent<Animator>();
        alpha = gameObject.GetComponent<Image>().color.a;
        color = alpha == 0 ? "ToBlack" : "ToTransparent";
    }

    private void OnEnable()
    {
        ChangeColor();
    }

    private void Update()
    {
        animProgress = anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    public void ChangeColor()
    {
        anim.Play(color);
        StartCoroutine(DetectStatus());
    }

    private void SceneChange()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void Activefalse()
    {
        gameObject.SetActive(false);
    }

    IEnumerator DetectStatus()
    {
        yield return new WaitUntil(()=> animProgress >= 1.0f);
        if (color == "ToBlack") SceneChange();
        else Activefalse();
    }

}
