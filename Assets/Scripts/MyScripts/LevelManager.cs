using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public Text scoreText;

    public BVHAnimationLoader bvhAnimationLoader;

    public MotionModel player;
    public MotionModel npc;

    public Transform mainTexture;

    private float totalScore = 0;

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
        totalScore = 0;
        StartCoroutine(GamePlayCoroutine());
    }

    private IEnumerator GamePlayCoroutine()
    {
        yield return new WaitForSeconds(5);
        bvhAnimationLoader.playAnimation();
        mainTexture.localScale = new Vector3(-1, 1, 1);
        while (true)
        {
            yield return new WaitForSeconds(1f);
            Debug.Log("***暫停，等待玩家動作***");
            float preSpeed = 0;
            foreach (AnimationState state in bvhAnimationLoader.anim)
            {
                preSpeed = state.speed;
                state.speed = 0;
            }
            while (true)
            {
                Debug.Log("***等待中***");
                float score = GetTotalScore();
                if (score >= 1)
                {
                    Debug.Log("***獲得分數! NPC繼續執行***");
                    totalScore += score;
                    scoreText.text = totalScore.ToString();
                    foreach (AnimationState state in bvhAnimationLoader.anim)
                        state.speed = preSpeed;
                    break;
                }
                yield return null;
            }

        }
    }

    private float GetTotalScore()
    {
        float total = 0;
        foreach (KeyValuePair<int, Transform> pair in npc.BoneTransforms)
        {
            Transform npcTransform = pair.Value;
            Transform playerTransform = player.BoneTransforms[pair.Key];
            float distance = ComputeRotationDistance(playerTransform, npcTransform);
            float score = 0;
            if (distance < 20)
                score = 1;
            else if (distance < 10)
                score = 2;
            else if (distance < 5)
                score = 3;
            else if (distance < 1)
                score = 4;
            total += score;
        }
        return total;
    }

    private float ComputeRotationDistance(Transform from, Transform to)
    {
        Vector3 fromVector = from.rotation.eulerAngles;
        Vector3 toVector = to.rotation.eulerAngles;
        float distance = Vector3.Distance(fromVector, toVector);
        return distance;
    }
}
