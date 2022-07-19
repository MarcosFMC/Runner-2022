using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class SaveManager : MonoBehaviour
{

    private static SaveManager _instance;
    public static SaveManager Instance { get { return _instance; } }



    //Fields
    public SaveState save;
    private const string saveFileName = "data.ps";
    private BinaryFormatter formatter;

    //Actions

    public Action<SaveState> OnLoad;
    public Action<SaveState> OnSave;
    private void Awake()
    {
        if (_instance == null) _instance = this;
        else Destroy(gameObject);

        formatter = new BinaryFormatter();
        Load();
    }

    public void Load()
    {
        try
        {
            FileStream file = new FileStream(/*Application.persistentDataPath +*/ saveFileName, FileMode.Open, FileAccess.Read);
            save = formatter.Deserialize(file) as SaveState;
            file.Close();
            OnLoad?.Invoke(save);
        }
        catch
        {
            Debug.Log("Save file not found, let's create a new one");
            Save();
        } 
    }

    public void Save()
    {
        //If theres no previous state found, create new one!
        if (save == null)
            save = new SaveState();

        //Set the time at which we've tried saving
        save.LastSaveTime = DateTime.Now;

        //Open a file on our system, and write to it
        FileStream file = new FileStream(/*Application.persistentDataPath +*/ saveFileName, FileMode.OpenOrCreate, FileAccess.Write);
        formatter.Serialize(file, save);
        file.Close();

        OnSave?.Invoke(save);
    }
}
