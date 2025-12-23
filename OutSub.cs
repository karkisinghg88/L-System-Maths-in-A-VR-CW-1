using UnityEngine;

public class OutSub : MonoBehaviour
{
    public LSystem LSystem;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            LSystem.iterations -= 1 ;
            LSystem.Generate();
        }
    }
}
