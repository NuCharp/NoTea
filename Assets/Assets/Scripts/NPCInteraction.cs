using UnityEngine;
using TMPro;  // Не забудь добавить этот using для TextMeshPro

public class NPCInteraction : MonoBehaviour
{
    public string[] randomTexts;  // Список случайных текстов для NPC
    public TMP_Text dialogueText;  // Используем TMP_Text для TextMeshPro

    private bool isPlayerInRange = false;  // Флаг, чтобы знать, рядом ли игрок

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ShowRandomText();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            dialogueText.text = "";
        }
    }

    private void ShowRandomText()
    {
        if (randomTexts.Length > 0)
        {
            int randomIndex = Random.Range(0, randomTexts.Length);
            dialogueText.text = randomTexts[randomIndex];
        }
    }
}
