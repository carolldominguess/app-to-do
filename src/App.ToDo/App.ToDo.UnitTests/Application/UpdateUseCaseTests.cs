using App.ToDo.Application.UseCases;
using App.ToDo.Domain.Entities;
using App.ToDo.Domain.Interfaces;
using App.ToDo.Domain.Interfaces.Repositories;
using App.ToDo.UnitTests.Builders;
using FluentAssertions;
using Moq;

namespace App.ToDo.UnitTests.Application;

public class UpdateUseCaseTests
{
    private readonly Mock<IToDoTaskRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly UpdateUseCase _useCase;

    public UpdateUseCaseTests()
    {
        _repositoryMock = new Mock<IToDoTaskRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _useCase = new UpdateUseCase(_repositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public void ProcessRequest_WhenEntityExists_ShouldUpdateAndCommit()
    {
        var existing = ToDoTaskBuilder.New().Build();
        var request = UpdateUcRequestBuilder.New().WithId(existing.Id).Build();

        _repositoryMock.Setup(r => r.GetById(existing.Id)).Returns(existing);

        _useCase.ProcessRequest(request);

        request.IsValid.Should().BeTrue();
        _repositoryMock.Verify(r => r.Update(It.IsAny<ToDoTask>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
    }

    [Fact]
    public void ProcessRequest_WhenEntityDoesNotExist_ShouldAddError()
    {
        var request = UpdateUcRequestBuilder.New().Build();

        _repositoryMock.Setup(r => r.GetById(It.IsAny<Guid>())).Returns((ToDoTask?)null);

        _useCase.ProcessRequest(request);

        request.IsValid.Should().BeFalse();
        request.Errors.Should().ContainSingle(e => e.Contains("não encontrada"));
        _unitOfWorkMock.Verify(u => u.Commit(), Times.Never);
    }

    [Fact]
    public void ProcessRequest_WithEmptyTitle_ShouldNotUpdateAndShouldHaveErrors()
    {
        var request = UpdateUcRequestBuilder.New().WithTitle(string.Empty).Build();

        _useCase.ProcessRequest(request);

        request.IsValid.Should().BeFalse();
        _repositoryMock.Verify(r => r.Update(It.IsAny<ToDoTask>()), Times.Never);
    }
}
