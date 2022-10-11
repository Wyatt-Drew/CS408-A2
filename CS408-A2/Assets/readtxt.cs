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
    public static Object[] objects = new Object[100];
    public static int obCount = 0;
    public static int frameCount = 0;

    void Awake()
    {
        Application.targetFrameRate = 60;
        ReadTextFile();
        CreateAnimations();
    }
    void Start()
    {

    }
    void LateUpdate()
    {
        int curFrame = Time.frameCount - 1;//Unity animation starts on frame 0.  The game has only frame 1.
        for (int i = 0; i < obCount; i++)
        {
            LogIntended(i, curFrame);
            LogReal(i, curFrame);
        }
    }
    public void LogReal(int i, int curFrame) //i is a loop counter for all objects
    {
        //log the objects real values
        GameObject instance = objects[i].instance;
        if (instance.GetComponent<Renderer>().enabled == false)
        {
            //print the object id, the time, and "Object does not exist".
            Debug.Log("Object ID: " + objects[i].objectID +
                        " Time (frames): " + curFrame +
                        " Object does not exist");
            return;
        }



        Vector3 vector = instance.transform.localPosition;
        double x = instance.transform.position.x;
        double y = instance.transform.position.y;
        double z = instance.transform.position.z;
        double xr = instance.transform.rotation.eulerAngles.x;
        double yr = instance.transform.rotation.eulerAngles.y;
        double zr = instance.transform.rotation.eulerAngles.z;
        double xs = instance.transform.localScale.x;
        double ys = instance.transform.localScale.y;
        double zs = instance.transform.localScale.z;
        Debug.Log("The real values of the object: " +
            " Object Number: " + objects[i].objectID +
            " Frame number: " + curFrame+
            " XYZ position: " + vector.x + " | " + vector.y + " | " + vector.z +
            " XYZ rotation: " + xr + " | " + yr + " | " + zr +
            " XYZ scale: " + xs + " | " + ys + " | " + zs
            );
    }
    public void LogIntended(int i, int curFrame) //i is a loop counter for all objects
    {
        int preFrame = 0;
        int postFrame = 0;
        if (objects[i].keyCount > 0)
        {
            //if before or after animation
            if (objects[i].key[0].frameNum > curFrame || objects[i].key[objects[i].keyCount - 1].frameNum < curFrame)
            {
                //print the object id, the time, and "Object does not exist".
                Debug.Log("Object ID: " + objects[i].objectID +
                            " Time (frames): " + curFrame +
                            " Object does not exist");
                return;
            }
            for (int j = 0; j < objects[i].keyCount; j++)
            {
                if (objects[i].key[j].frameNum == curFrame)
                {
                    preFrame = j;
                    postFrame = j;
                    break;
                }
                if (objects[i].key[j].frameNum < curFrame
                    && objects[i].key[j + 1].frameNum > curFrame)
                {
                    preFrame = j;
                    postFrame = j+1;
                    break;
                }
            }
            int deltaFrame = objects[i].key[postFrame].frameNum - objects[i].key[preFrame].frameNum;
            int frameProgress = curFrame - objects[i].key[preFrame].frameNum;
            if (deltaFrame == 0) // used to avoid dividing by 0 and to zero the term in the variable calculation
            {
                frameProgress = 0;
                deltaFrame = 1;
            }
            double progressRatio = (double)frameProgress / (double)deltaFrame;
            double x = objects[i].key[preFrame].vector.x + (objects[i].key[postFrame].vector.x - objects[i].key[preFrame].vector.x) * progressRatio;
            double y = objects[i].key[preFrame].vector.y + (objects[i].key[postFrame].vector.y - objects[i].key[preFrame].vector.y) * progressRatio;
            double z = objects[i].key[preFrame].vector.z + (objects[i].key[postFrame].vector.z - objects[i].key[preFrame].vector.z) * progressRatio;

            double xr = objects[i].key[preFrame].rotation.x + (objects[i].key[postFrame].rotation.x - objects[i].key[preFrame].rotation.x) * progressRatio;
            double yr = objects[i].key[preFrame].rotation.y + (objects[i].key[postFrame].rotation.y - objects[i].key[preFrame].rotation.y) * progressRatio;
            double zr = objects[i].key[preFrame].rotation.z + (objects[i].key[postFrame].rotation.z - objects[i].key[preFrame].rotation.z) * progressRatio;

            double xs = objects[i].key[preFrame].scale.x + (objects[i].key[postFrame].scale.x - objects[i].key[preFrame].scale.x) * progressRatio;
            double ys = objects[i].key[preFrame].scale.y + (objects[i].key[postFrame].scale.y - objects[i].key[preFrame].scale.y) * progressRatio;
            double zs = objects[i].key[preFrame].scale.y + (objects[i].key[postFrame].scale.z - objects[i].key[preFrame].scale.z) * progressRatio;

                Debug.Log("The intended values from file: " +
                " Object Number: " + objects[i].objectID +
                " Frame number: " + curFrame +
                " XYZ position: " + x + " | " + y + " | " + z +
                " XYZ rotation: " + xr + " | " + yr + " | " + zr +
                " XYZ scale: " + xs + " | " + ys + " | " + zs
                );

        }
        else
        {
            Debug.Log("No keyframes present");
        }
    }
    public static void ReadTextFile()
    {
        //*********************************************************** reading data *************************************

        eventList[] events = { };
        eventList event1 = new eventList();
        eventList[] emptyList = { event1 };
        keyFrame[] keys = { };
        keyFrame keyFrame1 = new keyFrame();
        keyFrame[] emptyKey = { keyFrame1 };
        string[] Data = { };
        string[] Splitted = { };
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
        int keyCount = 0;
        int eventCount = 0;
        //parsing Data array into object and keyframe arrays
        for (int i = 0; i < Data.Length; i++)
        {
            //Collects all objects in object array
            if (Data[i] == "OBJECT")
            {

                if (int.TryParse(Data[i + 1], out var number))
                {
                    objects[obCount].key = emptyKey;//initialize to empty array
                    objects[obCount].events = emptyList;
                    objects[obCount].objectID = number; // record item ID
                    objects[obCount].fileName = Data[i + 2];
                    objects[obCount].keyCount = 0;
                    i = i + 1;
                }
                else
                {
                    ///error
                    Debug.Log("Error: Object definition.  Please follow the correct format in .txt files");
                }
                obCount++;
            }
            //Collects all keyframes in keyframe array
            //Done this way to allow keyframes to come before objects
            if (Data[i] == "KEYFRAME")
            {

                int.TryParse(Data[i + 1], out var objectID);
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

                keys[keyCount].objectID = objectID;
                keys[keyCount].frameNum = frameNum;
                keys[keyCount].vector = new Vector3(px, py, pz);
                keys[keyCount].rotation = new Vector3(rx, ry, rz);
                keys[keyCount].scale = new Vector3(sx, sy, sz);
                keyCount++;
                i = i + 10;
            }
            if (Data[i] == "EVENT")
            {

                int.TryParse(Data[i + 1], out var objectID);
                string eventName = Data[i + 2];
                int.TryParse(Data[i + 3], out var frameNum);
                

                events = events.Concat(emptyList).ToArray(); //Increase array size by 1

                events[eventCount].objectID = objectID;
                events[eventCount].frameNum = frameNum;
                events[eventCount].eventName = eventName;


                eventCount++;
                i = i + 2;
            }
        }
        //for every keyframe find its object and put it in it.  
        int indexO = -1;
        int indexK;
        for (int i = 0; i < keys.Length; i++)
        {
            indexO = -1;
            for (int j = 0; j < obCount; j++)
            {
                if (keys[i].objectID == objects[j].objectID)
                {
                    indexO = j;
                    break;
                }
            }
            // **  Report an error if a key frame refers to an object that has never been specified. **
            if (indexO == -1)
            {
                Debug.Log("Error: Key frame refers to an object that has never been specified.");
                continue;
            }
            indexK = objects[indexO].keyCount;
            objects[indexO].key = objects[indexO].key.Concat(emptyKey).ToArray(); //Increase array size by 1
            objects[indexO].key[indexK] = keys[i];
            // ** Report an error if a time for a key frame is less than the time for the previous key frame for the same object.**
            if (indexK > 0 && objects[indexO].key[indexK].frameNum < objects[indexO].key[indexK - 1].frameNum)
            {
                Debug.Log("Error: Object " + indexO + " key frame is less than the time for the previous key frame.");
            }


            objects[indexO].keyCount = objects[indexO].keyCount + 1;
        }
        //for every event find its object and put it in it
        for (int i = 0; i < events.Length; i++)
        {
            indexO = -1;
            for (int j = 0; j < obCount; j++)
            {
                if (events[i].objectID == objects[j].objectID)
                {
                    indexO = j;
                    break;
                }
            }
            // **  Report an error if a key frame refers to an object that has never been specified. **
            if (indexO == -1)
            {
                Debug.Log("Error: Event refers to an object that has never been specified.");
            }
            indexK = objects[indexO].eventCount;
            objects[indexO].events = objects[indexO].events.Concat(emptyList).ToArray(); //Increase array size by 1
            objects[indexO].events[indexK] = events[i];
            //No need to name events in order.  Limit of 1 event per frame per object.


            objects[indexO].eventCount = objects[indexO].eventCount + 1;
        }
    }

    public static AnimationCurve linear(AnimationCurve curve)
    {
        float intangent = 0;
        float outtangent = 0;
        for (int i = 0; i < curve.keys.Length; ++i)
        {
            bool hasLeft = true;
            bool hasRight = true;
            float x1;
            float x2;
            float y1;
            float y2;
            float deltaX;
            float deltaY;

            if (i == 0) // If it is the first keyframe
            {
                hasLeft = false;
                intangent = 0; 
            }

            if (i == curve.keys.Length - 1) // if last keyframe
            {
                hasRight = false;
                outtangent = 0; 
            }

            if (hasLeft) 
            {
                x1 = curve.keys[i - 1].time;
                x2 = curve.keys[i].time;
                y1 = curve.keys[i - 1].value;
                y2 = curve.keys[i].value;
                deltaX = x2 - x1;
                deltaY = y2 - y1;
                intangent = deltaY / deltaX;  //rise/run
            }
            if (hasRight)
            {
                x1 = curve.keys[i].time;
                x2 = curve.keys[i+1].time;
                y1 = curve.keys[i].value;
                y2 = curve.keys[i+1].value;
                deltaX = x2 - x1;
                deltaY = y2 - y1;
                outtangent = deltaY / deltaX;
            }
            Keyframe k = curve[i];
            k.inTangent = intangent;
            k.outTangent = outtangent;
            curve.MoveKey(i, k); //update tangent
        }
        return curve;
    }


    public static void CreateAnimations()
    {
        int keyCount = 0;
        for (int i = 0; i < obCount; i++)
        {
            //create an instance and save it
            GameObject instance = Instantiate(Resources.Load(objects[i].fileName, typeof(GameObject))) as GameObject;
            objects[i].instance = instance;
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

            float speed = 60f;
            //KeyFrame test = new KeyFrame(20f, 2f);
            for (int j = 0; j < keyCount; j++)
            {
                //keyframe constructor parameters = keyframe(time(s),the thing you name)
                //60 frames per second.  The first field is in seconds.  Therefore divide by 60.
                
                //XYZ
                keyframeX[j] = new Keyframe(objects[i].key[j].frameNum / speed, objects[i].key[j].vector.x);
                keyframeY[j] = new Keyframe(objects[i].key[j].frameNum / speed, objects[i].key[j].vector.y);
                keyframeZ[j] = new Keyframe(objects[i].key[j].frameNum / speed, objects[i].key[j].vector.z);
                //XYZ rotation
                keyframeXR[j] = new Keyframe(objects[i].key[j].frameNum / speed, objects[i].key[j].rotation.x);
                keyframeYR[j] = new Keyframe(objects[i].key[j].frameNum / speed, objects[i].key[j].rotation.y);
                keyframeZR[j] = new Keyframe(objects[i].key[j].frameNum / speed, objects[i].key[j].rotation.z);
                //XYZ scale
                keyframeXS[j] = new Keyframe(objects[i].key[j].frameNum / speed, objects[i].key[j].scale.x);
                keyframeYS[j] = new Keyframe(objects[i].key[j].frameNum / speed, objects[i].key[j].scale.y);
                keyframeZS[j] = new Keyframe(objects[i].key[j].frameNum / speed, objects[i].key[j].scale.z);

            }
            //xyz
            clip.SetCurve("", typeof(Transform), "localPosition.x", linear(new AnimationCurve(keyframeX)));
            clip.SetCurve("", typeof(Transform), "localPosition.y", linear(new AnimationCurve(keyframeY)));
            clip.SetCurve("", typeof(Transform), "localPosition.z", linear(new AnimationCurve(keyframeZ)));
            //rotation
            clip.SetCurve("", typeof(Transform), "localEulerAngles.x", linear(new AnimationCurve(keyframeXR)));
            clip.SetCurve("", typeof(Transform), "localEulerAngles.y", linear(new AnimationCurve(keyframeYR)));
            clip.SetCurve("", typeof(Transform), "localEulerAngles.z", linear(new AnimationCurve(keyframeZR)));
            //scale
            clip.SetCurve("", typeof(Transform), "localScale.x", linear(new AnimationCurve(keyframeXS)));
            clip.SetCurve("", typeof(Transform), "localScale.y", linear(new AnimationCurve(keyframeYS)));
            clip.SetCurve("", typeof(Transform), "localScale.z", linear(new AnimationCurve(keyframeZS)));

            //apply animation
            animation.AddClip(clip, clip.name);


            AnimationEvent evnt = new AnimationEvent();
            //Change Visibility functions
            instance.AddComponent<ChangeVisibility>(); // add script
            //Hide when finished
            evnt.time = objects[i].key[objects[i].keyCount - 1].frameNum/speed;
            evnt.functionName = "HideEvent";
            clip.AddEvent(evnt);                   // add event for script
            //Hide when created
            if (objects[i].key[0].frameNum > 0f)
            {
                evnt.time = 0f;
                evnt.functionName = "HideEvent";
                clip.AddEvent(evnt);                   // add event for script
            }
            //Unhide when first frame
            evnt.time = objects[i].key[0].frameNum / speed;
                evnt.functionName = "UnhideEvent";
                clip.AddEvent(evnt);                   // add event for script

            //Dynamically add any more events specified
            for (int j = 0; j < objects[i].eventCount; j++)
            {
                if(j == 0)
                {
                    instance.AddComponent<customEvent>(); // add script only once
                }
                evnt.functionName = objects[i].events[j].eventName;
                evnt.time = objects[i].events[j].frameNum/speed;
                clip.AddEvent(evnt);
            }

            //animation[clip.name].speed = 1f;
            animation.Play(clip.name);

            foreach (AnimationState state in animation)
            {
                state.speed = 1F;
            }

        }
    }
}
public struct eventList
{
    public string eventName;
    public int objectID;
    public int frameNum;
}
    public struct keyFrame
{
    public int objectID;
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
    public int objectID;
    public GameObject instance;
    public eventList[] events;
    public int eventCount;
};

// update the clip to a change the red color
//curve = AnimationCurve.Linear(0.0f, 1.0f, 2.0f, 0.0f);
//clip.SetCurve("", typeof(Material), "_Color.r", curve);