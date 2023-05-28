using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class screenManager : MonoBehaviour
{
    public int ClickCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.R)) 
        {
            ClickCount++;
            if (!IsInvoking("DoubleClick"))
                Invoke("DoubleClick", 1.0f);
       
        }
       
        else if (ClickCount == 2)
        {
            CancelInvoke("DoubleClick");
            Application.Quit();
        }


        if (Input.GetMouseButtonDown(0) && SceneManager.GetActiveScene().name == "First")
        {
            SceneManager.LoadScene("Main");
            
        }

       
    }

    public void DoubleClick()
    {
        ClickCount = 0;
    }
}
