using System;
using UnityEngine;

namespace Golf.Golf.Source
{
    public class BalanceService
    {
        private const string BalanceKey = "PlayerBalance";

        public int CurrentBalance { get; private set; }

        public event Action<int> OnBalanceChanged;

        public BalanceService()
        {
            CurrentBalance = PlayerPrefs.GetInt(BalanceKey, 0);
        }

        public void Add(int amount)
        {
            if (amount <= 0)
                return;

            CurrentBalance += amount;
            SaveBalance();
        }

        public bool Spend(int amount)
        {
            if (amount <= 0 || CurrentBalance < amount)
                return false;

            CurrentBalance -= amount;
            SaveBalance();

            return true;
        }

        private void SaveBalance()
        {
            PlayerPrefs.SetInt(BalanceKey, CurrentBalance);
            PlayerPrefs.Save();

            OnBalanceChanged?.Invoke(CurrentBalance);
        }
    }
}
