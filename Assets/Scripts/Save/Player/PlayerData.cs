using System.Collections.Generic;
using UnityEngine;

namespace SaveSystem
{
    public class PlayerData : MonoBehaviour
    {
        [Header("�洢����")]
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

            Debug.Log($"���棺{_library.entryList[0].amount}");

            SaveByJson();
        }

        public void Load()
        {
            LoadByJson();
            SceneLoadManager.LoadMap();//���¼�û����֤��ͼ�����ڼ��ص�ͼ����ǰ���
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
            Debug.Log($"���棺{data.library.entryList[0].amount}");

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
            _mapLayout = data.mapLayout;//��Ϊû�˻ᶯ������ֱ�Ӹ�ֵ������
            _library = data.library; //library�ᱻcardmanager���ã��������ڼ�������֮ǰ
            Debug.Log($"���棺{data.library.entryList[0].amount}");
        }

        public static void DeleteDataSaveFile()
        {
            SaveManager.DeleteSaveFile(DataFileName);
        }
        #endregion
    }
}
