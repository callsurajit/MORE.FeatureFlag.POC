using Microsoft.EntityFrameworkCore;
using MORE.FeatureFlag.POC.Models;
using System.Collections.ObjectModel;

namespace MORE.FeatureFlag.POC.Data {
    public class QuestionService : IQuestionService {
        private readonly MOREPOCContext _context;
        private IConfiguration _config;

        public QuestionService(MOREPOCContext context, IConfiguration config) {
            _context = context;
            _config = config;
        }

        public async Task<List<QuestionDto>> GetQuestionsAsync() {
            return await (from q in _context.Questions
                          join qd in _context.QuestionDependencys on q.Id equals qd.QuestionId into t
                          from nt in t.DefaultIfEmpty()
                          join qt in _context.QuestionTypes on q.QuestionTypeId equals qt.Id
                          orderby q.DisplayOrder
                          select new QuestionDto
                          {
                              QuestionId = q.Id,
                              QuestionName = q.QuestionName,
                              QuestionTypeName = qt.Name,
                              QuestionValue = nt.QuestionValue,
                              ChildQuestionId = nt.ChildQuestionId,
                              DisplayOrder = q.DisplayOrder
                          }).ToListAsync();
        }

        public async Task<List<QuestionLeaf>> GetDecisionTreeAsync() {
            List<int> processedQuestion = new List<int>();
            //var decisionTree = new QuestionTreeDto();
            var decisionTree = new List<QuestionLeaf>();           
            // get the list of all the questions who are not children without the metadata
            var questionList = (from c in _context.Questions
                         where !(from o in _context.QuestionDependencys
                                select o.ChildQuestionId)
                               .Contains(c.Id)
                        select c).ToList();

            //var leafDict = new Dictionary<string, QuestionLeaf>();
            foreach (Question ques in questionList) {
                // check if the questionId is already processed
                if (processedQuestion.IndexOf(ques.Id) != -1)
                    continue;

                // create the leaves for each question
                var leaf = await createLeaves(ques, processedQuestion);
                decisionTree.Add(leaf);
            }            

            return decisionTree;
        }

        private async Task<QuestionLeaf> createLeaves(Question ques, List<int> processedQuestion) {
            // get the questionnaire details includes the metadata
            var questionDetails = await GetQuestionsAsync();

            var children = questionDetails.FindAll(x => x.QuestionId == ques.Id).ToList();
            var leaf0 = new QuestionLeaf(); // Q1
            leaf0.QuestionId = children[0].QuestionId;
            leaf0.QuestionName = children[0].QuestionName;
            leaf0.QuestionTypeName = children[0].QuestionTypeName;
            leaf0.OptionValue = string.Empty;
            processedQuestion.Add(ques.Id);

            if (children.Count > 1) {
                foreach (QuestionDto quesdto in children) {
                    QuestionLeaf leaf1 = new QuestionLeaf();
                    leaf1.OptionValue = quesdto.QuestionValue;
                    leaf1.QuestionName = string.Empty;
                    leaf1.QuestionTypeName = string.Empty;
                    if (quesdto.ChildQuestionId != null) {
                        var childn2 = questionDetails.FindAll(x => x.QuestionId == quesdto.ChildQuestionId).ToList();
                        leaf1.QuestionId = childn2[0].QuestionId;
                        leaf1.QuestionName = childn2[0].QuestionName; 
                        leaf1.QuestionTypeName = childn2[0].QuestionTypeName;
                        processedQuestion.Add(childn2[0].QuestionId);
                        if (childn2.Count > 1) {
                            foreach (QuestionDto quesn2 in childn2) {
                                QuestionLeaf leaf2 = new QuestionLeaf();
                                leaf2.QuestionName = string.Empty;
                                leaf2.QuestionTypeName = string.Empty;
                                leaf2.OptionValue = quesn2.QuestionValue;
                                if (quesn2.ChildQuestionId != null) {
                                    var childn3 = questionDetails.FindAll(x => x.QuestionId == quesn2.ChildQuestionId).ToList();
                                    leaf2.QuestionId = childn3[0].QuestionId;
                                    leaf2.QuestionName = childn3[0].QuestionName;
                                    leaf2.QuestionTypeName = childn3[0].QuestionTypeName;
                                    processedQuestion.Add(childn3[0].QuestionId);
                                }
                                leaf1.QuestionLeaves.Add(leaf2);
                            }
                        }
                    }
                    leaf0.QuestionLeaves.Add(leaf1);
                }
            }

            return leaf0;
        }

        //private async Task<QuestionLeaf> recurseLeaves(QuestionDto quesDto, List<int> processedQuestion) {           
        //    // get the questionnaire details includes the metadata
        //    var questionDetails = await GetQuestionsAsync();

        //    var childList = questionDetails.FindAll(x => x.QuestionId == quesDto.ChildQuestionId).ToList();
        //    var leafleaf = new Dictionary<string, QuestionLeaf>();
        //    var leaf = new QuestionLeaf();
        //    leaf.QuestionId = childList[0].QuestionId;
        //    leaf.QuestionName = childList[0].QuestionName;
        //    leaf.QuestionTypeName = childList[0].QuestionTypeName;
        //    processedQuestion.Add(childList[0].QuestionId);
        //    if (childList.Count == 1) 
        //        return leaf;              
        //    else {
        //        foreach (QuestionDto quesn2 in childList) {
        //            var leafleafn2 = new Dictionary<string, QuestionLeaf>();
        //            QuestionLeaf leaf2 = null;
        //            if (quesn2.ChildQuestionId != null) {
        //                leaf2 = await recurseLeaves(quesn2, processedQuestion);
        //                //var childn3 = questionDetails.FindAll(x => x.QuestionId == quesn2.ChildQuestionId).ToList();
        //                //leaf2 = new QuestionLeaf();
        //                //leaf2.QuestionId = childn3[0].QuestionId;
        //                //leaf2.QuestionName = childn3[0].QuestionName;
        //                //leaf2.QuestionTypeName = childn3[0].QuestionTypeName;
        //                //processedQuestion.Add(childn3[0].QuestionId);

        //                //leafleafn2.Add(quesn2.QuestionValue, leaf2);
        //                //leaf1.QuestionLeaves.Add(leafleafn2);
        //            }
        //            else {
        //                leafleafn2.Add(quesn2.QuestionValue, leaf2);
        //                leaf.QuestionLeaves.Add(leafleafn2);
        //            }
        //        }

        //        return leaf;
        //    }
        //}

        public async Task<QuestionRequestDto> GetQuestionByIdAsync(int questionId) {
            var quesReqDto = new QuestionRequestDto();
            var ques = _context.Questions.Where(x=> x.Id == questionId).First();

            if (ques != null) {
                quesReqDto.QuestionId = ques.Id;
                quesReqDto.QuestionTypeId = ques.QuestionTypeId;
                quesReqDto.QuestionName = ques.QuestionName;
                quesReqDto.DisplayOrder = ques.DisplayOrder;
            }

            var questionDto = _context.QuestionDependencys.Where(x => x.QuestionId == questionId).ToList();

            if (questionDto != null && questionDto.Count > 0) {
                quesReqDto.QuestionDependencyList = questionDto;
            }

            return quesReqDto;
        }

        public async Task<bool> UpdateQuestionsAsync(QuestionRequestDto questionRequest) {
            if(questionRequest.QuestionId  > 0) {
                var ques = _context.Questions.Where(x=> x.Id == questionRequest.QuestionId).FirstOrDefault();
                if (ques != null) {
                    ques.QuestionName = questionRequest.QuestionName;
                    ques.DisplayOrder = questionRequest.DisplayOrder;
                    ques.QuestionTypeId = questionRequest.QuestionTypeId;
                }
                
                foreach (var quesDep in questionRequest.QuestionDependencyList) {
                    if (quesDep.Id > 0) {
                        var quesDependency = _context.QuestionDependencys.Where(x => x.Id == quesDep.Id).First();
                        quesDependency.QuestionId = quesDep.QuestionId;
                        quesDependency.QuestionValue = quesDep.QuestionValue;
                        quesDependency.ChildQuestionId = quesDep.ChildQuestionId;
                    }
                    else {
                        var quesDependency = new QuestionDependency();
                        quesDependency.QuestionId = quesDep.QuestionId;
                        quesDependency.QuestionValue = quesDep.QuestionValue;
                        quesDependency.ChildQuestionId = quesDep.ChildQuestionId;
                        _context.QuestionDependencys.Add(quesDependency);
                    }
                }
            }
            else {
                var ques = new Question();
                ques.QuestionName = questionRequest.QuestionName;
                ques.QuestionTypeId = questionRequest.QuestionTypeId;
                ques.DisplayOrder= questionRequest.DisplayOrder;
                _context.Questions.Add(ques);

                foreach (var quesDep in questionRequest.QuestionDependencyList) {
                    var quesDependency = new QuestionDependency();
                    quesDependency.QuestionId = ques.Id;
                    quesDependency.QuestionValue = quesDep.QuestionValue;
                    quesDependency.ChildQuestionId = quesDep.ChildQuestionId;
                    _context.QuestionDependencys.Add(quesDependency);
                }
            }

            _context.SaveChanges();

            return true;
        }

        public async Task<bool> AddDeleteOptionAsync(OptionRequestDto optionRequest) {
            if (optionRequest.OperationId == 1) {
                var quesDependency = new QuestionDependency();
                quesDependency.QuestionId = optionRequest.QuestionId;
                quesDependency.QuestionValue = optionRequest.Optionvalue;
                if(optionRequest.ChildQuestionId > 0)
                    quesDependency.ChildQuestionId = optionRequest.ChildQuestionId;
                _context.QuestionDependencys.Add(quesDependency);
                _context.SaveChanges();

                return true;
            }
            else {
                var quesOption = _context.QuestionDependencys.Where(x => x.QuestionId == optionRequest.QuestionId
                                        && x.QuestionValue.ToLower() == optionRequest.Optionvalue.ToLower()).FirstOrDefault();
                if (quesOption != null) {
                    _context.QuestionDependencys.Remove(quesOption);
                    _context.SaveChanges();
                }

                return true;
            }
        }

        public async Task<bool> AddDeleteQuestionAsync(Question question) {
            if(question.Id > 0) {
                var ques = _context.Questions.Where(x => x.Id == question.Id).FirstOrDefault();
                if (ques != null) {
                    _context.Questions.Remove(ques);
                    _context.SaveChanges();
                }

                return true;
            }
            else {
                var ques = new Question();
                ques.Id = question.Id;
                ques.QuestionTypeId = question.QuestionTypeId;
                ques.QuestionName = question.QuestionName;
                ques.DisplayOrder = question.DisplayOrder;              
                _context.Questions.Add(ques);
                _context.SaveChanges();

                return true;
            }
        }
    }
}
