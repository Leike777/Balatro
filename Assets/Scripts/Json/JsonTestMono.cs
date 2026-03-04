using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonTestMono : MonoBehaviour
{
    public TestJson testJson = new TestJson();
    
    private void Start()
    {
        testJson.name = "崔文宝";
        testJson.age = 20;
        testJson.sex = true;
        
        JsonMgr.Instance.SaveData(testJson, "test") ;
        TestJson t = JsonMgr.Instance.LoadData<TestJson>("test");
    }
}

public class TestJson
{
    public string name;
    public int age;
    public bool sex;
}
