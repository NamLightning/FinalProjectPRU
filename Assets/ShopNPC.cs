using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopNPC : MonoBehaviour
{
    [Header("UI References")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public Button yesButton;
    public Button noButton;
    public GameObject shopPanel;

    [Header("Shop Settings")]
    public float interactionRange = 3f;
    public string npcName = "Shop Keeper";

    private Transform player;
    private bool playerInRange = false;
    private bool dialogueActive = false;

    void Start()
    {
        // Tìm player
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Setup buttons
        if (yesButton) yesButton.onClick.AddListener(OnYesClicked);
        if (noButton) noButton.onClick.AddListener(OnNoClicked);
    }

    void Update()
    {
        CheckPlayerDistance();

        // Kiểm tra input để mở shop
        if (playerInRange && !dialogueActive && Input.GetKeyDown(KeyCode.E))
        {
            ShowDialogue();
        }
    }

    void CheckPlayerDistance()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            playerInRange = distance <= interactionRange;
        }
    }

    void ShowDialogue()
    {
        dialogueActive = true;
        dialoguePanel.SetActive(true);
        dialogueText.text = $"{npcName}: Hi! Would you like to buy some items like health ?";

        // Tạm dừng game (tùy chọn)
        Time.timeScale = 0f;
    }

    public void OnYesClicked()
    {
        CloseDialogue();
        OpenShop();
    }

   public void OnNoClicked()
    {
        CloseDialogue();
    }

   public void CloseDialogue()
    {
        dialogueActive = false;
        dialoguePanel.SetActive(false);
        Time.timeScale = 1f; // Tiếp tục game
    }

    public void OpenShop()
    {

        dialogueActive = false;
        dialoguePanel.SetActive(false);
        shopPanel.SetActive(true);
        Time.timeScale = 0f; // Tạm dừng game khi mở shop
    }

    // Method này có thể được gọi từ ShopManager khi đóng shop
    public void CloseShop()
    {
        shopPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    // Hiển thị indicator khi player trong tầm
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
