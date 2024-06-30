using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;

public class TodoManager : MonoBehaviour
{
    public TodoItem todoItemPrefab; // Assign the TodoItem prefab in the Inspector
    public Transform content; // Assign the Content GameObject of the Scroll View in the Inspector

    private string url = "https://dummyjson.com/todos?limit=20";

    void Start()
    {
        StartCoroutine(GetTodos());
    }

    IEnumerator GetTodos()
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
        }
        else
        {
            // Parse the JSON response
            TodoResponse response = JsonUtility.FromJson<TodoResponse>(request.downloadHandler.text);
            DisplayTodos(response.todos);
            
        }
    }
    void DisplayTodos(List<Todo> todos)
    {
        foreach (Todo todo in todos)
        {
            TodoItem todoItem = Instantiate(todoItemPrefab, content);
            todoItem.descriptionTxt.text = todo.todo;
            todoItem.toggle.isOn = todo.completed;
            todoItem.userIdTxt.text = todo.userId.ToString();
        }
    }
    IEnumerator FetchTodos()
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to fetch todos: " + request.error);
        }
        else
        {
            JObject json = JObject.Parse(request.downloadHandler.text);
            JArray todos = (JArray)json["todos"];
            PopulateTodos(todos);
        }
    }

    void PopulateTodos(JArray todos)
    {
        foreach (var todo in todos)
        {
            TodoItem todoItem = Instantiate(todoItemPrefab, content);
            todoItem.descriptionTxt.text = todo["todo"].ToString();
            todoItem.toggle.isOn = (bool)todo["completed"];
        }
    }
}