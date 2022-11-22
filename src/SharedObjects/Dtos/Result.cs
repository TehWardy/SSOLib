using SharedObjects.Extensions;
using System.ComponentModel.DataAnnotations;

namespace SharedObjects.Dtos
{
    public class Result<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        string id = null;

        [Key]
        public string Id
        {
            get
            {
                if (id == null)
                {
                    try
                    {
                        return Item?.GetId()?.ToString();
                    }
                    catch { return null; }
                }
                else
                {
                    return id;
                }
            }
            set { id = value; }
        }

        public T Item { get; set; }
    }
}
