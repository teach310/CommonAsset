using UniRx;
public interface ITransition {
    IObservable<Unit> AnimateIn();
    IObservable<Unit> AnimateOut();
}
