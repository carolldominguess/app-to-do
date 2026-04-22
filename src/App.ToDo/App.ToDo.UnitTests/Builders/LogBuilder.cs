using App.ToDo.Domain.Entities;
using App.ToDo.Domain.Enums;

namespace App.ToDo.UnitTests.Builders;

public class LogBuilder
{
    private string _source = "TestUseCase";
    private LogStatus _status = LogStatus.Success;
    private string? _message = "Operação realizada com sucesso.";

    public static LogBuilder New() => new();

    public LogBuilder WithSource(string source) { _source = source; return this; }
    public LogBuilder WithStatus(LogStatus status) { _status = status; return this; }
    public LogBuilder WithMessage(string? message) { _message = message; return this; }
    public LogBuilder AsSuccess() { _status = LogStatus.Success; return this; }
    public LogBuilder AsError() { _status = LogStatus.Error; return this; }

    public Log Build() => new(_source, _status, _message);
}
