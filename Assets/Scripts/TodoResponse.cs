using System.Collections.Generic;

namespace DefaultNamespace
{
    [System.Serializable]
    public class TodoResponse
    {
        public List<Todo> todos;
        public int total;
        public int skip;
        public int limit;
    }
}