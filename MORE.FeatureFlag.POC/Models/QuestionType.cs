using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MORE.FeatureFlag.POC.Models
{
    public class QuestionType
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
