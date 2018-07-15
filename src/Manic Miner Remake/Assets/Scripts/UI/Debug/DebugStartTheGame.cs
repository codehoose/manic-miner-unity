using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class DebugStartTheGame : MonoBehaviour
{
    public string theGame = "Room Store Test";

    void Awake()
    {
        var button = GetComponent<Button>();
        button.onClick.AddListener(new UnityAction(
        () =>
        {
            PlayerPrefs.SetInt("_room", 0);
            PlayerPrefs.SetInt("_score", 0);
            SceneManager.LoadScene(theGame);
        }
        ));
    }
}
