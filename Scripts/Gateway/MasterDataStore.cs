using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MasterData;

public static class MasterDataStore
{

    public static List<Food> foodList = new List<Food>();
    public static List<Food> FoodList { 
        get{ return foodList; }
    }
}
