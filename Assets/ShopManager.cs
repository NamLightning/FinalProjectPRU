using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ShopItem
{
    public string itemName;
    public int price;
    public Sprite itemIcon;
    public ItemType type;
}

public enum ItemType
{
    Health,
    Weapon,
    PowerUp
}

public class ShopManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject shopPanel;
    public Transform itemContainer;
    public GameObject itemButtonPrefab;
    public TextMeshProUGUI playerScoreText;
    public Button closeButton;

    [Header("Shop Items")]
    public ShopItem[] shopItems;

    [Header("References")]
    public GameManager gameManager;
    public ShopNPC shopNPC;

    private List<Button> itemButtons = new List<Button>();

    void Start()
    {
        // Setup close button
        if (closeButton) closeButton.onClick.AddListener(CloseShop);

        // Tạo item buttons
        CreateShopItems();

        // Đóng shop khi start
        if (shopPanel) shopPanel.SetActive(false);
    }

    void CreateShopItems()
    {
        foreach (ShopItem item in shopItems)
        {
            GameObject buttonObj = Instantiate(itemButtonPrefab, itemContainer);
            Button itemButton = buttonObj.GetComponent<Button>();

            // Setup button appearance
            Image itemImage = buttonObj.transform.Find("ItemIcon").GetComponent<Image>();
            Text itemNameText = buttonObj.transform.Find("ItemName").GetComponent<Text>();
            Text priceText = buttonObj.transform.Find("Price").GetComponent<Text>();

            if (itemImage) itemImage.sprite = item.itemIcon;
            if (itemNameText) itemNameText.text = item.itemName;
            if (priceText) priceText.text = item.price.ToString() + " điểm";

            // Add click listener
            ShopItem currentItem = item; // Capture for closure
            itemButton.onClick.AddListener(() => BuyItem(currentItem));

            itemButtons.Add(itemButton);
        }
    }

    public void BuyItem(ShopItem item)
    {
        if (gameManager.SpendScore(item.price))
        {
            // Áp dụng hiệu ứng item
            ApplyItemEffect(item);

            // Update UI sau khi mua
            UpdateShopUI();

            Debug.Log($"Đã mua {item.itemName} với giá {item.price} điểm!");
        }
        else
        {
            Debug.Log("Không đủ điểm để mua item này!");
            // TODO: Hiển thị thông báo UI nếu cần
        }
    }

    private void ApplyItemEffect(ShopItem item)
    {
        if (gameManager.IsGameOver()) return;

        switch (item.type)
        {
            case ItemType.Health:
                gameManager.Heal(1);
                break;

            case ItemType.Weapon:
                Debug.Log("Weapon upgrade applied!");
                break;

            case ItemType.PowerUp:
                Debug.Log("Power-up applied!");
                break;
        }
    }

    private void UpdateShopUI()
    {
        if (playerScoreText)
            playerScoreText.text = "Điểm: " + gameManager.CurrentScore;

        for (int i = 0; i < itemButtons.Count; i++)
        {
            if (i < shopItems.Length)
            {
                bool canAfford = gameManager.CurrentScore >= shopItems[i].price;
                itemButtons[i].interactable = canAfford;

                ColorBlock colors = itemButtons[i].colors;
                colors.normalColor = canAfford ? Color.white : Color.gray;
                itemButtons[i].colors = colors;
            }
        }
    }

    public void CloseShop()
    {
        shopPanel.SetActive(false);
        if (shopNPC) shopNPC.CloseShop();
    }

    void OnEnable()
    {
        UpdateShopUI();
    }
}