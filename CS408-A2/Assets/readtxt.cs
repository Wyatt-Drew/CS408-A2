using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using System.Linq;

public class readtxt : MonoBehaviour
{
    public GameObject newObject;
    void Start()
    {
        ReadString();
    }
    public static void ReadString()
    {
        //*********************************************************** reading data *************************************
        Object[] objects = new Object[100];
        keyFrame[] keys = { };
        keyFrame keyFrame1 = new keyFrame();
        keyFrame[] emptyKey = { keyFrame1 };
        string[] Data = {};
        string[] Splitted = {};
        string path = "Assets/textFiles/animation.txt";
        StreamReader sr = new StreamReader(path);
        //read all data into Data array
        for (int i = 0; !sr.EndOfStream; i++)
        {
            string line = sr.ReadLine();
            Splitted = line.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
            Data = Data.Concat(Splitted).ToArray();
        }
        sr.Close();
        int obCount = 0;
        int keyCount = 0;
        //parsing Data array into object and keyframe arrays
        for (int i = 0; i < Data.Length; i++)
        {
            //Collects all objects in object array
            if (Data[i] == "OBJECT")
            {
                obCount++;
                if (int.TryParse(Data[i + 1], out var number))
                {
                    objects[number].key = emptyKey;//initialize to empty array
                    objects[number].fileName = Data[i + 2];
                    objects[number].keyCount = 0;
                    i = i + 1;
                }
                else
                {
                    ///error
                }

            }
            //Collects all keyframes in keyframe array
            //Done this way to allow keyframes to come before objects
            if (Data[i] == "KEYFRAME")
            {

                int.TryParse(Data[i + 1], out var objectNum);
                int.TryParse(Data[i + 2], out var frameNum);
                float.TryParse(Data[i + 3], out var px); //p = position
                float.TryParse(Data[i + 4], out var py);
                float.TryParse(Data[i + 5], out var pz);
                float.TryParse(Data[i + 6], out var rx); // r = rotation
                float.TryParse(Data[i + 7], out var ry);
                float.TryParse(Data[i + 8], out var rz);
                float.TryParse(Data[i + 9], out var sx); // s = scale
                float.TryParse(Data[i + 10], out var sy);
                float.TryParse(Data[i + 11], out var sz);

                keys = keys.Concat(emptyKey).ToArray(); //Increase array size by 1

                keys[keyCount].objectNum = objectNum;
                keys[keyCount].frameNum = frameNum;
                keys[keyCount].vector = new Vector3(px, py, pz);
                keys[keyCount].rotation = new Vector3(rx, ry, rz);
                keys[keyCount].scale = new Vector3(sx, sy, sz);
                keyCount++;
                i = i + 10;
            }

        }
        //for every keyframe find its object and put it in it.  If it does not follow the correct order
        //then report an error
        int indexO;
        int indexK;
            for (int i = 0; i < keys.Length; i++)
        {
            indexK = objects[keys[i].objectNum].keyCount;
            indexO = keys[i].objectNum;
            objects[indexO].key = objects[indexO].key.Concat(emptyKey).ToArray(); //Increase array size by 1
            objects[indexO].key[indexK] = keys[i];
            objects[indexO].keyCount = objects[indexO].keyCount + 1;
        }
        //********************************************* creating animations for each object **********
        keyCount = 0;
        for (int i = 0; i < obCount; i++)
        {
            GameObject instance = Instantiate(Resources.Load(objects[i].fileName, typeof(GameObject))) as GameObject;
            Animation animation = instance.AddComponent<Animation>() as Animation;
            // create a new AnimationClip
            AnimationClip clip = new AnimationClip();
            clip.legacy = true;

            // create a curve to move the GameObject and assign to the clip
            keyCount = objects[i].keyCount;
            Keyframe[] keyframeX = new Keyframe[keyCount];
            Keyframe[] keyframeY = new Keyframe[keyCount];
            Keyframe[] keyframeZ = new Keyframe[keyCount];
            //rotation
            Keyframe[] keyframeXR = new Keyframe[keyCount];
            Keyframe[] keyframeYR = new Keyframe[keyCount];
            Keyframe[] keyframeZR = new Keyframe[keyCount];
            //scale
            Keyframe[] keyframeXS = new Keyframe[keyCount];
            Keyframe[] keyframeYS = new Keyframe[keyCount];
            Keyframe[] keyframeZS = new Keyframe[keyCount];
            for (int j = 0; j < keyCount; j++)
            {
                //keyframe constructor parameters = keyframe(time(s),the thing you name)
                //60 frames per second.  The first field is in seconds.  Therefore divide by 60.
                //XYZ
                keyframeX[j] = new Keyframe(objects[i].key[j].frameNum / 60f, objects[i].key[j].vector.x);
                keyframeY[j] = new Keyframe(objects[i].key[j].frameNum / 60f, objects[i].key[j].vector.y);
                keyframeZ[j] = new Keyframe(objects[i].key[j].frameNum / 60f, objects[i].key[j].vector.z);
                //XYZ rotation
                keyframeXR[j] = new Keyframe(objects[i].key[j].frameNum / 60f, objects[i].key[j].rotation.x);
                keyframeYR[j] = new Keyframe(objects[i].key[j].frameNum / 60f, objects[i].key[j].rotation.y);
                keyframeZR[j] = new Keyframe(objects[i].key[j].frameNum / 60f, objects[i].key[j].rotation.z);
                //XYZ scale
                keyframeXS[j] = new Keyframe(objects[i].key[j].frameNum / 60f, objects[i].key[j].scale.x);
                keyframeYS[j] = new Keyframe(objects[i].key[j].frameNum / 60f, objects[i].key[j].scale.y);
                keyframeZS[j] = new Keyframe(objects[i].key[j].frameNum / 60f, objects[i].key[j].scale.z);
            }
            //AnimationCurve curveZS = new AnimationCurve(keyframeZS);
            //xyz
            clip.SetCurve("", typeof(Transform), "localPosition.x", new AnimationCurve(keyframeX));
            clip.SetCurve("", typeof(Transform), "localPosition.y", new AnimationCurve(keyframeY));
            clip.SetCurve("", typeof(Transform), "localPosition.z", new AnimationCurve(keyframeZ));
            //rotation
            clip.SetCurve("", typeof(Transform), "localEulerAngles.x", new AnimationCurve(keyframeXR));
            clip.SetCurve("", typeof(Transform), "localEulerAngles.y", new AnimationCurve(keyframeYR));
            clip.SetCurve("", typeof(Transform), "localEulerAngles.z", new AnimationCurve(keyframeZR));
            //scale
            clip.SetCurve("", typeof(Transform), "localScale.x", new AnimationCurve(keyframeXS));
            clip.SetCurve("", typeof(Transform), "localScale.y", new AnimationCurve(keyframeYS));
            clip.SetCurve("", typeof(Transform), "localScale.z", new AnimationCurve(keyframeZS));


            // update the clip to a change the red color
            //curve = AnimationCurve.Linear(0.0f, 1.0f, 2.0f, 0.0f);
            //clip.SetCurve("", typeof(Material), "_Color.r", curve);
            // now animate the GameObject
            animation.AddClip(clip, clip.name);
            animation.Play(clip.name);

            //GameObject instance = Instantiate(Resources.Load("ladybug", typeof(GameObject))) as GameObject;
            // GameObject obj = Instantiate(newObject, transform.position, transform.rotation) as GameObject;
        }
    }
    //void createObject(Object objects2[0])
    //{
        //public GameObject projectile;
        //      
        //newObject = GameObject.Find("/ladybug");
        //GameObject obj = Instantiate(newObject, transform.position, transform.rotation) as GameObject;
        //Renderer renderer = launched.GetComponent<Renderer>();
    //}
}

    public struct keyFrame
{
    public int objectNum;
    public int frameNum;
    public Vector3 vector;
    public Vector3 rotation;
    public Vector3 scale;
};
public struct Object
{
    public string fileName;
    public keyFrame[] key;
    public int keyCount;
};
