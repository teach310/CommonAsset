﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class LoginService {

    public IObservable<Unit> Login(){
        return FirebaseClient.LoadMaster();
    }
}
