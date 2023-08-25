using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MORE.FeatureFlag.POC.Models
{
    public class QuestionTreeDto
    {
        public QuestionTreeDto() {
            //Label = "Daily Substitute Permit";
            this.QuestionLeaves = new List<QuestionLeaf>();
            //this.QuestionLeaves = new List<IDictionary<string, QuestionLeaf>>();
        }

        //public string Label { get; set; }
        public ICollection<QuestionLeaf> QuestionLeaves { get; set; }
        //public ICollection<IDictionary<string, QuestionLeaf>> QuestionLeaves { get; set; }
    }
}
