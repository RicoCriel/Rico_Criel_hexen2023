using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

class ButtonView: MonoBehaviour
{
    [SerializeField] private Button _undoButton;

    [SerializeField] private UnityEvent OnUndo;

    private void OnEnable()
    {
        _undoButton.onClick.AddListener(Undo);
    }

    private void OnDisable()
    {
        _undoButton.onClick.RemoveListener(Undo);
    }

    private void Undo()
    {
        OnUndo?.Invoke();
    }
}
