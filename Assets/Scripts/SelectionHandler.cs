using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

[RequireComponent(typeof(InputSystemUIInputModule), typeof(EventSystem))]
public class SelectionHandler : MonoBehaviour
{
    [SerializeField] Selectable _defaultSelectable;
    InputSystemUIInputModule _inputModule;
    EventSystem _eventSystem;

    void Awake()
    {
        _inputModule = GetComponent<InputSystemUIInputModule>();
        _eventSystem = GetComponent<EventSystem>();
    }

    public void SetDefaultSelectable(Selectable selectable)
    {
        _defaultSelectable = selectable;
    }

    void OnEnable()
    {
        _inputModule.move.action.performed += OnMove;
    }
    
    void OnDisable()
    {
        _inputModule.move.action.performed -= OnMove;
    }
    
    void OnMove(InputAction.CallbackContext ctx)
    {
        if (Mathf.Approximately(ctx.ReadValue<Vector2>().magnitude, 0.0f))
        {
            return;
        }
        
        GameObject selected = _eventSystem.currentSelectedGameObject;
        if (selected != null && selected.activeInHierarchy && _defaultSelectable.gameObject.activeInHierarchy)
        {
            return;
        }
        
        _defaultSelectable?.Select();
    }
}
