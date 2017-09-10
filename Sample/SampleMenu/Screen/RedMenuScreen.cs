using System.Collections;
using System.Collections.Generic;
using Repository;
using UnityEngine;

public class RedMenuScreen : ScreenPresenter
{
    public override UniRx.IObservable<UniRx.Unit> Initialize()
    {
        foreach (var item in FoodRepository.FindAll())
        {
            Debug.LogError(item.name);
        }
        return base.Initialize();
    }

}
