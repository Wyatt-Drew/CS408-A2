using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
/*
public class readtxt : MonoBehaviour
{
    var fileName = "animation.txt";
    public Transform contentWindow;
	//public gameObject recallTextObject;

	char node_type;
	bool is_death1 = false;
	int description1, north1, east1, west1, south1;
	char unused_char;
	int unused_int1, unused_int2;
	ifstream input_file;
	input_file.open(filename);
	if (input_file.fail())
	{
		cout << "Error: File failed to open";
	}
	else
{
	input_file >> node_count;
	input_file >> start_node >> victory_node >> start_message >> end_message;
	for (int i = 0; i < node_count; i++)
	{
		input_file >> node_type;
		if (node_type == 'D')
		{
			is_death1 = true;
		}
		else
		{
			is_death1 = false;
		}
		input_file >> description1 >> north1 >> south1 >> east1 >> west1;
		nodes[i] = Node(description1, north1, south1, east1, west1, is_death1);
		if (node_type == 'O')
		{
			input_file >> unused_char >> unused_int1 >> unused_int2;
		}
	}
}
input_file.close();
return;
}

    // Start is called before the first frame update
    void Start()
    {
        //string readFromFilePath = Application.textFilesPath + "animation.txt";
        StreamReader inp_stm = new StreamReader(file_path);

        while (!inp_stm.EndOfStream)
        {
            string inp_ln = inp_stm.ReadLine();
            // Do Something with the input. 
        }

        inp_stm.Close();
    }


}
*/