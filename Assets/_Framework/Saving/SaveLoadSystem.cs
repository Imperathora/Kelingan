using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Framework.Saving
{
    public class SaveLoadSystem<T> where T : GameSessionData, new()
    {
        public delegate void SaveLoadDelegate(ref T _data);
        public SaveLoadDelegate OnSaveStarted;
        public SaveLoadDelegate OnLoadStarted;

        public event Action OnSaveDeleted;

        public void Save()
        {
            T data = new T();

            if (OnSaveStarted != null)
            {
                OnSaveStarted.Invoke(ref data);
            }

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/SaveGame.dat");

            bf.Serialize(file, data);
            file.Close();

            Debug.Log("Game saved.");
        }

        public void Load()
        {
            if (File.Exists(Application.persistentDataPath + "/SaveGame.dat"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/SaveGame.dat", FileMode.Open);
                T data = (T)bf.Deserialize(file);
                file.Close();

                if (OnLoadStarted != null)
                {
                    OnLoadStarted.Invoke(ref data);
                }

                Debug.Log("Game loaded.");
            }
        }

        public void DeleteSave()
        {
            if (File.Exists(Application.persistentDataPath + "/SaveGame.dat"))
            {
                File.Delete(Application.persistentDataPath + "/SaveGame.dat");

                if (OnSaveDeleted != null)
                {
                    OnSaveDeleted.Invoke();
                }

                Debug.Log("Savegame deleted.");
            }
        }
    }

    [Serializable]
    public class GameSessionData { }
}