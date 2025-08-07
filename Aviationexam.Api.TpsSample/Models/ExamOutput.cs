namespace Aviationexam.Api.TpsSample.Models;

public class ExamOutput
{
    public required long Id { get; set; }

    public required int IdCaa { get; set; }

    public required int IdUser { get; set; }

    public int? IdLevel { get; set; }

    public required int IdQuestionBank { get; set; }

    public required int IdSubject { get; set; }

    public string? Note { get; set; }

    public short? QuestionsCorrect { get; set; }

    public required short QuestionsCount { get; set; }

    public decimal? Score { get; set; }

    public DateTime? DateStarted { get; set; }

    public DateTime? DateFinished { get; set; }

    public string? UserFeedback { get; set; }

    public required ExamStatus Status { get; set; }
}
