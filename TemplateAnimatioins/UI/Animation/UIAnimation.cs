using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// UIAnimationのイベント
/// </summary>
namespace Zeke.Tweening
{
	public class UIAnimation : UIBase, IUIAnimation
	{
		// エディタ側に書くと初期化されてしまうためこちらに記述
		[HideInInspector]
		public bool isExtraSettings = true;

		public virtual AnimationModel Model {
			get;
		}

		public virtual bool IsStart {
			get;
			set;
		}

		public virtual Tween Play ()
		{
			return Model.Play ();
		}
	}

	public class AnimateInUI : UIAnimation
	{
        [SerializeField, HideInInspector]   
		private bool isStart = false;
		public override bool IsStart {
			get {
				return this.isStart;
			}
			set {
				this.isStart = value;
			}
		}

		public bool playOnEnable = false;

		public virtual void OnEnable(){
			if (playOnEnable) {
				Ready ();
				Play ();
			}
		}

		// 遷移前の準備
		public virtual void Ready(){
			Model.Ready ();
		}

		protected virtual void Reset(){
			Model.SaveEndParams ();
		}
	}

	public class AnimateOutUI : UIAnimation
	{
        [SerializeField, HideInInspector]
		private bool isStart = true;
		public override bool IsStart {
			get {
				return this.isStart;
			}
			set {
				this.isStart = value;
			}
		}

		protected virtual void Reset(){
			Model.SaveStartParams ();
		}
	}

	// アニメーションごとの内容
	[System.Serializable]
	public class AnimationModel{

		[HideInInspector]
		public float delay = 0f;
		[HideInInspector]
		public Ease ease;

		public float duration = 0.5f;

		public AnimationModel(){
			delay = 0f;
			duration = 0.5f;
		}

		public virtual void SetStartParams(){
		}

		public virtual void SetEndParams(){
		}

		public virtual void SaveStartParams(){
		}

		public virtual void SaveEndParams(){
		}

		public virtual void Ready(){
		}

		public virtual Tween Play(){
			return null;
		}
	}
}