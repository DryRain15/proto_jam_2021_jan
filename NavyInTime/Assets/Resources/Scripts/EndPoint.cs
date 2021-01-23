using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPoint : MonoBehaviour
{
    public string NextStage;

    public BoxCollider2D Collider2D;
    
    private void Update()
    {
        var pos = transform.position;

        var ray = Physics2D.BoxCastAll(pos, Collider2D.size, 
            0, Vector2.zero, 0, (1 << 16));

        if (ray.Length > 0)
        {
            foreach (var hit in ray)
            {
                if (hit.collider.gameObject == LittleGirl.self.gameObject)
                {
                    ToNextStage();
                    print("load " + NextStage);
                    Collider2D.enabled = false;
                }
            }
        }
    }

    public void ToNextStage()
    {
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        ReversablePoolController.self.StopAll();
        
        FadeController.self.SetFade(1, 1);
        
        yield return new WaitForSeconds(1.1f);
        
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(NextStage);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        
        Destroy(this.gameObject);
    }
}
