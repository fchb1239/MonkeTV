using System;
using UnityEngine;
using System.Collections;

namespace MonkeTV.Behaviours
{
    public class Button : GorillaPressableButton
    {
        Material pressedMat = Resources.Load<Material>("objects/treeroom/materials/pressed");
        Material unpressedMat = Resources.Load<Material>("objects/treeroom/materials/plastic");

        void Awake()
        {
            transform.GetComponent<Renderer>().material = unpressedMat;
        }

        public override void ButtonActivation()
        {
            base.ButtonActivation();
            Console.WriteLine("Button pressed");
            StartCoroutine(Press());
        }

        private IEnumerator Press()
        {
            Console.WriteLine("Player is pressing: " + transform.name);
            transform.GetComponent<Renderer>().material = pressedMat;
            try
            {
                RunPressMethod();
            }
            catch { }
            yield return new WaitForSeconds((float)0.2);
            transform.GetComponent<Renderer>().material = unpressedMat;
            yield break;
        }

        void RunPressMethod()
        {
            if (transform.name == "forward")
            {
                TVClass.instance.VideoPlayForward();
            }

            if (transform.name == "backward")
            {
                TVClass.instance.VideoPlayBackward();
            }

            if (transform.name == "pause")
            {
                TVClass.instance.VideoPause();
            }
        }
    }
}
