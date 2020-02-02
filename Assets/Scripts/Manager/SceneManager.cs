using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class SceneManager : BaseManager
{
    //场景Prefabs的名字格式为："Background_{0}",index
    //场景Prefabs的Tab为：Background，场景边缘Trigger的Tab为：Edge
    //每个场景因为长宽比例原因，需针对手动创建Trigger
    public SceneManager(GameFacade facade) : base(facade) { }

    private int index = -1;
    //场景编号的最大值，在每次美工完成新的场景之后要重新编写!!!
    private int maxIndex = 11;
    private Dictionary<int, GameObject> sceneDic = new Dictionary<int, GameObject>();

    public GameObject bgGO = null;
    
    public override void OnInit()
    {
        bgGO = GameObject.FindGameObjectWithTag("Background");
        string name = bgGO.name;
        string[] strs = name.Split('_');
        index = int.Parse(strs[1]);
        for (int i = 0; i < maxIndex + 1; i++)
        {
            String str = "Background_" + i;
            if (LoadScene(str) != null) 
            {
                sceneDic.Add(i, LoadScene(str));
            }
            else
            {
                str = "BackgroundFix_" + i;
                if (LoadScene(str) != null) 
                {
                    sceneDic.Add(i, LoadScene(str));
                }
            }
        }
    }

    public void SwitchScene(int index)
    {
        GameObject sceneGO = null;
        if (!sceneDic.TryGetValue(index, out sceneGO)) 
        {
            return;
        }
        GameObject.Destroy(bgGO);
        bgGO = GameObject.Instantiate(sceneGO);
        if (index == 4)
        {
            SpriteRenderer sr = bgGO.transform.Find("Plank").GetComponent<SpriteRenderer>();
            if (facade.GetMissionIndex() >= 2) 
            {
                sr.sprite = Resources.Load<Sprite>("Elements/木板");
            }
        }
        this.index = index;
    }

    public int GetPresentIndex()
    {
        return index;
    }

    public GameObject GetPresentGO()
    {
        return bgGO;
    }

    private GameObject LoadScene(string sceneName)
    {
        return Resources.Load<GameObject>("Backgrounds/" + sceneName);
    }
}
