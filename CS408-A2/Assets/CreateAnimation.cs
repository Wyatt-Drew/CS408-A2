using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateAnimation : MonoBehaviour
{
    public void Start()
    {
        Animation animation = GetComponent<Animation>();
        AnimationCurve curve;

        // create a new AnimationClip
        AnimationClip clip = new AnimationClip();
        clip.legacy = true;

        // create a curve to move the GameObject and assign to the clip
        Keyframe[] keyframe;
        keyframe = new Keyframe[3];
        keyframe[0] = new Keyframe(0.0f, 0.0f);
        keyframe[1] = new Keyframe(1.0f, 1.5f);
        keyframe[2] = new Keyframe(2.0f, 0.0f);
        curve = new AnimationCurve(keyframe);
        clip.SetCurve("", typeof(Transform), "localPosition.x", curve);

        // update the clip to a change the red color
        curve = AnimationCurve.Linear(0.0f, 1.0f, 2.0f, 0.0f);
        clip.SetCurve("", typeof(Material), "_Color.r", curve);

        // now animate the GameObject
        animation.AddClip(clip, clip.name);
        animation.Play(clip.name);
    }

}

