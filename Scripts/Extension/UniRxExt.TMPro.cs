using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

namespace UniRx
{
	public static partial class UniRxExt
	{
		public static IDisposable SubscribeToText(this IObservable<string> source, TextMeshProUGUI label){
			return source.SubscribeWithState(label, (x, t) => t.text = x);
		}

		public static IDisposable SubscribeToText<T>(this IObservable<T> source, TextMeshProUGUI label){
			return source.SubscribeWithState(label, (x, t) => t.text = x.ToString());
		}

		public static IDisposable SubscribeToText<T>(this IObservable<T> source, TextMeshProUGUI label, Func<T, string> selector){
			return source.SubscribeWithState2(label, selector,  (x, t, s) => t.text = s(x));
		}
	}
}
