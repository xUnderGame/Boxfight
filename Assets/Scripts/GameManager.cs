using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public static GameManager Instance;
    [HideInInspector] public GameUI gameUI = new();
    [HideInInspector] public GameObject playerObject;
    [HideInInspector] public Player player;

    public void Awake()
    {
        // GameManager Instance
        if (Instance) { Destroy(gameObject); return; }
        else Instance = this;

        // Misc setup..
        playerObject = GameObject.Find("Player");
        player = playerObject.GetComponent<Player>();

        // Setting GameUI stuff up!
        gameUI.main = GameObject.Find("Game UI");
        gameUI.weapons = gameUI.main.transform.Find("Weapons").gameObject;
        gameUI.primaryWeapon = gameUI.weapons.transform.Find("Primary").gameObject;
        gameUI.secondaryWeapon = gameUI.weapons.transform.Find("Secondary").gameObject;
        gameUI.hpValue = gameUI.main.transform.Find("HP Bar").Find("Value").GetComponent<Text>();
        gameUI.manaValue = gameUI.main.transform.Find("Mana Bar").Find("Value").GetComponent<Text>();
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

        // Updates the energy UI
        public void UpdateEnergyUI() { manaValue.text = $"{Instance.player.currentEnergy}/{Instance.player.maxEnergy}"; }

        // Updates the HP UI
        public void UpdateHealthUI() { hpValue.text = $"{Instance.player.currentHP}/{Instance.player.maxHP}"; }

        // Updates the weapons UI
        public void UpdateWeaponsUI(InventoryScriptable sc)
        {
            // Primary weapon sprite and mana cost
            primaryWeapon.transform.Find("Sprite").GetComponent<RawImage>().texture
            = sc.activeWeapon.weaponSprite.texture;

            primaryWeapon.transform.Find("Energy Cost").GetComponent<Text>().text =
            sc.activeWeapon.energyCost.ToString();

            // Secondary weapon sprite and mana cost (HARDCODED!!! Kinda...?)
            if (sc.weapons.Count < 2) return;
            secondaryWeapon.transform.Find("Sprite").GetComponent<RawImage>().texture
            = sc.weapons[1].weaponSprite.texture;
            
            secondaryWeapon.transform.Find("Energy Cost").GetComponent<Text>().text =
            sc.weapons[1].energyCost.ToString();
        }
    }
}
