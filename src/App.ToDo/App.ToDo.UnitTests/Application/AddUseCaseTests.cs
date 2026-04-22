using App.ToDo.Application.UseCases;
using App.ToDo.Domain.Entities;
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
    private readonly AddUseCase _useCase;

    public AddUseCaseTests()
    {
        _repositoryMock = new Mock<IToDoTaskRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _useCase = new AddUseCase(_repositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public void ProcessRequest_WithValidRequest_ShouldAddAndCommit()
    {
        var request = AddUcRequestBuilder.New().Build();

        _useCase.ProcessRequest(request);

        request.IsValid.Should().BeTrue();
        _repositoryMock.Verify(r => r.Add(It.IsAny<ToDoTask>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
    }

    [Fact]
    public void ProcessRequest_WithEmptyTitle_ShouldNotAddAndShouldHaveErrors()
    {
        var request = AddUcRequestBuilder.New().WithTitle(string.Empty).Build();

        _useCase.ProcessRequest(request);

        request.IsValid.Should().BeFalse();
        request.Errors.Should().NotBeEmpty();
        _repositoryMock.Verify(r => r.Add(It.IsAny<ToDoTask>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.Commit(), Times.Never);
    }

    [Fact]
    public void ProcessRequest_WithEmptyDescription_ShouldNotAddAndShouldHaveErrors()
    {
        var request = AddUcRequestBuilder.New().WithDescription(string.Empty).Build();

        _useCase.ProcessRequest(request);

        request.IsValid.Should().BeFalse();
        request.Errors.Should().Contain(e => e.Contains("descrição"));
        _repositoryMock.Verify(r => r.Add(It.IsAny<ToDoTask>()), Times.Never);
    }
}
