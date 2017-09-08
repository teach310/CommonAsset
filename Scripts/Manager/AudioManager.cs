using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;



public enum BGMKey
{
	Title,
	Select,
	Battle,
	Result
}

/// <summary>
/// 最低限の機能のシンプルなAudioManager
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class AudioManager : SingletonMonoBehaviour<AudioManager> {

	// SEKeyで音を取得
    private Dictionary<string, AudioClip> _SE_Map
    = new Dictionary<string, AudioClip>();

	// BGMKeyで音を取得
    private Dictionary<string, AudioClip> _BGM_Map
    = new Dictionary<string, AudioClip>();

	private AudioSource _bgmSource;

	private List<AudioSource> _seSource = new List<AudioSource>();

	protected override void Awake ()
	{
		base.Awake ();
		_bgmSource = this.GetComponent<AudioSource> ();
	}


	/// <summary>
	/// SEを再生
	/// </summary>
	/// <param name="key">Key.</param>
    public void PlaySE(string key){
		if (!_SE_Map.ContainsKey (key)) {
            _SE_Map.Add (key, ResourcesManager.Instance.GetSE (key));
		}

		AudioSource source = _seSource.Where (x => !x.isPlaying).FirstOrDefault ();
		if (!source) {
			source = this.gameObject.AddComponent<AudioSource> ();
			_seSource.Add (source);
		}
		source.clip = _SE_Map [key];
		source.Play();
	}


	/// <summary>
	/// BGMを再生
	/// </summary>
	/// <param name="key">Key.</param>
    public void PlayBGM(string key){
		if (!_BGM_Map.ContainsKey (key)) {
            _BGM_Map.Add (key, ResourcesManager.Instance.GetBGM (key));
		}
		_bgmSource.clip = _BGM_Map [key];
		_bgmSource.Play ();
	}

}
