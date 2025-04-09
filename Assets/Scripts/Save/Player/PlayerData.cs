using System.Collections.Generic;
using UnityEngine;

namespace SaveSystem
{
    public class PlayerData : MonoBehaviour
    {
        [Header("存储数据")]
        [SerializeField] private CardLibrarySO _library;
        [SerializeField] private List<Equipment_ItemData> _equipmentItemsData = new List<Equipment_ItemData>();
        [SerializeField] private MapLayoutSO _mapLayout;

        [SerializeField] private SceneLoadManager SceneLoadManager;

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
            _library = CardManager.instance.currentLibrary;
            _equipmentItemsData = EquipManager.instance.equipmentItemsData;
            _mapLayout = GameManager.instance.mapLayout;

            Debug.Log($"保存：{_library.entryList[0].amount}");

            SaveByJson();
        }

        public void Load()
        {
            LoadByJson();
            SceneLoadManager.LoadMap();//用事件没法保证地图数据在加载地图场景前完成
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
            Debug.Log($"保存：{data.library.entryList[0].amount}");

            LoadData(data);
        }

        #endregion


        #region Help Functions
        private Data SavingData()
        {
            return new Data
            {
                equipmentItemsData = _equipmentItemsData,
                mapLayout = _mapLayout,
                library = _library
            };
        }

        private void LoadData(Data data)
        {
            _equipmentItemsData = data.equipmentItemsData;
            _mapLayout = data.mapLayout;//因为没人会动，所以直接赋值就行了
            _library = data.library; //library会被cardmanager重置，而且是在加载数据之前
            Debug.Log($"保存：{data.library.entryList[0].amount}");
        }

        public static void DeleteDataSaveFile()
        {
            SaveManager.DeleteSaveFile(DataFileName);
        }
        #endregion
    }
}
