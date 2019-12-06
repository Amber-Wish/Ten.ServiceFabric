using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Ten.DDD.DomainCore.Models
{
    /// <summary>
    /// 分页请求参数
    /// </summary>
    public class PageRequest
    {
        /// <summary>
        /// 当前页
        /// </summary>
        [JsonProperty("pageIndex")]
        public int PageIndex { get; set; }
        /// <summary>
        /// 每页行数
        /// </summary>
        [Range(1, 500)]
        [JsonProperty("pageNumber")]
        public int PageNumber { get; set; } = 20;
        /// <summary>
        /// 排序列
        /// </summary>
        [JsonProperty("sortField")]
        public string SortField { get; set; }
        /// <summary>
        /// 排序类型：asc/desc
        /// </summary>
        [JsonProperty("sort")]
        public string Sort { get; set; }
        /// <summary>
        /// 总记录数
        /// </summary>
        [JsonProperty("total")]
        public long Total { get; set; }
        /// <summary>
        /// 查询条件Json
        /// </summary>
        [JsonProperty("conditionJson")]
        public string ConditionJson { get; set; }
        /// <summary>
        /// 查询关键词
        /// </summary>
        [JsonProperty("searchKeyWord")]
        public string SearchKeyWord { get; set; }
    }
}