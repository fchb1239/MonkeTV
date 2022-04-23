using System;
using UnityEngine;
using System.IO;
using System.Reflection;
using System.Collections.Generic;

namespace MonkeTV.Behaviours
{
    public class TVClass : MonoBehaviour
    {
        GameObject TV;
        GameObject buttonForward;
        GameObject buttonBackward;
        GameObject buttonPause;
        GameObject sliderVolume; //This will be used for a volume slider later on
        int currentlySeclected;
        string fileLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        List<string> videos = new List<string>();
        public static TVClass instance;

        void Awake()
        {
            instance = this;

            CreateTV();

            CreateButton(buttonForward, "forward", new Vector3(-63.1f, 11.9f, -82.9f), Quaternion.Euler(0f, 0f, 45f));
            CreateButton(buttonBackward, "backward", new Vector3(-63.1f, 11.9f, -82.5f), Quaternion.Euler(0f, 0f, 45f));
            CreateButton(buttonPause, "pause", new Vector3(-63.1f, 11.9f, -82.7f), Quaternion.Euler(0f, 0f, 45f));

            VideoLoad();
            VideoPlay();
        }

        void CreateTV()
        {
            TV = GameObject.Instantiate(GameObject.CreatePrimitive(PrimitiveType.Plane));
            TV.transform.position = new Vector3(-63f, 12.5f, -82.7f);
            TV.transform.rotation = new Quaternion(0.5f, -0.5f, 0.5f, 0.5f);
            TV.transform.localScale = new Vector3(0.1920f, 0.1080f, 0.1f);
            var videoPlayer = TV.AddComponent<UnityEngine.Video.VideoPlayer>();
            //i wanna listen to https://www.youtube.com/watch?v=D8RX__HjPzs on repeat
            videoPlayer.isLooping = true;
            /*
            var audioSource = TV.AddComponent<AudioSource>();
            audioSource.spatialBlend = 0;
            audioSource.spatialize = false;
            videoPlayer.SetTargetAudioSource(0, audioSource);
            */
            GameObject.Destroy(TV.GetComponent<MeshCollider>());
            Console.WriteLine("Instatiated TV: " + TV.name);
        }

        void CreateButton(GameObject gameObject, string name, Vector3 pos, Quaternion rot)
        {
            gameObject = GameObject.Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube));
            gameObject.transform.position = pos;
            gameObject.transform.rotation = rot;
            gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            gameObject.name = name;
            gameObject.layer = 18;
            gameObject.GetComponent<BoxCollider>().isTrigger = true;
            gameObject.AddComponent<Button>();

            Console.WriteLine("Instatiated button: " + gameObject.name);
        }

        public void VideoPlay()
        {
            var videoPlayer = TV.GetComponent<UnityEngine.Video.VideoPlayer>();
            if (currentlySeclected < 0)
            {
                currentlySeclected = 0;
                videoPlayer.url = fileLocation + "\\Videos\\" + videos[currentlySeclected];
            }
            else if (currentlySeclected > videos.Count)
            {
                currentlySeclected = videos.Count;
                videoPlayer.url = fileLocation + "\\Videos\\" + videos[currentlySeclected];
            }
            else
            {
                videoPlayer.url = fileLocation + "\\Videos\\" + videos[currentlySeclected];
            }
            videoPlayer.Play();
        }

        public void VideoLoad()
        {
            /*
            fileText = File.ReadAllText(fileLocation + "\\videos.txt");
            videos = fileText.Split(',');
            */
            DirectoryInfo directory = new DirectoryInfo(fileLocation + "\\Videos");
            foreach (var file in directory.GetFiles("*.mp4"))
            {
                Console.WriteLine(file.Name);
                videos.Add(file.Name);
            }
            Console.WriteLine("Loaded video files");
        }

        public void VideoPlayForward()
        {
            if(currentlySeclected < videos.Count)
            {
                currentlySeclected++;
                Console.WriteLine("Just went forward: " + currentlySeclected);
            }
            VideoPlay();
        }

        public void VideoPlayBackward()
        {
            if (currentlySeclected > 0)
            {
                currentlySeclected--;
                Console.WriteLine("Just went backward: " + currentlySeclected);
            }
            VideoPlay();
        }

        public void VideoPause()
        {
            var videoPlayer = TV.GetComponent<UnityEngine.Video.VideoPlayer>();
            if (videoPlayer.isPaused)
            {
                videoPlayer.Play();
                Console.WriteLine("Unpaused video");
            }
            else
            {
                videoPlayer.Pause();
                Console.WriteLine("Paused video");
            }
        }
    }
}
