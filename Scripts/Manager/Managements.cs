using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managements : MonoBehaviour {

	void Awake(){
		DontDestroyOnLoad (this.gameObject);
	}
}
