using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Golf.Golf.Source;
using Golf.GamePlay;
using Golf.Golf.GamePlay;
using Golf.Golf.States.Interfaces;

namespace Golf.Golf.States
{
    public class ShopState : IGameState
    {
        private const int PickaxePrice = 5;
        private const int HammerPrice = 10;
        private const int SecondMapPrice = 1;

        private const string PickaxeOwnedKey = "PickaxeOwned";
        private const string HammerOwnedKey = "HammerOwned";
        private const string SelectedModelKey = "SelectedStickModel";

        private const string SecondMapOwnedKey = "SecondMapOwned";
        private const string SelectedMapKey = "SelectedMap";

        private readonly GameplayArea m_firstArea;
        private readonly GameplayArea m_secondArea;
        private readonly Camera m_shopCamera;

        private readonly GameObject m_shopHud;

        private readonly Button m_backButton;
        private readonly Button m_pickaxeButton;
        private readonly Button m_hammerButton;
        private readonly Button m_hoeButton;
        private readonly Button m_secondMapButton;

        private readonly TMP_Text m_balanceText;
        private readonly TMP_Text m_pickaxeButtonText;
        private readonly TMP_Text m_hammerButtonText;
        private readonly TMP_Text m_hoeButtonText;
        private readonly TMP_Text m_secondMapButtonText;

        private readonly BalanceService m_balance;
        private readonly Stick m_stick;
        private readonly AudioSource m_shopMusic;

        private readonly Action m_returnToMenu;
        private readonly Action<int> m_selectMap;

        public ShopState(
            GameplayArea firstArea,
            GameplayArea secondArea,
            Camera shopCamera,
            GameObject shopHud,
            Button backButton,
            Button pickaxeButton,
            Button hammerButton,
            TMP_Text balanceText,
            TMP_Text pickaxeButtonText,
            TMP_Text hammerButtonText,
            Button hoeButton,
            TMP_Text hoeButtonText,
            Button secondMapButton,
            TMP_Text secondMapButtonText,
            BalanceService balance,
            Stick stick,
            AudioSource shopMusic,
            Action returnToMenu,
            Action<int> selectMap)
        {
            m_firstArea = firstArea;
            m_secondArea = secondArea;
            m_shopCamera = shopCamera;
            m_shopHud = shopHud;

            m_backButton = backButton;
            m_pickaxeButton = pickaxeButton;
            m_hammerButton = hammerButton;
            m_hoeButton = hoeButton;
            m_secondMapButton = secondMapButton;

            m_balanceText = balanceText;
            m_pickaxeButtonText = pickaxeButtonText;
            m_hammerButtonText = hammerButtonText;
            m_hoeButtonText = hoeButtonText;
            m_secondMapButtonText = secondMapButtonText;

            m_balance = balance;
            m_stick = stick;
            m_shopMusic = shopMusic;

            m_returnToMenu = returnToMenu;
            m_selectMap = selectMap;
        }

        public void Enter()
        {
            m_firstArea.AreaCamera.enabled = false;
            m_secondArea.AreaCamera.enabled = false;
            m_shopCamera.enabled = true;

            m_shopHud.SetActive(true);

            m_backButton.onClick.RemoveAllListeners();
            m_backButton.onClick.AddListener(
                () => m_returnToMenu?.Invoke()
            );

            m_pickaxeButton.onClick.RemoveAllListeners();
            m_pickaxeButton.onClick.AddListener(BuyOrEquipPickaxe);

            m_hammerButton.onClick.RemoveAllListeners();
            m_hammerButton.onClick.AddListener(BuyOrEquipHammer);

            m_hoeButton.onClick.RemoveAllListeners();
            m_hoeButton.onClick.AddListener(EquipHoe);

            m_secondMapButton.onClick.RemoveAllListeners();
            m_secondMapButton.onClick.AddListener(BuyOrEquipSecondMap);

            m_balance.OnBalanceChanged += UpdateBalance;

            UpdateBalance(m_balance.CurrentBalance);
            UpdateButtons();

            if (m_shopMusic != null && !m_shopMusic.isPlaying)
                m_shopMusic.Play();
        }

        public void Exit()
        {
            m_balance.OnBalanceChanged -= UpdateBalance;

            m_shopCamera.enabled = false;

            int selectedMap = PlayerPrefs.GetInt(SelectedMapKey, 0);

            if (selectedMap == 1)
                m_secondArea.AreaCamera.enabled = true;
            else
                m_firstArea.AreaCamera.enabled = true;

            m_shopHud.SetActive(false);

            if (m_shopMusic != null)
                m_shopMusic.Stop();
        }

        private void BuyOrEquipPickaxe()
        {
            bool isOwned = PlayerPrefs.GetInt(PickaxeOwnedKey, 0) == 1;

            if (!isOwned)
            {
                if (!m_balance.Spend(PickaxePrice))
                {
                    Debug.Log("Недостаточно золота для покупки кирки!");
                    return;
                }

                PlayerPrefs.SetInt(PickaxeOwnedKey, 1);
                PlayerPrefs.Save();
            }

            SelectModel(1);
        }

        private void BuyOrEquipHammer()
        {
            bool isOwned = PlayerPrefs.GetInt(HammerOwnedKey, 0) == 1;

            if (!isOwned)
            {
                if (!m_balance.Spend(HammerPrice))
                {
                    Debug.Log("Недостаточно золота для покупки молотка!");
                    return;
                }

                PlayerPrefs.SetInt(HammerOwnedKey, 1);
                PlayerPrefs.Save();
            }

            SelectModel(2);
        }

        private void EquipHoe()
        {
            SelectModel(0);
        }

        private void SelectModel(int modelIndex)
        {
            PlayerPrefs.SetInt(SelectedModelKey, modelIndex);
            PlayerPrefs.Save();

            if (modelIndex == 0)
                m_stick.SelectDefaultModel();
            else if (modelIndex == 1)
                m_stick.SelectPickaxeModel();
            else if (modelIndex == 2)
                m_stick.SelectHammerModel();

            UpdateButtons();
        }

        private void BuyOrEquipSecondMap()
        {
            bool isOwned = PlayerPrefs.GetInt(
                SecondMapOwnedKey, 0
            ) == 1;

            int selectedMap = PlayerPrefs.GetInt(
                SelectedMapKey, 0
            );

            if (!isOwned)
            {
                if (!m_balance.Spend(SecondMapPrice))
                {
                    Debug.Log("Недостаточно золота для покупки второй карты!");
                    return;
                }

                PlayerPrefs.SetInt(SecondMapOwnedKey, 1);
                PlayerPrefs.Save();

                UpdateButtons();
                return;
            }

            if (selectedMap == 1)
                SelectMap(0);
            else
                SelectMap(1);
        }

        private void SelectMap(int mapIndex)
        {
            if (mapIndex == 1 &&
                PlayerPrefs.GetInt(SecondMapOwnedKey, 0) != 1)
                return;

            PlayerPrefs.SetInt(SelectedMapKey, mapIndex);
            PlayerPrefs.Save();

            m_selectMap?.Invoke(mapIndex);

            UpdateButtons();
        }

        private void UpdateBalance(int balance)
        {
            m_balanceText.text = "Gold: " + balance;
        }

        private void UpdateButtons()
        {
            bool pickaxeOwned = PlayerPrefs.GetInt(
                PickaxeOwnedKey, 0
            ) == 1;

            bool hammerOwned = PlayerPrefs.GetInt(
                HammerOwnedKey, 0
            ) == 1;

            int selectedModel = PlayerPrefs.GetInt(
                SelectedModelKey, 0
            );

            if (!pickaxeOwned)
                m_pickaxeButtonText.text = "50 GOLD";
            else if (selectedModel == 1)
                m_pickaxeButtonText.text = "EQUIPPED";
            else
                m_pickaxeButtonText.text = "EQUIP";

            if (!hammerOwned)
                m_hammerButtonText.text = "100 GOLD";
            else if (selectedModel == 2)
                m_hammerButtonText.text = "EQUIPPED";
            else
                m_hammerButtonText.text = "EQUIP";

            m_hoeButtonText.text =
                selectedModel == 0 ? "EQUIPPED" : "EQUIP";

            bool secondMapOwned = PlayerPrefs.GetInt(
                SecondMapOwnedKey, 0
            ) == 1;

            int selectedMap = PlayerPrefs.GetInt(
                SelectedMapKey, 0
            );

            if (!secondMapOwned)
                m_secondMapButtonText.text = SecondMapPrice + "1 GOLD";
            else if (selectedMap == 1)
                m_secondMapButtonText.text = "UNEQUIP";
            else
                m_secondMapButtonText.text = "EQUIP";
        }
    }
}
