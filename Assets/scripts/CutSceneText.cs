using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Playables;

public class CutSceneText : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    public string[] PlayerLines;
    public string[] EnemyLines;
    public float textSpeed = 0.05f;

    private int index;

    void Start()
    {
        textMesh.text = string.Empty;
    }
    void Update()
    {

        
    }

    // 🟢 This function will be triggered by the Signal Receiver
    // public void StartDialogue()
    // {
    //     index = 0;
    //     textMesh.text = string.Empty; // Reset text
    //     StartCoroutine(());
    // }

    IEnumerator TypePlayerLines()
    {
        foreach (char letter in PlayerLines[index].ToCharArray())
        {

            textMesh.text += letter;
            yield return new WaitForSeconds(textSpeed);

        }
    }

    public void PlayerSpeaking()
    {
        if (index < PlayerLines.Length - 1)
        {
            index++;
            textMesh.text = string.Empty;
            StartCoroutine(TypePlayerLines());
        }
        else
        {
            textMesh.text = string.Empty; // Clear text when dialogue ends
        }
    }

    public void NextLineNPC()
    {
        if (index < EnemyLines.Length - 1)
        {
            index++;
            textMesh.text = string.Empty;
            StartCoroutine(TypePlayerLines());
        }
        else
        {
            textMesh.text = string.Empty; // Clear text when dialogue ends
        }
    }
    public void PauseTimeline()
    {

    }

}
