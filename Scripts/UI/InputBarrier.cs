using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using System.Linq;

[RequireComponent(typeof(Canvas))]
public class InputBarrier : SingletonMonoBehaviour<InputBarrier> {

    public enum Target{
        Screen = 99,
        Window = 199, // Default
        Dialog = 299
    }

    Canvas barrier;
    Canvas Barrier{
        get{ 
            if(barrier == null)
                barrier = this.GetComponent<Canvas>();
            return barrier;
        }
    }

    class BarrierRequest
    {
        public int Depth{ get; set; }
        public BarrierRequest(int depth){
            Depth = depth;
        }
    }

    static List<BarrierRequest> requests = new List<BarrierRequest>();
 
    protected override void Awake()
    {
        base.Awake();
        if(requests.Count == 0)
            InActivateBarrier();
        //DontDestroyOnLoad(this.gameObject);
    }

    static void ActivateBarrier(){
        Instance.Barrier.enabled = true;
    }

    static void InActivateBarrier(){
        Instance.Barrier.enabled = false;
    }

    public static IDisposable Guard(Target target = Target.Window){
        return Guard ((int)target);
    }

    public static IDisposable Guard(int depth){
        var req = new BarrierRequest (depth);
        requests.Add (req);
        UpdateBarrier ();
        return Disposable.Create (() => {
            if(requests.Contains(req)){
                requests.Remove(req);
                UpdateBarrier();
            }
        });
    }

    static void UpdateBarrier(){
        if (requests.Count == 0) {
            InActivateBarrier ();
            return;
        }
        ActivateBarrier ();
        Instance.Barrier.sortingOrder = requests.Max (x => x.Depth);
    }

    public static void RemoveAllBarrier(){
        requests.Clear ();
        UpdateBarrier ();
    }
}
