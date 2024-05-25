using System;
using System.Collections.Generic;
using System.IO;
using Cells;
using UnityEngine;

public class SaveManager
{
	private string _saveFilePath;

	public void Initialize()
	{
		_saveFilePath = Path.Combine(Application.persistentDataPath, "Save.json");
	}

	public void Save(SelectedGameSettings selectedGameSettings, List<SerializableCell> cells)
	{
		SaveData data = new SaveData(selectedGameSettings, cells);
		string json = JsonUtility.ToJson(data);
		File.WriteAllText(_saveFilePath, json);
		Debug.Log("Data saved to " + _saveFilePath);
	}

	public SaveData Load()
	{
		if (File.Exists(_saveFilePath))
		{
			string json = File.ReadAllText(_saveFilePath);
			SaveData data = JsonUtility.FromJson<SaveData>(json);
			Debug.Log("Data loaded: " + json);
			return data;
		}
		else
		{
			Debug.LogError("Save file not found at " + _saveFilePath);
		}

		return null;
	}

	public void Delete()
	{
		if (File.Exists(_saveFilePath))
		{
			File.Delete(_saveFilePath);
			Debug.Log("Save file deleted from " + _saveFilePath);
		}
		else
		{
			Debug.LogError("No save file to delete at " + _saveFilePath);
		}
	}


	[Serializable]
	public class SaveData
	{
		public SelectedGameSettings SelectedGameSettings;
		public List<SerializableCell> Cells;

		public SaveData(SelectedGameSettings selectedGameSettings, List<SerializableCell> cells)
		{
			SelectedGameSettings = selectedGameSettings;
			Cells = cells;
		}
	}
}