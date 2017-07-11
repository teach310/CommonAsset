using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.Linq;

public class GameObjectContainer : MonoBehaviour{
	private List<GameObject> list = new List<GameObject> ();
	public List<GameObject> Objects{
		get{ return Objects; }
	}

	public GameObject Add(GameObject obj){
		var newObj = Instantiate (obj, this.transform) as GameObject;
		newObj.transform.localScale = Vector3.one;
		list.Add (newObj);
		return obj;
	}

	public void Clear(){
		foreach (var item in list) {
			Destroy (item);
		}
		list.Clear ();
	}

	public void Remove(string name){
		var obj = list.FirstOrDefault (x => x.name == name);
		if (obj == null) {
			Debug.Log ("Fail to Remove");
			return;
		}
		list.Remove (obj);
		Destroy (obj);
	}

	public GameObject Find(string name){
		var result = list.SelectMany (parent => parent.ChildrenAndSelf ())
			.FirstOrDefault (obj => obj.name.Contains(name));

		return result;
	}
}
