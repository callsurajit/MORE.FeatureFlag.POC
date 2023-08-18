using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MORE.FeatureFlag.POC.Models
{
    public class QuestionDto
    {
        [Key]
        public int QuestionId { get; set; }
        public string QuestionName { get; set; }
        public string QuestionTypeName { get; set; }
        public string QuestionValue { get; set; }
        public int? ChildQuestionId { get; set; }
        public int DisplayOrder { get; set; }
    }
}
