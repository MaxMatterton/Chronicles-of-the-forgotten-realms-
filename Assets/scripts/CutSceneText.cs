using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Timeline;

public class CutSceneText : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    public string[] textLines;
    public float textSpeed = 0.05f;

    private int index;

    void Start()
    {
        textMesh.text = string.Empty;
        StartDialoge();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (textMesh.text == textLines[index])
            {
                nextline();
            }
            else{
                StopAllCoroutines();
                textMesh.text = textLines[index];
            }
        }
    }
    void StartDialoge()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }
    IEnumerator TypeLine()
    {
        foreach (char line in textLines[index].ToCharArray())
        {
            textMesh.text += line;
            yield return new WaitForSeconds(textSpeed);
        }
    }
    void nextline()
    {
        if (index < textLines.Length - 1)
        {
            index ++;
            textMesh.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            textMesh.text = string.Empty;
        }
    }
}
