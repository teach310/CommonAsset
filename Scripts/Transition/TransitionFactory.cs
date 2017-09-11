using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TransitionStyle = Const.TransitionStyle;

namespace Common.Transition
{
    /// <summary>
    /// TransitionStyleからITransitionを返す
    /// </summary>
    public static class TransitionFactory
    {
        static TransitionNull transitionNull;

        static TransitionNull TransitionNull {
            get {
                if (transitionNull == null)
                    transitionNull = new TransitionNull ();
                return transitionNull;
            }
        }

        public static ITransition Create (TransitionStyle style)
        {
            switch (style) {
            case TransitionStyle.Fade:
                return new TransitionFade ();
            case TransitionStyle.Null:
                return TransitionNull;
            default:
                return TransitionNull;
            }
        }
    }
}
