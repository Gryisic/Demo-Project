using System;
using System.Collections;
using UnityEngine;

public abstract class CustomCoroutineBase 
{
    public bool IsProcessing => _coroutine != null;

    protected MonoBehaviour _owner;
    protected Coroutine _coroutine;
}

public class CustomCoroutine : CustomCoroutineBase
{ 
    private Func<IEnumerator> _routine;

    public CustomCoroutine(MonoBehaviour owner, Func<IEnumerator> routine) 
    {
        _owner = owner;
        _routine = routine;
    }

    public void Start() 
    {
        Stop();

        _coroutine = _owner.StartCoroutine(Process());
    }

    public void Stop() 
    {
        if (IsProcessing) 
        {
            _owner.StopCoroutine(_coroutine);

            _coroutine = null;
        }
    }

    private IEnumerator Process() 
    {
        yield return _routine.Invoke();

        _coroutine = null;
    }
}

public class CustomCoroutine<T> : CustomCoroutineBase
{
    private Func<T, IEnumerator> _routine;

    public CustomCoroutine(MonoBehaviour owner, Func<T, IEnumerator> routine)
    {
        _owner = owner;
        _routine = routine;
    }

    public void Start(T arg)
    {
        Stop();

        _coroutine = _owner.StartCoroutine(Process(arg));
    }

    public void Stop()
    {
        if (IsProcessing)
        {
            _owner.StopCoroutine(_coroutine);

            _coroutine = null;
        }
    }

    private IEnumerator Process(T arg)
    {
        yield return _routine.Invoke(arg);

        _coroutine = null;
    }
}
