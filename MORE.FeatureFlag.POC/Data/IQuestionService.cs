using MORE.FeatureFlag.POC.Models;

namespace MORE.FeatureFlag.POC.Data {
    public interface IQuestionService {
        Task<List<QuestionDto>> GetQuestionsAsync();
        Task<QuestionTreeDto> GetDecisionTreeAsync();
        Task<bool> UpdateQuestionsAsync(QuestionRequestDto questionRequest);
    }
}
