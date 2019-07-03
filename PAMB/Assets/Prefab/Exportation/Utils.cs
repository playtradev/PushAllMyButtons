using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

namespace PlaytraGamesLtd
{
	public class Utils
	{
		public static void Serialize<T>(T obj, string path)
		{
			var serializer = new XmlSerializer(obj.GetType());
			using (var writer = XmlWriter.Create(path))
			{
				serializer.Serialize(writer, obj);
			}
		}

		public static T Deserialize<T>(string file) where T : class
		{
			var serializer = new XmlSerializer(typeof(T));
			using (StreamReader reader = new StreamReader(file))
			{
				return serializer.Deserialize(reader) as T;
			}
		}

		public static string SerializeToString<T>(T obj)
		{
			var serializer = new XmlSerializer(obj.GetType());
			using (StringWriter textWriter = new StringWriter())
			{
				serializer.Serialize(textWriter, obj);
				return textWriter.ToString();
			}
		}

		public static T DeserializeFromString<T>(string Value) where T : class
		{
			var serializer = new XmlSerializer(typeof(T));
			using (var reader = new System.IO.StringReader(Value))
			{
				return serializer.Deserialize(reader) as T;
			}
		}

		public static LevelStorageClass DeserializeStreamingAssetsCSV(string path)
		{
			List<string> stringList = new List<string>();
			StreamReader inp_stm = new StreamReader(path);
            while (!inp_stm.EndOfStream)
            {
                string inp_ln = inp_stm.ReadLine();
                stringList.Add(inp_ln);
            }
            inp_stm.Close();

			return DeserializerCSVtoLevelStorageClass(stringList);
		}

		public static LevelStorageClass DeserializeResourcesCSV(string path)
        {

			TextAsset PrnFile = Resources.Load(path) as TextAsset;
            List<string> stringList = new List<string>();
			stringList.AddRange(PrnFile.text.Split('\n').ToList());
			stringList.Remove(stringList.Last());
            return DeserializerCSVtoLevelStorageClass(stringList);
        }

		public static LevelStorageClass DeserializerCSVtoLevelStorageClass(List<string> stringList)
		{
			LevelStorageClass res = new LevelStorageClass();
			LevelStageClass stage;
            for (int i = 1; i < stringList.Count; i++)
            {
                stage = new LevelStageClass();
                string[] temp = stringList[i].Split(',');

                stage.ID = Convert.ToInt16(temp[0].Trim());
                stage.StagePosition = Convert.ToInt16(temp[1].Trim());
                LevelClass level;
                for (int y = 2; y < 11; y++)
                {
                    level = new LevelClass(Convert.ToInt16(temp[y].Trim()), 4);
                    if (y < 10)
                    {
                        stage.LevelsInStage.Add(level);
                    }
                    else
                    {
                        stage.ExtraLevel = level;
                    }
                }
				stage.IsCircuitVisible = temp[11].ToLower().Contains("true") ? true : false;
                res.LevelStage.Add(stage);
            }

			return res;
		}

        public static void WriteLog(string Log, Color c)
        {
            string path;
            Log = Environment.NewLine + Log;
#if UNITY_EDITOR
            path = System.IO.Path.Combine(Application.streamingAssetsPath, "Log.txt");
            try
            {
                using (TextWriter outputFile = new StreamWriter(path, true))
                {
                    //GameManagerScript
                    if (c == Color.red)
                    {
                        outputFile.WriteLine("<color=#ff0000ff>" + Log + "</color>");
                    }
                    //CharacterBase
                    if (c == Color.green)
                    {
                        outputFile.WriteLine("<color=#00ff00ff>" + Log + "</color>");
                    }
                    //AI
                    if (c == Color.blue)
                    {
                        outputFile.WriteLine("<color=#00ffffff>" + Log + "</color>");
                    }
                    //UI/UserInput
                    if (c == Color.yellow)
                    {
                        outputFile.WriteLine("<color=#ffff00ff>" + Log + "</color>");
                    }
                    //Info
                    if (c == Color.black)
                    {
                        outputFile.WriteLine(Log);
                    }
                }
            }
            catch (System.Exception ex)
            {
                //Debug.Log("Error  1 " + ex.Message + ex.Source);
            }

#elif UNITY_ANDROID && !UNITY_EDITOR
        path = System.IO.Path.Combine(Application.persistentDataPath, "Log.txt");
        Debug.Log("Enter");
        try
        {
            using (TextWriter outputFile = new StreamWriter(path, true))
            {
                //GameManagerScript
                if (c == Color.red)
                {
                    outputFile.WriteLine("<color=#ff0000ff>" + Log + "</color>");
                }
                //CharacterBase
                if (c == Color.green)
                {
                    outputFile.WriteLine("<color=#00ff00ff>" + Log + "</color>");
                }
                //AI
                if (c == Color.blue)
                {
                    outputFile.WriteLine("<color=#00ffffff>" + Log + "</color>");
                }
                //UI/UserInput
                if (c == Color.yellow)
                {
                    outputFile.WriteLine("<color=#ffff00ff>" + Log + "</color>");
                }
                //Info
                if (c == Color.black)
                {
                    outputFile.WriteLine(Log);
                }
            }
        }
        catch(System.Exception ex)
        {
        Debug.Log("Error  1 " + ex.Message + ex.Source);
        }
#elif UNITY_IOS && !UNITY_EDITOR

        path = System.IO.Path.Combine(Application.persistentDataPath, "Log.txt");
        try
        {
            
        Debug.Log("FileStream");
            //GameManagerScript
            if (c == Color.red)
            {
                File.AppendAllText(path, "<color=#ff0000ff>" + Log + "</color>");
            }
            //CharacterBase
            if (c == Color.green)
            {
                File.AppendAllText(path,"<color=#00ff00ff>" + Log + "</color>");
            }
            //AI
            if (c == Color.blue)
            {
                File.AppendAllText(path,"<color=#00ffffff>" + Log + "</color>");
            }
            //UI/UserInput
            if (c == Color.yellow)
            {
                File.AppendAllText(path,"<color=#ffff00ff>" + Log + "</color>");
            }

            //Info
            if (c == Color.black)
            {
                File.AppendAllText(path,Log);
            }
           
        }
        catch (System.Exception ex)
        {
            Debug.Log("Error  0 " + ex.Message + ex.Source);
        }


#endif


        }

        public static void ClearLog()
        {
            string path;
#if UNITY_EDITOR
            path = Application.dataPath + "Log.txt";
#elif UNITY_ANDROID && !UNITY_EDITOR
path = System.IO.Path.Combine(Application.persistentDataPath, "Log.txt");
        try
        {
            using (StreamWriter outputFile = new StreamWriter(path, true))
            {
                outputFile.WriteLine("");
            }
        }
        catch(System.Exception ex)
        {
        Debug.Log("Error  0 " + ex.Message + ex.Source);
        }
#elif UNITY_IOS && !UNITY_EDITOR
        path = System.IO.Path.Combine(Application.persistentDataPath, "Log.txt");
        try
        {
            byte[] info = new UTF8Encoding(true).GetBytes("");
            Debug.Log("Clean 1 ");
            File.WriteAllBytes(path, info);
        }
        catch (System.Exception ex)
        {
            Debug.Log("Error  0 " + ex.Message + ex.Source);
        }
#endif


        }
    }
}


