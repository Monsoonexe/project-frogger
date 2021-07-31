using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Static, project-level game data.
/// </summary>
/// <seealso cref="IntVariable"/>
/// /// <seealso cref="StringVariable"/>
public abstract class AVariable : ApexScriptableObject
{
    public const string CREATE_ASSET_MENU_PATH = "ScriptableObjects/Variables/";
    /// <summary>
    /// Raised when the value has been updated.
    /// </summary>
    public readonly UnityEvent onValueChangedEvent
        = new UnityEvent(); //Unity events can be raised from derived class, unlike C# events

    protected virtual void Reset()
    {
        SetDevDescription("Game data.");
    }
}

/// <summary>
/// Static, project-level game data.
/// </summary>
public abstract class AVariable<T> : AVariable
{
    /// <summary>
    /// Explicitly-typed event.
    /// </summary>
    public class TypedEvent : UnityEvent<T>
    {
        //exists
    }

    [SerializeField]
    protected T _value = default;

    [SerializeField]
    protected bool _isReadonly = false;

    [SerializeField]
    [Tooltip("Should this Variable start the game at an initial value?")]
    private bool _initialize = true;

    [SerializeField]
    [Tooltip("If this Variable should start the game at an initial value, which value?")]
    private T _initialValue = default;

    /// <summary>
    /// Raised when the value has been updated.
    /// </summary>
    public readonly TypedEvent onValueChangedTypedEvent
        = new TypedEvent();

    public T Value
    {
        get => _value;
        set => SetValue(value);
    }

    private void OnEnable()
    {
        if (!_isReadonly && _initialize)
            _value = _initialValue;
    }

    /// <summary>
    /// Updates value and raises events.
    /// </summary>
    /// <param name="newValue">New value</param>
    public void SetValue(T newValue)
    {
        //constant value?
        if (_isReadonly)
        {
            Debug.LogWarning("[" + GetType().Name + "] " +
                "Attempting to change value of readonly variable.", this);
            return;//don't change anything
        }

        _value = newValue;
        onValueChangedEvent.Invoke();
        onValueChangedTypedEvent.Invoke(_value);
    }

    public static implicit operator T (AVariable<T> a)
        => a._value; //implicitly use this ref as its base value
}
