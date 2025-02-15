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
    }

    // 🟢 This function will be triggered by the Signal Receiver
    public void StartDialogue()
    {
        index = 0;
        textMesh.text = string.Empty; // Reset text
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char letter in textLines[index].ToCharArray())
        {
            textMesh.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    public void NextLine()
    {
        if (index < textLines.Length - 1)
        {
            index++;
            textMesh.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            textMesh.text = string.Empty; // Clear text when dialogue ends
        }
    }

}
