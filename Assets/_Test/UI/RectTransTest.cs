using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public Image img;
    private Image img1;
    [SerializeField]private Image img2;
    protected Image img3;
    public RectTransform rectTrans;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("tranform.position:" + transform.position);
        Debug.Log("rectTrans.position:" + rectTrans.position);
        
        Debug.Log("tranform.localPosition:" + transform.localPosition);
        Debug.Log("rectTrans.localPosition" + rectTrans.localPosition);
        
        Debug.Log("rectTrans.anchoredPosition" + rectTrans.anchoredPosition);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
