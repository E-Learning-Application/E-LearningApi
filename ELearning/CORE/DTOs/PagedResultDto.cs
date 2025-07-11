﻿namespace CORE.DTOs
{
    public class PagedResultDto<T>
    {
        public List<T> Data { get; set; } = null!;
        public int TotalPages { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
