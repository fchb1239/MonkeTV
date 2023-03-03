using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace MonkeTV.Behaviours
{
    public class TimeSlider : MonoBehaviour
    {
        internal GorillaPressableButton baseButton;
        internal Renderer baseRenderer;

        internal bool cDrag;
        internal List<Collider> colliders = new List<Collider>();

        internal void Start()
        {
            // Regarding "ModeSelectButton" it was just something random
            foreach (var mButton in Resources.FindObjectsOfTypeAll<ModeSelectButton>())
            {
                if (mButton.gameObject.name is "Casual")
                {
                    baseButton = mButton;
                    break;
                }
            }

            TryGetComponent(out baseRenderer);
            baseRenderer.material = baseButton.unpressedMaterial;
        }

        internal void OnTriggerEnter(Collider collider)
        {
            if (!(colliders.Count is 0) && colliders.Contains(collider)) return;

            colliders.Add(collider);
            if (colliders.Count is 1)
            {
                cDrag = true;
                GorillaTagger.Instance.StartVibration(collider.GetComponent<GorillaTriggerColliderHandIndicator>().isLeftHand, GorillaTagger.Instance.tapHapticStrength / 3f, GorillaTagger.Instance.tapHapticDuration);
            }
        }

        internal void SetVolume(float volume)
        {
            float fixedLength = (float)Plugin.Instance.tClass.tPlayer.length;
            float time = fixedLength - (volume * fixedLength);
            Plugin.Instance.tClass.tPlayer.time = time;
            float minutes = Mathf.FloorToInt(time / 60);
            float seconds = Mathf.FloorToInt(time % 60);
            Plugin.Instance.tClass.ShowMessageMethod(string.Concat("TIME: ", string.Format("{0:00}:{1:00}", minutes, seconds)));

            float yPos = 12.68741f - GetComponent<Slider>()._Position((float)Plugin.Instance.tClass.tPlayer.time / (float)Plugin.Instance.tClass.tPlayer.length, 12.68741f, 12.30003f);
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, yPos, gameObject.transform.position.z);
        }

        internal void LateUpdate()
        {
            if (colliders.Count > 0)
            {
                float yPos = Mathf.Clamp(colliders[0].transform.position.y, 12.30003f, 12.68741f);
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, yPos, gameObject.transform.position.z);

                // Using OnTriggerExit is the worst thing ever
                foreach (var collider in colliders)
                {
                    float cDist = Vector3.Distance(gameObject.transform.position, collider.transform.position);
                    if (cDist > 0.045f)
                    {
                        int cIndex = colliders.FindIndex(a => a == collider);

                        if (cIndex is 0) GorillaTagger.Instance.StartVibration(collider.GetComponent<GorillaTriggerColliderHandIndicator>().isLeftHand, GorillaTagger.Instance.tapHapticStrength / 3f, GorillaTagger.Instance.tapHapticDuration);
                        colliders.Remove(collider);

                        if (colliders.Count is 0) cDrag = false;
                        if (cDrag is false) SetVolume(GetComponent<Slider>().fValue);
                    }
                }
            }
            else
            {
                if (!cDrag)
                {
                    float yPos = 12.68741f - GetComponent<Slider>()._Position((float)Plugin.Instance.tClass.tPlayer.time / (float)Plugin.Instance.tClass.tPlayer.length, 12.68741f, 12.30003f);
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x, yPos, gameObject.transform.position.z);
                }
            }
        }
    }
}
