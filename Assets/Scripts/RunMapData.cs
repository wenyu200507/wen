[System.Serializable]
public class audio
{
    public string id;
    public string sha1;
}
[System.Serializable]
public class image
{
    public string id;
    public string sha1;
}
[System.Serializable]
public class item
{
    public audio audio;
}
[System.Serializable]
public class options
{
    public string id;
    public int rowIndex;
    public int colIndex;
    public image image;
}
[System.Serializable]
public class Body
{
    public item item;
    public answers[][] answers;
    public options options;
}
[System.Serializable]
public class Stimulus
{
    public Body body;
}
[System.Serializable]
public class answers
{
    public string stimulusOfQuestion;
}
[System.Serializable]
public class Data
{
    public Activity activity;
}
[System.Serializable]
public class stimulusOfQuestion
{
    public string body;
}
[System.Serializable]
public class QuestionsData
{
    //public Body body;
    public string Key;
}
[System.Serializable]
public class Questions
{
    public QuestionsData[] questions1;
}
[System.Serializable]
public class Activity
{
    public Questions questions;
    public string ProductName;
}
