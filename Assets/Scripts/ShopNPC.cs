using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ShopNPC : MonoBehaviour
{
    [Header("UI References")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public Button yesButton;
    public Button noButton;
    public GameObject shopPanel;
    public GameObject instructionText;

    [Header("Shop Settings")]
    public float interactionRange = 3f;
    public string npcName = "Shop Keeper";
    public string instructionMessage = "Press E to talk";

    private Transform player;
    private bool playerInRange = false;
    private bool dialogueActive = false;
    private bool shopActive = false; // Thêm biến để theo dõi trạng thái shop

    void Start()
    {
        // Tìm player
        player = GameObject.FindGameObjectWithTag("Player").transform;

      
        if (dialoguePanel) dialoguePanel.SetActive(false);
        if (shopPanel) shopPanel.SetActive(false);

        // Setup buttons
        if (yesButton) yesButton.onClick.AddListener(OnYesClicked);
        if (noButton) noButton.onClick.AddListener(OnNoClicked);
    }

    void Update()
    {
        CheckPlayerDistance();

        // Hiển thị/ẩn instruction text
        if (playerInRange && !dialogueActive && !shopActive)
        {
            if (instructionText)
            {
                instructionText.SetActive(true);
                instructionText.GetComponent<TextMeshProUGUI>().text = instructionMessage;
            }
        }
        else
        {
            if (instructionText) instructionText.SetActive(false);
        }

        // Kiểm tra input để mở shop
        if (playerInRange && !dialogueActive && !shopActive && Input.GetKeyDown(KeyCode.E))
        {
            ShowDialogue();
        }

        // Đóng dialogue khi player ra khỏi tầm
        if (!playerInRange && dialogueActive)
        {
            CloseDialogue();
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
        instructionText.SetActive(false);
        dialogueActive = true;
        dialoguePanel.SetActive(true);
        dialogueText.text = $"{npcName}: Hi! Would you like to buy some items?";

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
        shopActive = true; // Set shop active
        shopPanel.SetActive(true);
       
        Time.timeScale = 0f; // Tạm dừng game khi mở shop
    }


    // Method này có thể được gọi từ ShopManager khi đóng shop
    public void CloseShop()
    {
        shopActive = false; // Set shop inactive
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
