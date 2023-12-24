using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public static GameManager Instance;
    [HideInInspector] public GameUI gameUI = new();
    [HideInInspector] public GameObject playerObject;
    [HideInInspector] public Player player;
    [HideInInspector] public GameObject pickupPool;
    [HideInInspector] public GameObject bulletPool;
    [HideInInspector] public GameObject energyBitPrefab;
    public GameObject nearestInteractable;

    public void Awake()
    {
        // GameManager Instance
        if (Instance) { Destroy(gameObject); return; }
        else Instance = this;

        // Misc setup...
        energyBitPrefab = (GameObject)Resources.Load("Prefabs/Interactables/Energy Bit");
        pickupPool = GameObject.Find("Pickup Pool");
        bulletPool = GameObject.Find("Bullet Pool");
        playerObject = GameObject.Find("Player");
        player = playerObject.GetComponent<Player>();

        // Setting GameUI stuff up!
        gameUI.main = GameObject.Find("Game UI");
        gameUI.hpValue = gameUI.main.transform.Find("HP Bar").Find("Value").GetComponent<Text>();
        gameUI.manaValue = gameUI.main.transform.Find("Mana Bar").Find("Value").GetComponent<Text>();

        gameUI.weapons = gameUI.main.transform.Find("Weapons").gameObject;
        gameUI.primaryWeapon = gameUI.weapons.transform.Find("Primary").gameObject;
        gameUI.secondaryWeapon = gameUI.weapons.transform.Find("Secondary").gameObject;
        gameUI.dialogbox = gameUI.main.transform.Find("Dialogbox").gameObject;

        gameUI.dialogName = gameUI.dialogbox.transform.Find("Name").gameObject.GetComponent<Text>();
        gameUI.dialogText = gameUI.dialogbox.transform.Find("Text").gameObject.GetComponent<Text>();

    }

    // Changes to a different scene.
    public void ChangeScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    // GameUI class for better navigation and structure.
    public class GameUI {
        // Main references
        public GameObject main;
        public Text hpValue;
        public Text manaValue;

        // Weapons
        public GameObject weapons;
        public GameObject primaryWeapon;
        public GameObject secondaryWeapon;

        // Dialog
        public GameObject dialogbox;
        public Text dialogName;
        public Text dialogText;

        // Updates the energy UI
        public void UpdateEnergyUI() { manaValue.text = $"{Instance.player.currentEnergy}/{Instance.player.maxEnergy}"; }

        // Updates the HP UI
        public void UpdateHealthUI() { hpValue.text = $"{Instance.player.currentHP}/{Instance.player.maxHP}"; }

        // Updates the weapons UI
        public void UpdateWeaponsUI(InventoryScriptable sc, int oldIndex = 1)
        {
            // Primary weapon sprite and mana cost
            RawImage primaryImage = primaryWeapon.transform.Find("Sprite").GetComponent<RawImage>();
            primaryImage.texture = sc.activeWeapon.weaponSprite.texture;
            // primaryImage.color = sc.activeWeapon.gameObject.GetComponent<SpriteRenderer>().color;

            primaryWeapon.transform.Find("Energy Cost").GetComponent<Text>().text =
            sc.activeWeapon.energyCost.ToString();

            // Secondary weapon sprite and mana cost
            if (sc.weapons.Count != sc.weapons.Capacity) return;
            RawImage secondaryImage = secondaryWeapon.transform.Find("Sprite").GetComponent<RawImage>();
            secondaryImage.texture = sc.weapons[oldIndex].weaponSprite.texture;
            // secondaryImage.color = sc.weapons[oldIndex].gameObject.GetComponent<SpriteRenderer>().color;
            
            secondaryWeapon.transform.Find("Energy Cost").GetComponent<Text>().text =
            sc.weapons[oldIndex].energyCost.ToString();
        }

        // Toggle ON/OFF dialog box UI
        public void ToggleDialogBox() { dialogbox.SetActive(!dialogbox.activeSelf); }
    }
}
