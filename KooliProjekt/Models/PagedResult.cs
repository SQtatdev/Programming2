namespace KooliProjekt.Models
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; } // Список элементов текущей страницы
        public int TotalCount { get; set; } // Общее количество элементов
        public int PageNumber { get; set; } // Номер текущей страницы
        public int PageSize { get; set; } // Размер страницы

        // Конструктор, если нужен
        public PagedResult()
        {
            Items = new List<T>();
        }
    }
}
