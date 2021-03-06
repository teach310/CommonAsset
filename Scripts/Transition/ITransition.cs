﻿using UniRx;
using System;
public interface ITransition {
    IObservable<Unit> AnimateIn();
    IObservable<Unit> AnimateOut();
}
