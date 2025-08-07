namespace Aviationexam.Api.TpsSample.Models;

public class ExamSetOutput
{
    public required int Id { get; set; }

    public required int IdUser { get; set; }

    public required int IdCaa { get; set; }

    public required int IdQuestionBank { get; set; }

    public int? IdLevel { get; set; }

    public required ExamSetStatus Status { get; set; }

    public DateTime? DateStart { get; set; }

    public DateTime? DateEnd { get; set; }

    public string Note { get; set; } = null!;

    public required bool ProctoredExams { get; set; }

    public required IReadOnlyCollection<ExamSetSubjectOutput> SubjectExams { get; set; } = null!;
}
