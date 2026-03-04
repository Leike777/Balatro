using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AsyncTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Func();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Func()
    {
        FuncAsync1();
        print("Func");
    }

    public async void FuncAsync1()
    {
        await FuncAsync2();
        print ("FuncAsync1");
    }
    
    public async Task FuncAsync2()
    {
        print ("FuncAsync2");
        await Task.Delay(1000);
        
    }
}
