using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MORE.FeatureFlag.POC.Models
{
    public class OptionRequestDto
    {
        [Key]
        public int QuestionId { get; set; }
        public string? Optionvalue { get; set; }
        public int? ChildQuestionId { get; set; }
        public int OperationId { get; set; }
    }
}
