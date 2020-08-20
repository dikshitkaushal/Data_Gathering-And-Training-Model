using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Drive : MonoBehaviour
{
    public float Translate_Speed = 50f;
    public float Rotation_Speed = 100f;
    float Translateinput;
    float Rotateinput;
    float visibledistance = 200f;
    List<string> CollectedTrainingData = new List<string>();
    StreamWriter trainingsetfile;
    // Start is called before the first frame update
    void Start()
    {
        string path = Application.dataPath + "/TrainingData.txt";
        trainingsetfile = File.CreateText(path);
    }
    float Round(float x)
    {
        return (float)System.Math.Round(x, System.MidpointRounding.AwayFromZero) / 2.0f;
    }
    private void OnApplicationQuit()
    {
        foreach(string td in CollectedTrainingData)
        {
            trainingsetfile.WriteLine(td);
        }
        trainingsetfile.Close();
    }
    // Update is called once per frame
    void Update()
    {
        Translateinput = Input.GetAxis("Vertical");
        Rotateinput = Input.GetAxis("Horizontal");
        float Translate = Translate_Speed * Time.deltaTime * Translateinput;
        float Rotate = Rotation_Speed * Time.deltaTime * Rotateinput;
        transform.Translate(0, 0, Translate);
        transform.Rotate(0, Rotate, 0);
        Debug.DrawRay(transform.position, transform.forward, Color.red, visibledistance);
        Debug.DrawRay(transform.position, transform.right, Color.red, visibledistance);
        RaycastHit hit;
        float fdist = 0;
        float rdist = 0;
        float ldist = 0;
        float l45dist = 0;
        float r45dist = 0;
        if(Physics.Raycast(transform.position,transform.forward,out hit,visibledistance))
        {
            fdist = 1 - Round(hit.distance / visibledistance);
        }
        if (Physics.Raycast(transform.position, transform.right, out hit, visibledistance))
        {
            rdist = 1 - Round(hit.distance / visibledistance);
        }
        if (Physics.Raycast(transform.position, -transform.right, out hit, visibledistance))
        {
            ldist = 1 - Round(hit.distance / visibledistance);
        }
        if (Physics.Raycast(transform.position, Quaternion.AngleAxis(45,Vector3.up)*-transform.right, out hit, visibledistance))
        {
            r45dist = 1 - Round(hit.distance / visibledistance);
        }
        if (Physics.Raycast(transform.position, Quaternion.AngleAxis(-45, Vector3.up) * transform.right, out hit, visibledistance))
        {
            l45dist = 1 - Round(hit.distance / visibledistance);
        }
        string td = fdist + "," + rdist + "," + ldist + "," + r45dist + "," + l45dist + "," + Round(Translateinput) + "," + Round(Rotateinput);
        if(!CollectedTrainingData.Contains(td))
        {
            CollectedTrainingData.Add(td);
        }
    }
}
