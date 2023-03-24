using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

namespace MonkeTV.Behaviours
{
    public class TVClass : MonoBehaviour
    {
        public bool Loaded { get; private protected set; }

        // Videos
        public string fLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public List<string> vidNames = new List<string>();
        public bool isPaused;
        internal int currentlySelected;

        // Objects
        public GameObject tStand;
        public GameObject tObject;
        public VideoPlayer tPlayer;
        internal GameObject bForward;
        internal GameObject bBackward;
        internal GameObject bPause;
        internal GameObject mSlider;
        internal GameObject vSlider;
        internal Font utopiumFont;
        internal Text curText;
        internal Text tvInfo;

        // Notification
        public IEnumerator iEnum;
        public float ReportedTime = Time.time;

        internal void Start()
        {
            if (Loaded) return;
            Loaded = true;

            // TV initialization
            CreateTV();

            // Button initialization
            bForward = CreateButton("forward", new Vector3(-63.14552f, 12.317f, -82.17768f), Quaternion.Euler(0f, -127.393f, 0), new Vector3(0.07108399f, 0.07108399f, 0.07108399f));
            bBackward = CreateButton("backward", new Vector3(-63.33839f, 12.317f, -81.92535f), Quaternion.Euler(0f, -127.393f, 0), new Vector3(0.07108399f, 0.07108399f, 0.07108399f));
            bPause = CreateButton("pause", new Vector3(-63.24147f, 12.317f, -82.05215f), Quaternion.Euler(0f, -127.393f, 0), new Vector3(0.1651334f, 0.07108399f, 0.03031021f));

            // Slider initialization
            mSlider = CreateSlider("volume", new Vector3(-63.054f, 12.68741f, -82.297f), Quaternion.Euler(0f, -127.393f, 0), new Vector3(0.04f, 0.04f, 0.04f));
            vSlider = CreateSlider("timer", new Vector3(-63.004f, 12.68741f, -82.347f), Quaternion.Euler(0f, -127.393f, 0), new Vector3(0.04f, 0.04f, 0.04f));

            // Text initialization
            curText = CreateText(bPause.transform, new Vector3(0f, 0f, 0.66f), Quaternion.identity, new Vector3(-0.019f, 0.04522476f, 0.04522476f));
            curText.text = "PLAYING";
            tvInfo = CreateText(tPlayer.transform, new Vector3(-0.118f, -0.4f, -0.02f), Quaternion.Euler(0f, -180f, 0f), new Vector3(-0.006959652f, 0.009958658f, 0.004246371f));
            tvInfo.alignment = TextAnchor.MiddleLeft;
            tvInfo.text = "hehehe yup";
            tvInfo.font = GorillaTagger.Instance.offlineVRRig.playerText.font;
            tvInfo.color = Color.clear;
            Text lArrowText = CreateText(bBackward.transform, new Vector3(0f, 0f, 0.55f), Quaternion.identity, new Vector3(-0.04413842f, 0.04522476f, 0.01928384f));
            lArrowText.text = "<";
            Text rArrowText = CreateText(bForward.transform, new Vector3(0f, 0f, 0.55f), Quaternion.identity, new Vector3(-0.04413842f, 0.04522476f, 0.01928384f));
            rArrowText.text = ">";

            // Video initialization
            VideoLoad();
            VideoPlay();
        }

        public void ShowMessageMethod(string msg)
        {
            if (iEnum != null) StopCoroutine(iEnum);

            ReportedTime = Time.time;
            iEnum = ShowMessage(msg);
            StartCoroutine(iEnum);
        }

        private IEnumerator ShowMessage(string msg)
        {
            float rTime = ReportedTime;
            float val = 0;
            for(int i = 0; i < 5; i++)
            {
                tvInfo.text = msg;
                tvInfo.color = new Color(1, 1, 1, val);
                val += 0.1f * 2;
                yield return new WaitForSeconds(0.025f);
            }
            
            if (ReportedTime != rTime) yield break;

            tvInfo.color = Color.white;
            yield return new WaitForSeconds(1f);
            for (int i = 0; i < 10; i++)
            {
                tvInfo.color = new Color(1, 1, 1, val);
                val -= 0.1f;
                yield return new WaitForSeconds(0.025f);
            }

            if (ReportedTime != rTime) yield break;
            tvInfo.color = Color.clear;

            yield break;
        }

        private void CreateTV()
        {
            foreach(var tO in Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name.StartsWith("TV_Prefab ")))
            {
                if (tO.name.EndsWith(" (2)"))
                {
                    Console.WriteLine("Found TV model");
                    tObject = Instantiate(tO, null);
                    tObject.transform.position = new Vector3(-63.181f, 12.41f, -82.016f);
                    tObject.transform.rotation = Quaternion.Euler(0f, 232.607f, 0f);
                    tObject.transform.localScale = Vector3.one;
                    tObject.TryGetComponent(out Renderer tRenderer);
                    tObject.TryGetComponent(out Collider tCollider);
                    tRenderer.enabled = true;

                    Material tMat = new Material(Shader.Find("Gorilla/GorillaUnlit"));
                    tMat.mainTexture = tRenderer.material.mainTexture;
                    tRenderer.material = tMat;

                    Destroy(tCollider);
                    break;
                }
            }

            foreach (var sO in Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name.StartsWith("Stool_Prefab ")))
            {
                if (sO.name.EndsWith(" (5)"))
                {
                    Console.WriteLine("Found TV stand model");
                    tStand = Instantiate(sO, null);
                    tStand.transform.position = new Vector3(-63.11097f, 11.80626f, -81.9583f);
                    tStand.transform.rotation = Quaternion.Euler(0f, 145.174f, 0f);
                    tStand.transform.localScale = Vector3.one;
                    tStand.TryGetComponent(out Renderer sRenderer);
                    tStand.TryGetComponent(out Collider sCollider);
                    sRenderer.enabled = true;

                    Material tMat = new Material(Shader.Find("Gorilla/GorillaUnlit"));
                    tMat.mainTexture = sRenderer.material.mainTexture;
                    sRenderer.material = tMat;

                    Destroy(sCollider);
                    break;
                }
            }

            foreach (var fC in Resources.FindObjectsOfTypeAll<Text>())
            {
                if (fC.font.name is "Utopium")
                {
                    utopiumFont = fC.font;
                    break;
                }
            }

            // Listening to https://www.youtube.com/watch?v=D8RX__HjPzs on repeat
            GameObject tVideoObj = GameObject.CreatePrimitive(PrimitiveType.Quad);
            tVideoObj.transform.position = new Vector3(-63.24012f, 12.526f, -82.04734f);
            tVideoObj.transform.rotation = Quaternion.Euler(0f, 52.607f, 0);
            tVideoObj.transform.localScale = new Vector3(0.4508178f, 0.3228103f, 0.3228102f);
            tVideoObj.TryGetComponent(out Collider tvoCollider);
            tPlayer = tVideoObj.AddComponent<VideoPlayer>();
            tPlayer.isLooping = true;

            Destroy(tvoCollider);
            Console.WriteLine("Created TV object");
        }

        private GameObject CreateButton(string bName, Vector3 bPosition, Quaternion bRotation, Vector3 bScale)
        {
            GameObject bObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bObject.name = bName;
            bObject.layer = 18;

            bObject.transform.position = bPosition;
            bObject.transform.rotation = bRotation;
            bObject.transform.localScale = bScale;

            bObject.GetComponent<BoxCollider>().isTrigger = true;
            bObject.AddComponent<Button>();

            return bObject;
        }

        private GameObject CreateSlider(string sName, Vector3 sPosition, Quaternion sRotation, Vector3 sScale)
        {
            GameObject sObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            sObject.name = sName;
            sObject.layer = 18;

            sObject.transform.position = sPosition;
            sObject.transform.rotation = sRotation;
            sObject.transform.localScale = sScale;

            sObject.GetComponent<BoxCollider>().isTrigger = true;
            sObject.AddComponent<Slider>().mPlier = 2.58123f;
            if (sName is "volume") sObject.AddComponent<VolumeSlider>();
            else if (sName is "timer") sObject.AddComponent<TimeSlider>();

            return sObject;
        }

        private Text CreateText(Transform parent, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            GameObject cObject = new GameObject();
            cObject.transform.SetParent(parent, false);
            cObject.transform.localPosition = position;
            cObject.transform.localRotation = rotation;
            cObject.transform.localScale = scale;
            cObject.AddComponent<Canvas>();
            Text cT = cObject.AddComponent<Text>();
            cT.font = utopiumFont;
            cT.alignment = TextAnchor.MiddleCenter;
            cT.alignByGeometry = true;
            cT.color = Color.black;

            return cT;
        }

        public void VideoLoad()
        {
            // Didn't have to change much here
            DirectoryInfo directory = new DirectoryInfo(fLocation + "\\Videos");
            foreach (var file in directory.GetFiles("*.mp4")) vidNames.Add(file.Name);

            // Nevermind
            var tSource = tPlayer.gameObject.AddComponent<AudioSource>();
            tSource.spatialBlend = 1;
            tSource.minDistance = 2f;
            tSource.maxDistance = 8f;
            tSource.rolloffMode = AudioRolloffMode.Linear;
            tPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
            tPlayer.SetTargetAudioSource(0, tSource);
        }

        public void VideoPlay()
        {
            SetURL();
            tPlayer.Play();
            curText.text = tPlayer.isPaused ? "PAUSED" : "PLAYING";
            StartCoroutine(ShowMessage($"PLAYING {vidNames[currentlySelected].ToString().ToUpper()}"));
        }

        internal void SetURL()
        {
            // Patch currentlySelected variable if the video doesn't exists
            if (currentlySelected < 0) currentlySelected = vidNames.Count - 1;
            else if (currentlySelected > vidNames.Count - 1) currentlySelected = 0;

            // Okay now set the damn url
            tPlayer.url = fLocation + "\\Videos\\" + vidNames[currentlySelected];
        }

        public void VideoPlayForward()
        {
            currentlySelected++;
            VideoPlay();
        }

        public void VideoPlayBackward()
        {
            currentlySelected--;
            VideoPlay();
        }

        public void VideoPause()
        {
            if (isPaused) tPlayer.Play(); else tPlayer.Pause();
            isPaused = tPlayer.isPaused;

            curText.text = isPaused ? "PAUSED" : "PLAYING";
            StartCoroutine(ShowMessage(isPaused ? "PAUSED" : "RESUMED"));
        }
    }
}
