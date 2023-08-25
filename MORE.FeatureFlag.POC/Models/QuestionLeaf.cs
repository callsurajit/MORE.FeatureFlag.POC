using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MORE.FeatureFlag.POC.Models
{
    public class QuestionLeaf
    {
        public QuestionLeaf() {
            this.QuestionLeaves = new List<QuestionLeaf>() ;
        }

        [Key]
        public int QuestionId { get; set; }
        public string QuestionTypeName { get; set; }
        public string QuestionName { get; set; }
        //public int DisplayOrder { get; set; }
        public string OptionValue { get; set; }
        public ICollection<QuestionLeaf> QuestionLeaves { get; set; }
    }
}
