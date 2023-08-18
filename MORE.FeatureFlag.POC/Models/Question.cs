using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MORE.FeatureFlag.POC.Models
{
    public class Question
    {
        [Key]
        public int Id { get; set; }
        public int QuestionTypeId { get; set; }
        public string QuestionName { get; set; }
        public int DisplayOrder { get; set; }
    }
}
