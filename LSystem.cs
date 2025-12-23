using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using UnityEngine.SceneManagement;

public class TransformInfo
{
    public Vector3 position;
    public Quaternion rotation;
}

public class LSystem : MonoBehaviour
{
    [Header("Tree 1")]
    public TMP_Dropdown myDropdown2;
    public TMP_InputField Angle2;
    public TMP_InputField Rule01;



    [Header("Tree 2")]

    public TMP_Dropdown myDropdown;
    public TMP_InputField Axiom, Angle;
    public TMP_InputField Rule02;



    [Header("Main Screen")]

    public RawImage MainScreenImage;
    public Button StartButton;
    public Button RestartButton;

    [Header("Other")]



    public TMP_Text IterationText;


    public event Action OnLSystemGenerated;  

     public int iterations;
    [SerializeField] private GameObject Branch;
    [SerializeField] private float length;
    public float angle = 30f;


    private const string axiom = "X";
    private Stack<TransformInfo> transformStack;
    private Dictionary<char, string> rules;
    private string currentString = string.Empty;

    

    public string F1;

    public string X1;

    public void Re_Start()
    {
        MainScreenImage.enabled = true;
        StartButton.gameObject.SetActive(true);
        RestartButton.gameObject.SetActive(false);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
          //  UnityEditor.EditorApplication.isPlaying = false;
       
             Application.Quit();
    }


    public void clickStart()
    {
        MainScreenImage.enabled = false;
        StartButton.gameObject.SetActive(false);
        RestartButton.gameObject.SetActive(true);


    }


    public void Update()
    {
        IterationText.text = iterations.ToString();

    }
    

    public void Generate1()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        string rule02 = Rule02.text;
        string dropdown02 = myDropdown.options[myDropdown.value].text;

        if (rule02 == "")
        {
            rule02 = dropdown02;
        }



        string selectedText = rule02;


        string currentText = Axiom.text;


        if (float.TryParse(Angle.text, out float newAngle))
            angle = newAngle;



        transformStack = new Stack<TransformInfo>();
        rules = new Dictionary<char, string>


        {
            { 'F', currentText },
            { 'X', selectedText }
        };

     
        Generate();

    }

    public void Generate2()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        string rule01 = Rule01.text;
        string dropdown01 = myDropdown2.options[myDropdown2.value].text;

        if (rule01 == "")
        {
            rule01 = dropdown01;
        }

        string selectedText2 = rule01;
       

        if (float.TryParse(Angle2.text, out float newAngle2))
            angle = newAngle2;
       

        transformStack = new Stack<TransformInfo>();
        rules = new Dictionary<char, string>

        

        {
            { 'F', selectedText2 },
            { 'X', X1 }
        };


        Generate();

    }


   /* public void Start()
    {
        transformStack = new Stack<TransformInfo>();
        rules = new Dictionary<char, string>
        {
            { 'F', F1 },
            { 'X', X1 }
        };

        Generate();
        
    }*/

    public void IncreaseIR()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        transform.position = Vector3.zero;  
        transform.rotation = Quaternion.identity;

        iterations++;

        Generate();
     

    }
    public void DecreaseIR()
    {

       

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        transform.position = Vector3.zero;     
        transform.rotation = Quaternion.identity;

        if (iterations > 1)
        iterations--;

        Generate();
        
    }

    public void Generate()
    {
        currentString = axiom;
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < iterations; i++)
        {
            foreach (char c in currentString)
                sb.Append(rules.ContainsKey(c) ? rules[c] : c.ToString());

            currentString = sb.ToString();
            sb = new StringBuilder();
           
        }

        foreach (char c in currentString)
        {
            switch (c)
            {
                case 'F':
                    Vector3 initialPosition = transform.position;
                    transform.Translate(Vector3.up * length);
                    GameObject treeSegment = Instantiate(Branch, transform);
                    LineRenderer lr = treeSegment.GetComponent<LineRenderer>();
                    lr.SetPosition(0, initialPosition);
                    lr.SetPosition(1, transform.position);
                    break;

                case '+':
                    transform.Rotate(Vector3.back * angle);
                    break;

                case '-':
                    transform.Rotate(Vector3.forward * angle);
                    break;

                case '[':
                    transformStack.Push(new TransformInfo
                    {
                        position = transform.position,
                        rotation = transform.rotation
                    });
                    break;

                case ']':
                    TransformInfo tiPop = transformStack.Pop();
                    transform.position = tiPop.position;
                    transform.rotation = tiPop.rotation;
                    break;
            }
        }

        OnLSystemGenerated?.Invoke();    // << ADDED
    }
}
