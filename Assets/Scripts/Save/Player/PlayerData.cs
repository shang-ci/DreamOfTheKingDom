using System.Collections.Generic;
using UnityEngine;

namespace SaveSystem
{
    public class PlayerData : MonoBehaviour
    {
        [Header("´æ´¢Êý¾Ý")]
        [SerializeField] private CardLibrarySO library;
        [SerializeField] private List<Equipment_ItemData> equipmentItemsData = new List<Equipment_ItemData>();
        [SerializeField] private MapLayoutSO mapLayout;

        const string DataFileName = "PlayerData.ZXH";

        [SerializeField]
        class Data
        {
            public List<Equipment_ItemData> equipmentItemsData = new List<Equipment_ItemData>();
            public MapLayoutSO mapLayout;
            public CardLibrarySO library;
        }


        #region save and load
        public void Save()
        {
            library = CardManager.instance.currentLibrary;
            equipmentItemsData = EquipManager.instance.equipmentItemsData;
            mapLayout = GameManager.instance.mapLayout;

            SaveByJson();
        }

        public void Load()
        {
            LoadByJson();
        }

        #endregion


        #region json

        public void SaveByJson()
        {
            SaveManager.Savebyjson(DataFileName, SavingData());
        }

        public void LoadByJson()
        {
            var data = SaveManager.Loadbyjson<Data>(DataFileName); 
            LoadData(data);
        }

        #endregion


        #region Help Functions
        private Data SavingData()
        {
            return new Data
            {
                equipmentItemsData = equipmentItemsData,
                mapLayout = mapLayout,
                library = library
            };
        }

        private void LoadData(Data data)
        {
            equipmentItemsData = data.equipmentItemsData;
            mapLayout = data.mapLayout;
            library = data.library;
        }

        public static void DeleteDataSaveFile()
        {
            SaveManager.DeleteSaveFile(DataFileName);
        }
        #endregion
    }
}
