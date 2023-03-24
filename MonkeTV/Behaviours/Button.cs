using UnityEngine;

namespace MonkeTV.Behaviours
{
    public class Button : GorillaPressableButton
    {
        internal GorillaPressableButton baseButton;
        internal Renderer baseRenderer;

        internal Material mUnpressed;
        internal Material mPressed;

        // Not sure why the Start method is in GPB even know it does nothing
        internal new void Start()
        {
            // Regarding "ModeSelectButton" it was just something random
            foreach(var mButton in Resources.FindObjectsOfTypeAll<ModeSelectButton>())
            {
                if (mButton.gameObject.name is "Casual")
                {
                    baseButton = mButton;
                    break;
                }
            }

            mUnpressed = baseButton?.unpressedMaterial;
            mPressed = baseButton?.pressedMaterial;

            TryGetComponent(out baseRenderer);
            baseRenderer.material = mUnpressed;
        }

        internal void LateUpdate()
        {
            if (!(baseRenderer is null)) baseRenderer.material = name is "pause" ? (Plugin.Instance.tClass.tPlayer.isPaused ? mPressed : mUnpressed) : (Time.time >= (debounceTime + touchTime) ? mUnpressed : mPressed);
        }

        public override void ButtonActivation()
        {
            base.ButtonActivation();

            switch (name)
            {
                default:
                    Plugin.Instance.tClass.VideoPause();
                    break;
                case "forward":
                    Plugin.Instance.tClass.VideoPlayForward();
                    break;
                case "backward":
                    Plugin.Instance.tClass.VideoPlayBackward();
                    break;
            }
        }
    }
}
