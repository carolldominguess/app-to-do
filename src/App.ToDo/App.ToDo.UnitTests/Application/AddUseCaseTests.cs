using App.ToDo.Application.UseCases;
using App.ToDo.Domain.Entities;
using App.ToDo.Domain.Enums;
using App.ToDo.Domain.Interfaces;
using App.ToDo.Domain.Interfaces.Repositories;
using App.ToDo.UnitTests.Builders;
using FluentAssertions;
using Moq;

namespace App.ToDo.UnitTests.Application;

public class AddUseCaseTests
{
    private readonly Mock<IToDoTaskRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ILogRepository> _logRepositoryMock;
    private readonly AddUseCase _useCase;

    public AddUseCaseTests()
    {
        _repositoryMock = new Mock<IToDoTaskRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _logRepositoryMock = new Mock<ILogRepository>();
        _useCase = new AddUseCase(_repositoryMock.Object, _unitOfWorkMock.Object, _logRepositoryMock.Object);
    }

    [Fact]
    public void ProcessRequest_WithValidRequest_ShouldAddAndCommitAndLogSuccess()
    {
        var request = AddUcRequestBuilder.New().Build();

        _useCase.ProcessRequest(request);

        request.IsValid.Should().BeTrue();
        _repositoryMock.Verify(r => r.Add(It.IsAny<ToDoTask>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
        _logRepositoryMock.Verify(
            l => l.Save(It.Is<Log>(log => log.Status == LogStatus.Success)),
            Times.Once);
    }

    [Fact]
    public void ProcessRequest_WithEmptyTitle_ShouldNotAddAndLogError()
    {
        var request = AddUcRequestBuilder.New().WithTitle(string.Empty).Build();

        _useCase.ProcessRequest(request);

        request.IsValid.Should().BeFalse();
        request.Errors.Should().NotBeEmpty();
        _repositoryMock.Verify(r => r.Add(It.IsAny<ToDoTask>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.Commit(), Times.Never);
        _logRepositoryMock.Verify(
            l => l.Save(It.Is<Log>(log => log.Status == LogStatus.Error)),
            Times.Once);
    }

    [Fact]
    public void ProcessRequest_WithEmptyDescription_ShouldNotAddAndLogError()
    {
        var request = AddUcRequestBuilder.New().WithDescription(string.Empty).Build();

        _useCase.ProcessRequest(request);

        request.IsValid.Should().BeFalse();
        request.Errors.Should().Contain(e => e.Contains("descrição"));
        _repositoryMock.Verify(r => r.Add(It.IsAny<ToDoTask>()), Times.Never);
        _logRepositoryMock.Verify(
            l => l.Save(It.Is<Log>(log => log.Status == LogStatus.Error)),
            Times.Once);
    }
}
