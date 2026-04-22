using App.ToDo.Application.Requests;
using App.ToDo.Application.UseCases;
using App.ToDo.Domain.Entities;
using App.ToDo.Domain.Interfaces;
using App.ToDo.Domain.Interfaces.Repositories;
using App.ToDo.UnitTests.Builders;
using FluentAssertions;
using Moq;

namespace App.ToDo.UnitTests.Application;

public class RemoveUseCaseTests
{
    private readonly Mock<IToDoTaskRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly RemoveUseCase _useCase;

    public RemoveUseCaseTests()
    {
        _repositoryMock = new Mock<IToDoTaskRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _useCase = new RemoveUseCase(_repositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public void ProcessRequest_WhenEntityExists_ShouldRemoveAndCommit()
    {
        var existing = ToDoTaskBuilder.New().Build();
        var request = new RemoveUcRequest { Id = existing.Id };

        _repositoryMock.Setup(r => r.GetById(existing.Id)).Returns(existing);

        _useCase.ProcessRequest(request);

        request.IsValid.Should().BeTrue();
        _repositoryMock.Verify(r => r.Remove(It.IsAny<ToDoTask>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
    }

    [Fact]
    public void ProcessRequest_WhenEntityDoesNotExist_ShouldAddError()
    {
        var request = new RemoveUcRequest { Id = Guid.NewGuid() };

        _repositoryMock.Setup(r => r.GetById(It.IsAny<Guid>())).Returns((ToDoTask?)null);

        _useCase.ProcessRequest(request);

        request.IsValid.Should().BeFalse();
        request.Errors.Should().ContainSingle(e => e.Contains("não encontrada"));
        _unitOfWorkMock.Verify(u => u.Commit(), Times.Never);
    }
}
