using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class BVHManager : MonoBehaviour
{
    public static BVHManager Instance;

    private BVHParser parser;

    private void Awake()
    {
        Instance = this;
    }

    public void Parse(string path)
    {
        using (StreamReader reader = new StreamReader(path))
        {
            string text = reader.ReadToEnd();
            parser = new BVHParser(text);
        }
    }
}
