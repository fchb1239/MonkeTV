using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace MonkeTV.Behaviours
{
    public class VolumeSlider : MonoBehaviour
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

        internal void Update()
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
                        if (cDrag is false)
                        {
                            Plugin.Instance.tClass.tPlayer.GetTargetAudioSource(0).volume = GetComponent<Slider>().fValue;
                            Plugin.Instance.tClass.ShowMessageMethod(string.Concat("VOLUME: ", Mathf.RoundToInt(GetComponent<Slider>().fValue * 100), "%"));
                        }
                    }
                }
            }
        }
    }
}
