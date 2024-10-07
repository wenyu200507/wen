using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Confirm : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CloseBtn()
    {
        UIManager.Instance.CloseUI("UI_Confirm",true);
    }

    public void ConfirmBtn()
    {
        RunGame.Instance.exit();
    }
}
