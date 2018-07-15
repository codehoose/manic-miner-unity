using UnityEngine;

public class DebugMode : MonoBehaviour
{
    int index = 0;

    float lastTime;

    public KeyCode[] phrase;

    public bool debugEnabled;
	
	void Update ()
    {
        if (Input.GetKeyUp(phrase[index]))
        {
            print(phrase[index]);

            lastTime = Time.realtimeSinceStartup;
            index++;

            if (index == phrase.Length)
            {
                debugEnabled = !debugEnabled;
                index = 0;
            }
        }

        if (Time.realtimeSinceStartup > lastTime + 0.5f)
        {
            index = 0;
        }
	}
}
