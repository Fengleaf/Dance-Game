using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public BVHAnimationLoader bvhAnimationLoader;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        bvhAnimationLoader.filename = Path.Combine(Application.streamingAssetsPath, "BVHs") + "/" + LevelData.levelName + ".bvh";
        bvhAnimationLoader.parseFile();
        bvhAnimationLoader.loadAnimation();
    }

    public void OnModelComplete()
    {
        bvhAnimationLoader.playAnimation();
    }
}
