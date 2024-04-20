using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public interface IDataPersistence
{
    void LoadGame(GameData data);
    void SaveGame(ref GameData data);
}
