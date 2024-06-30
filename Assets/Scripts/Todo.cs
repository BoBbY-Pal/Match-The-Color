
using UnityEngine.Serialization;

[System.Serializable]
public class Todo
{
    public int id;
    [FormerlySerializedAs("description")] public string todo;
    public bool completed;
    public int userId;
}
