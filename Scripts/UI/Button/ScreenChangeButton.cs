using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ScreenChangeButton : MonoBehaviour
{

    public bool goRoot = false;
    [SerializeField] WindowPresenter window;
    [SerializeField] ScreenPresenter screen;

    void Awake()
    {
        this.GetComponent<Button>().onClick.AddListener(ChangeScreen);
    }

    public void ChangeScreen() {
        if (goRoot)
            ScenePresenter.Instance.GoRootScreen(window.name);
        else
            ScenePresenter.Instance.MoveScreen(screen.name);
    }
}
