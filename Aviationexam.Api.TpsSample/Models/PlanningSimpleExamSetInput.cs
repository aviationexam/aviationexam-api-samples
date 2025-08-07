namespace Aviationexam.Api.TpsSample.Models;

public class PlanningSimpleExamSetInput
{
    public required int IdUser { get; set; }

    public required int IdQuestionBank { get; set; }

    public int? IdExamLevel { get; set; }

    public DateTime? DateStarted { get; set; }

    public bool? ProctoredExams { get; set; }
}
