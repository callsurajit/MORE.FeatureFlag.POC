using Microsoft.AspNetCore.Mvc;
using MORE.FeatureFlag.POC.Data;
using MORE.FeatureFlag.POC.Models;

namespace MORE.FeatureFlag.POC.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class QuestionnaireController : ControllerBase {
        private readonly ILogger<QuestionnaireController> _logger;
        private readonly IQuestionService _questionService;
        private readonly IConfiguration _config;

        public QuestionnaireController(IQuestionService questionService, IConfiguration config, ILogger<QuestionnaireController> logger) {
            _questionService = questionService;
            _config = config;
            _logger = logger;
        }

        [HttpGet("getquestions")]
        public async Task<IActionResult> GetQuestions() {
            return Ok(await _questionService.GetQuestionsAsync());
        }

        [HttpGet("getdecisiontree")]
        public async Task<IActionResult> GetDecisionTree() {
            return Ok(await _questionService.GetDecisionTreeAsync());
        }

        [HttpGet("getquestionbyId/{questionId}")]
        public async Task<IActionResult> GetQuestionById(int questionId) {
            return Ok(await _questionService.GetQuestionByIdAsync(questionId));
        }

        [HttpPost("updatequestions")]
        public async Task<bool> UpdateQuestions(QuestionRequestDto questionRequest) {
            return await _questionService.UpdateQuestionsAsync(questionRequest);
        }

        [HttpPost("adddeleteoption")]
        public async Task<bool> AddDeleteOption(OptionRequestDto optionRequest) {
            return await _questionService.AddDeleteOptionAsync(optionRequest);
        }

        [HttpPost("adddeletequestion")]
        public async Task<bool> AddDeleteQuestion(Question question) {
            return await _questionService.AddDeleteQuestionAsync(question);
        }
    }
}
