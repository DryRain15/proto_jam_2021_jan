using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
    public Text screen;
    public string NextStage;
    
    public float targetAlpha;
    private float currentAlpha;

    private float timer;
    public float duration;

    private bool isStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        screen.color = Color.black;
        targetAlpha = 0f;
        currentAlpha = 1f;
        timer = 0f;
        duration = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(currentAlpha - targetAlpha) < 0.001f)
        {
            currentAlpha = targetAlpha;
            screen.color = new Color(0, 0, 0, targetAlpha);
            timer = 0f;

            if (targetAlpha >= 1f)
                targetAlpha = 0f;
            else
                targetAlpha = 1f;
        }

        timer += Time.deltaTime;
        currentAlpha = Mathf.Lerp(currentAlpha, targetAlpha, timer / duration);
        screen.color = new Color(0, 0, 0, currentAlpha);

        if (Input.anyKeyDown && !isStarted)
        {
            isStarted = true;
            ToNextStage();
        }
    }
    
    public void ToNextStage()
    {
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        FadeController.self.SetFade(1, 2);
        
        yield return new WaitForSeconds(2f);
        
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(NextStage);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        
        Destroy(this.gameObject);
    }
}
