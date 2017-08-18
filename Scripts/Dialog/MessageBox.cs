using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Common.Tweening;
using System.Linq;
using UnityEngine.UI;

public class MessageBox : DialogPresenter
{
    List<AnimateInUI> animateInUiList;
    List<AnimateInUI> AnimateInUiList{
        get{
            if(animateInUiList == null){
                animateInUiList = this.GetComponents<AnimateInUI>().ToList();
            }
            return animateInUiList;
        }
    }


    private void Reset()
    {
        DialogTools.AddDefaultAnimation(this.gameObject);
    }

    public override IObservable<Unit> OnBeforeOpen()
    {
        AnimateInUiList.ForEach(x=>x.Ready());
        return base.OnBeforeOpen();
    }

    public override IObservable<Unit> OnOpen()
    {
        return Observable.WhenAll(
            AnimateInUiList.Select(x => x.Play().OnCompleteAsObservable())
        );
    }

    public override IObservable<Unit> OnClose()
    {
        return Observable.WhenAll((
            this.GetComponents<AnimateOutUI>().Select(x => x.Play().OnCompleteAsObservable())
        ));
    }



    public Text titleLabel;
    public Text description;
    [SerializeField] Button yesButton;
    public IObservable<Unit> OnYes{
        get{ return yesButton.OnClickAsObservable().FirstOrDefault(); }
    }

    [SerializeField] Button noButton;
    public IObservable<Unit> OnNo{
        get { return noButton.OnClickAsObservable().FirstOrDefault(); }
    }

	public void ResetView(string title, string description)
	{
        titleLabel.text = title;
        this.description.text = description;
	}
}
