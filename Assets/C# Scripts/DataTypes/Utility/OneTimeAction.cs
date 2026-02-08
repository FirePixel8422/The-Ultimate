using System;


namespace Fire_Pixel.Utility
{
    /// <summary>
    /// Container that stores an <see cref="Action"/> which can be subscribed to and is invoked only once. If subscribed too after invoke already happened, call the subscriber instantly
    /// </summary>
    public class OneTimeAction
    {
        private event Action internalAction;
        private bool hasExecuted;
        public bool HasExecuted => hasExecuted;


        public void Invoke()
        {
            internalAction?.Invoke();
            internalAction = null;
            hasExecuted = true;
        }
        public static OneTimeAction operator +(OneTimeAction e, Action action)
        {
            if (!e.hasExecuted)
            {
                e.internalAction += action;
            }
            else
            {
                action?.Invoke();
            }
            return e;
        }
        public static OneTimeAction operator -(OneTimeAction e, Action action)
        {
            if (!e.hasExecuted)
            {
                e.internalAction -= action;
            }
            return e;
        }
    }

    /// <summary>
    /// Container that stores an <see cref="Action{T}"/> which can be subscribed to and is invoked only once. If subscribed too after invoke already happened, call the subscriber instantly
    /// </summary>
    public class OneTimeAction<T>
    {
        private event Action<T> internalAction;
        private bool hasExecuted;
        private T invokedValue;
        public bool HasExecuted => hasExecuted;


        public void Invoke(T value)
        {
            if (hasExecuted) return;

            internalAction?.Invoke(value);
            internalAction = null;
            hasExecuted = true;
            invokedValue = value;
        }
        public static OneTimeAction<T> operator +(OneTimeAction<T> e, Action<T> action)
        {
            if (!e.hasExecuted)
            {
                e.internalAction += action;
            }
            else
            {
                action?.Invoke(e.invokedValue);
            }
            return e;
        }
        public static OneTimeAction<T> operator -(OneTimeAction<T> e, Action<T> action)
        {
            if (!e.hasExecuted)
            {
                e.internalAction -= action;
            }
            return e;
        }
    }
}