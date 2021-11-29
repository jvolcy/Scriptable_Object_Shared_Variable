using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sphere_controller : MonoBehaviour
{
    public FloatVariable SO_MyVar_;
    float DEFAULT_MYVAR_VALUE = 10f;
    NullableReferenceVariable<float> MyVar;

    // Start is called before the first frame update
    void Start()
    {
        //connect the nullable to the SO variable and the local variable;
        //initialize the local variable to some default value
        MyVar = new NullableReferenceVariable<float>(SO_MyVar_, DEFAULT_MYVAR_VALUE);

        Debug.Log("MyVar = " + MyVar.value.ToString());
        
    }

    // Update is called once per frame
    void Update()
    {
        //connect and disconnect the SO on the Inspector window to watch
        //either the SO or the local variable increment.

        if (Time.frameCount % 100 == 0)
        {
            MyVar.value++;
            Debug.Log(MyVar.value);
        }
    }
}
