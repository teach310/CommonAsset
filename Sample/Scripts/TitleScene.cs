using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{

    public void GoMenu() {
        FirebaseClient.LoadMaster().OnComplete(() => SceneManager.LoadScene("SampleMenu"));
    }
}
