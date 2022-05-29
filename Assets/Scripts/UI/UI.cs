
using UnityEngine;

public class UI : MonoBehaviour
{
    protected GameEvents gameEvents;

    protected void Start()
    {
        gameEvents = GameEvents.instance;
        AddEvents();
    }

    private void AddEvents()
    {
        gameEvents.OnNextUIEvent += OnNextUI;
    }

    private void RemoveEvents()
    {
        gameEvents.OnNextUIEvent -= OnNextUI;
    }

    protected void OnDestroy()
    {
        RemoveEvents();
    }

    private void OnNextUI(UI ui)
    {
        Instantiate(ui, transform.parent);

        Destroy(gameObject);
    }
}
