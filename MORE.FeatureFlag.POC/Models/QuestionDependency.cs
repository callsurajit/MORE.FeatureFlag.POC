using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MORE.FeatureFlag.POC.Models
{
    public class QuestionDependency {
        [Key]
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string QuestionValue { get; set; }
        public int? ChildQuestionId { get; set; }
    }
}
