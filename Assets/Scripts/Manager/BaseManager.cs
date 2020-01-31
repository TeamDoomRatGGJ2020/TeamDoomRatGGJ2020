using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager
{
    //所有的Manager都要继承自BaseManager
    //Manager想要实现生命周期函数必须要重写父类方法才能实现

    protected GameFacade facade;

    public BaseManager(GameFacade facade)
    {
        this.facade = facade;
    }

    public virtual void Update() { }
    public virtual void OnInit() { }
    public virtual void OnDestroy() { }
}
