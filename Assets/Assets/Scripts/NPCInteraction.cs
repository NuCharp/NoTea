using UnityEngine;
using TMPro;  // �� ������ �������� ���� using ��� TextMeshPro

public class NPCInteraction : MonoBehaviour
{
    public string[] randomTexts;  // ������ ��������� ������� ��� NPC
    public TMP_Text dialogueText;  // ���������� TMP_Text ��� TextMeshPro

    private bool isPlayerInRange = false;  // ����, ����� �����, ����� �� �����

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
