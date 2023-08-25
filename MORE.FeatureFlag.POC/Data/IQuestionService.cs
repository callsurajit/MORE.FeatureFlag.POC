using MORE.FeatureFlag.POC.Models;

namespace MORE.FeatureFlag.POC.Data {
    public interface IQuestionService {
        Task<List<QuestionDto>> GetQuestionsAsync();
        Task<List<QuestionLeaf>> GetDecisionTreeAsync();
        Task<bool> UpdateQuestionsAsync(QuestionRequestDto questionRequest);
        Task<QuestionRequestDto> GetQuestionByIdAsync(int questionId);
        Task<bool> AddDeleteOptionAsync(OptionRequestDto optionRequest);
        Task<bool> AddDeleteQuestionAsync(Question question);
        

    }
}
