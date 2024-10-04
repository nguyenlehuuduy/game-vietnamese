using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class QuestionModel<T>
{
    public int questionId;
    public TopicId topicId;
    public Type type;
    public Content content;
    public T answer;
    public string[] options;
}

[Serializable]
public class Content
{
    public string des;
    public string[] images;
}

[Serializable]
public class TopicId
{
    public int idTopic;
    public int idLevelSubject;
    public string nameSubject;
    public int idRound;
}

[Serializable]
public class Type
{
    public int typeId;
    public string typeName;
}


[Serializable]
public class JSONDataQuestionClass<T>
{
    public QuestionModel<T>[] question;
}