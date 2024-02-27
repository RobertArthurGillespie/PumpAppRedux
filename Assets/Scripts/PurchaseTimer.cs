using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PurchaseTimer : MonoBehaviour
{
    public IEnumerator LoadSplashScreen()
    {
        yield return new WaitForSeconds(30f);
        SceneManager.LoadScene(1);
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadSplashScreen());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
