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

    void Update()
    {
        // Cập nhật điểm liên tục khi shop đang mở
        if (shopPanel.activeInHierarchy)
        {
            UpdateShopUI();
        }
    }

    void CreateShopItems()
    {
        // Clear existing buttons
        foreach (Transform child in itemContainer)
        {
            Destroy(child.gameObject);
        }
        itemButtons.Clear();

        foreach (ShopItem item in shopItems)
        {

            GameObject buttonObj = Instantiate(itemButtonPrefab, itemContainer);
            Button itemButton = buttonObj.GetComponent<Button>();

            if (itemButton == null)
            {

                continue;
            }



            // Setup button appearance - SỬA LỖI TÌM COMPONENT
            SetupButtonAppearance(buttonObj, item);

            // Add click listener với debug
            ShopItem currentItem = item; // Capture for closure
            itemButton.onClick.AddListener(() => {
                BuyItem(currentItem);
            });

            itemButtons.Add(itemButton);
        }
    }

    private void SetupButtonAppearance(GameObject buttonObj, ShopItem item)
    {
      
        Transform itemIconTransform = buttonObj.transform.Find("ItemIcon");
        Transform itemNameTransform = buttonObj.transform.Find("ItemName");
        Transform priceTransform = buttonObj.transform.Find("Price");

        // Cách 2: Tìm bằng GetComponentsInChildren (backup)
        if (itemNameTransform == null || priceTransform == null)
        {
            Text[] allTexts = buttonObj.GetComponentsInChildren<Text>();
            TextMeshProUGUI[] allTMPs = buttonObj.GetComponentsInChildren<TextMeshProUGUI>();

            // Gán text cho các component tìm được
            if (allTexts.Length > 0) allTexts[0].text = item.itemName;
            if (allTexts.Length > 1) allTexts[1].text = item.price.ToString();

            if (allTMPs.Length > 0) allTMPs[0].text = item.itemName;
            if (allTMPs.Length > 1) allTMPs[1].text = item.price.ToString();
        }
    }

    // THÊM HÀM MỚI: Kiểm tra xem item có thể mua được không
    private bool CanBuyItem(ShopItem item)
    {
        if (gameManager == null) return false;

        // Kiểm tra đủ điểm
        if (gameManager.CurrentScore < item.price) return false;

        // Kiểm tra điều kiện đặc biệt cho từng loại item
        switch (item.type)
        {
            case ItemType.Health:
                // Chỉ cho phép mua heal nếu chưa full máu
                return !gameManager.IsFullHealth();

            case ItemType.Weapon:
                // Có thể thêm logic kiểm tra weapon ở đây
                return true;

            case ItemType.PowerUp:
                // Có thể thêm logic kiểm tra power-up ở đây
                return true;

            default:
                return true;
        }
    }

    public void BuyItem(ShopItem item)
    {

        // Kiểm tra GameManager
        if (gameManager == null)
        {
            return;
        }

        // SỬA: Sử dụng hàm CanBuyItem thay vì chỉ kiểm tra điểm
        if (CanBuyItem(item) && gameManager.SpendScore(item.price))
        {
            // Áp dụng hiệu ứng item
            ApplyItemEffect(item);

            // Update UI sau khi mua
            UpdateShopUI();
        }
        else
        {
            // Thông báo lý do không thể mua (tuỳ chọn)
            if (item.type == ItemType.Health && gameManager.IsFullHealth())
            {
                Debug.Log("Health is already full!");
            }
            else if (gameManager.CurrentScore < item.price)
            {
                Debug.Log("Not enough score!");
            }
        }
    }

    private void ApplyItemEffect(ShopItem item)
    {
        if (gameManager.IsGameOver())
        {
            return;
        }

        switch (item.type)
        {
            case ItemType.Health:
                gameManager.Heal(1);
                break;

            case ItemType.Weapon:
               
                break;

            case ItemType.PowerUp:
               
                break;
        }
    }

    private void UpdateShopUI()
    {
        if (playerScoreText)
            playerScoreText.text = "Score: " + gameManager.CurrentScore.ToString();

        for (int i = 0; i < itemButtons.Count; i++)
        {
            if (i < shopItems.Length)
            {
                // SỬA: Sử dụng CanBuyItem thay vì chỉ kiểm tra điểm
                bool canBuy = CanBuyItem(shopItems[i]);
                itemButtons[i].interactable = canBuy;

                ColorBlock colors = itemButtons[i].colors;
                colors.normalColor = canBuy ? Color.white : Color.gray;
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