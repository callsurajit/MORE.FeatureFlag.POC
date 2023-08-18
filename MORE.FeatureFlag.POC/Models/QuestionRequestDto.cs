using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MORE.FeatureFlag.POC.Models
{
    public class QuestionRequestDto
    {
        public QuestionRequestDto() {
            this.QuestionDependencyList = new List<QuestionDependency>();
        }

        [Key]
        public int QuestionId { get; set; }
        public string QuestionName { get; set; }
        public int QuestionTypeId { get; set; }
        public int DisplayOrder { get; set; }
        public ICollection<QuestionDependency> QuestionDependencyList { get; set; }
    }
}
