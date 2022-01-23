using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using SFB;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public Text scoreText;
    public Text gameOverText;

    public BVHAnimationLoader bvhAnimationLoader;

    public MotionModel player;
    public MotionModel npc;

    public Transform mainTexture;

    private float totalScore = 0;

    private VNectModel playerModel;
    private BVHRecorder recorder;

    public Transform uiCanvas;
    public GameObject warningObject;
    public Text warningText;
    public GameObject boundingBox;
    public Countdown countdownPrefab;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerModel = player.GetComponent<VNectModel>();
        recorder = player.GetComponent<BVHRecorder>();
        bvhAnimationLoader.filename = Path.Combine(Application.streamingAssetsPath, "BVHs") + "/" + LevelData.levelName + ".bvh";
        bvhAnimationLoader.parseFile();
        bvhAnimationLoader.loadAnimation();
        warningObject.SetActive(true);
        warningText.text = "Wait model Load...";
    }

    public void OnModelComplete()
    {
        warningObject.SetActive(false);
        totalScore = 0;
        StartCoroutine(GamePlayCoroutine());
        StartCoroutine(RecordActionCoroutine());
    }

    public void ExportBVH()
    {
        string s = StandaloneFileBrowser.SaveFilePanel("儲存", Application.dataPath, "dance", "bvh");
        if (!string.IsNullOrEmpty(s))
        {
            recorder.overwrite = File.Exists(s);
            List<string> path = new List<string>(s.Split('\\'));
            string fileName = path[path.Count - 1];
            path.RemoveAt(path.Count - 1);
            string directory = string.Join("\\", path);
            recorder.directory = directory;
            recorder.filename = fileName;
            recorder.saveBVH();
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    private IEnumerator RecordActionCoroutine()
    {
        while (true)
        {
            recorder.capturing = !playerModel.IsError;
            yield return null;
        }
    }

    private IEnumerator GamePlayCoroutine()
    {
        yield return new WaitForSeconds(5);
        bvhAnimationLoader.playAnimation();
        mainTexture.localScale = new Vector3(-1, 1, 1);
        yield return new WaitForSeconds(7);
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (!bvhAnimationLoader.anim.isPlaying)
            {
                // 遊戲結束
                warningObject.SetActive(true);
                warningText.text = "~~~Game Over~~~ Congratuation!!!!";
                break;
            }
            npc.SaveRecord();
            float preSpeed = 0;
            foreach (AnimationState state in bvhAnimationLoader.anim)
            {
                preSpeed = state.speed;
                state.speed = 0;
            }
            player.StartRecord();
            while (true)
            {
                if (playerModel.IsErrorAverage)
                {
                    player.SaveRecord();
                    warningObject.SetActive(true);
                    warningText.text = "Please step in the bounding box, thank you!";
                    boundingBox.SetActive(true);
                    yield return new WaitUntil(() => !playerModel.IsErrorAverage);
                    warningObject.SetActive(false);
                    boundingBox.SetActive(false);
                    Countdown countdown = Instantiate(countdownPrefab, uiCanvas);
                    bool playerDisapear = false;
                    yield return null;
                    while (!countdown.IsEnd())
                    {
                        player.StartRecord();
                        if (playerModel.IsErrorAverage)
                        {
                            warningObject.SetActive(true);
                            warningText.text = "Please step in the bounding box, thank you!";
                            boundingBox.SetActive(true);
                            playerDisapear = true;
                            countdown.countdownState = Countdown.state.STOP;
                            break;
                        }
                        yield return null;
                    }
                    if (playerDisapear)
                        continue;
                    else
                        countdown.countdownState = Countdown.state.STOP;
                }
                float score = GetTotalScore();
                if (score >= 1)
                {
                    totalScore += score;
                    scoreText.text = totalScore.ToString();
                    foreach (AnimationState state in bvhAnimationLoader.anim)
                        state.speed = preSpeed;
                    npc.StartRecord();
                    break;
                }
                yield return null;
            }
        }
    }

    private float GetTotalScore()
    {
        if (npc.boneFrames.Count == 0)
            return 1;

        float total = 0;
        //bool ok = false;
        int index = 0;
        float min = Mathf.Infinity;
        for (int i = 0; i < player.boneFrames.Count; i++)
        {
            total = 0;
            // 找最接近
            foreach (KeyValuePair<int, Transform> pair in npc.boneFrames[0])
            {
                Transform npcTransform = pair.Value;
                Transform playerTransform = player.boneFrames[i][pair.Key];
                float distance = ComputeRotationDistance(playerTransform, npcTransform);
                total += distance;
            }
            if (total < min)
            {
                min = total;
                index = i;
            }
        }

        total = 0;
        for (int j = 0; j < npc.boneFrames.Count; j++)
        {
            if (j + index >= player.boneFrames.Count)
                return 0;
            float total2 = 0;
            foreach (KeyValuePair<int, Transform> pair in npc.boneFrames[j])
            {
                Transform npcTransform = pair.Value;
                Transform playerTransform = player.boneFrames[j + index][pair.Key];
                float distance = ComputeRotationDistance(playerTransform, npcTransform);
                float score = 0;
                if (distance < 40)
                    score = 1;
                else if (distance < 20)
                    score = 2;
                else if (distance < 15)
                    score = 3;
                else if (distance < 10)
                    score = 4;
                total2 += score;
            }
            Debug.Log(total2);
            if (total2 <= 1)
                return 0;
            total += total2;
        }

        //foreach (KeyValuePair<int, Transform> pair in npc.BoneTransforms)
        //{
        //    Transform npcTransform = pair.Value;
        //    Transform playerTransform = player.BoneTransforms[pair.Key];
        //    float distance = ComputeRotationDistance(playerTransform, npcTransform);
        //    float score = 0;
        //    if (distance < 20)
        //        score = 1;
        //    else if (distance < 10)
        //        score = 2;
        //    else if (distance < 5)
        //        score = 3;
        //    else if (distance < 1)
        //        score = 4;
        //    total += score;
        //}
        Debug.Log("total: " + total);
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
