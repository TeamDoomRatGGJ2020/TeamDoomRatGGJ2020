using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : BasePanel
{
    private Button startButton;
    private Button quitButton;

    private CameraFilterPack_Blur_Movie cameraMovie;
    private float timeCount;

    private bool isNeedEffect = false;

    public Quarrel quarrel;

    public override void OnEnter()
    {
        base.OnEnter(); 
        startButton = transform.Find("StartButton").GetComponent<Button>();
        quitButton = transform.Find("QuitButton").GetComponent<Button>();

        startButton.onClick.AddListener(OnStartClick);
        quitButton.onClick.AddListener(OnQuitClick);

        cameraMovie = Camera.main.GetComponent<CameraFilterPack_Blur_Movie>();
    }

    void Update()
    {
        if (isNeedEffect)
        { 
            MovieEffect();
        }
    }

    private void OnStartClick()
    {
        isNeedEffect = true;
        startButton.transform.DOLocalMoveX(2000, 0.5f);
        quitButton.transform.DOLocalMoveX(2000, 0.5f);
        startButton.transform.DOScale(0, 0.5f);
        quitButton.transform.DOScale(0, 0.5f);

        StartQuarrel();
    }

    private void StartQuarrel(){
        var quarrelObject = GameObject.Find("Quarrel");
        quarrel = quarrelObject.GetComponent<Quarrel>();
        quarrel.enabled = true;
    }

    private void MovieEffect()
    {
        cameraMovie.Radius = Mathf.Lerp(cameraMovie.Radius, 0f, 0.99f * Time.deltaTime);
        cameraMovie.Factor = Mathf.Lerp(cameraMovie.Factor, 0f, 0.99f * Time.deltaTime);
        DOTween.To(() => timeCount, a => timeCount = a, 1, 2f).OnComplete(EndEffect);
    }

    private void EndEffect()
    {
        cameraMovie.FastFilter = 1;
        isNeedEffect = false;
        cameraMovie.enabled = false;
        gameObject.SetActive(false);
    }

    private void OnQuitClick()
    {
        Application.Quit();
    }
}
