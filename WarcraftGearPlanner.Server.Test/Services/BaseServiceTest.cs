using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Caching.Memory;
using System.Linq.Expressions;
using WarcraftGearPlanner.Server.Data;
using WarcraftGearPlanner.Server.Data.Entities;
using WarcraftGearPlanner.Server.Services;
using WarcraftGearPlanner.Shared.Models;
using WarcraftGearPlanner.Tests;

namespace WarcraftGearPlanner.Server.Tests.Services;

public class TestModel : BaseModel { }

public class TestEntity : BaseEntity { }

public class TestService(
	IRepository<TestEntity> repository,
	IValidator<TestModel> validator,
	IMemoryCache memoryCache,
	IMapper mapper
) : BaseService<TestModel, TestEntity>(repository, validator, memoryCache, mapper)
{ }

[TestClass]
public class BaseServiceTest : BaseTest<TestService>
{
	private static readonly Guid _id = Guid.NewGuid();
	private readonly TestModel _model = new() { Id = _id };
	private readonly TestEntity _entity = new() { Id = _id };

	protected override void Setup()
	{
		base.Setup();

		Mocker.GetMock<IRepository<TestEntity>>()
			.Setup(x => x.GetByIdAsync(It.Is<Guid>(g => g == _id)))
			.ReturnsAsync(_entity)
			.Verifiable();

		Mocker.GetMock<IRepository<TestEntity>>()
			.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<TestEntity, bool>>>()))
			.ReturnsAsync([_entity])
			.Verifiable();

		Mocker.GetMock<IRepository<TestEntity>>()
			.Setup(x => x.CreateAsync(It.Is<TestEntity>(e => e.Id == _id)))
			.ReturnsAsync(_entity)
			.Verifiable();

		Mocker.GetMock<IRepository<TestEntity>>()
			.Setup(x => x.CreateListAsync(It.IsAny<List<TestEntity>>()))
			.ReturnsAsync([_entity])
			.Verifiable();

		Mocker.GetMock<IRepository<TestEntity>>()
			.Setup(x => x.UpdateAsync(It.Is<TestEntity>(e => e.Id == _id)))
			.ReturnsAsync(_entity)
			.Verifiable();

		Mocker.GetMock<IRepository<TestEntity>>()
			.Setup(x => x.UpdateListAsync(It.IsAny<List<TestEntity>>()))
			.ReturnsAsync([_entity])
			.Verifiable();


		Mocker.GetMock<IRepository<TestEntity>>()
			.Setup(x => x.DeleteAsync(It.Is<TestEntity>(e => e.Id == _id)))
			.Verifiable();

		Mocker.GetMock<IMapper>()
			.Setup(x => x.Map<TestEntity>(It.Is<TestModel>(m => m.Id == _id)))
			.Returns(_entity)
			.Verifiable();

		Mocker.GetMock<IMapper>()
			.Setup(x => x.Map<List<TestEntity>>(It.IsAny<List<TestModel>>()))
			.Returns([_entity])
			.Verifiable();

		Mocker.GetMock<IMapper>()
			.Setup(x => x.Map<TestModel>(It.Is<TestEntity>(e => e.Id == _id)))
			.Returns(_model)
			.Verifiable();

		Mocker.GetMock<IMapper>()
			.Setup(x => x.Map(It.Is<TestModel>(m => m.Id == _id), It.Is<TestEntity>(e => e.Id == _id)))
			.Returns(_entity)
			.Verifiable();

		Mocker.GetMock<IMapper>()
			.Setup(x => x.Map<List<TestModel>>(It.IsAny<List<TestEntity>>()))
			.Returns([_model])
			.Verifiable();

		Mocker.GetMock<IMemoryCache>()
			.Setup(x => x.Remove(It.Is<string>(s => s == "TestEntity")))
			.Verifiable();

		Mocker.GetMock<IMemoryCache>()
			.Setup(x => x.CreateEntry(It.Is<string>(s => s == "TestEntity")))
			.Returns(new Mock<ICacheEntry>().Object)
			.Verifiable();
	}

	[TestMethod]
	public async Task CreateAsync_WhenEntityExists_ThrowsException()
	{
		await Assert.ThrowsExceptionAsync<Exception>(() => Instance.CreateAsync(_model));

		Mocker.GetMock<IRepository<TestEntity>>().Verify(s => s.GetByIdAsync(It.Is<Guid>(g => g == _id)), Times.Once);
		Mocker.GetMock<IRepository<TestEntity>>().Verify(s => s.CreateAsync(It.IsAny<TestEntity>()), Times.Never);
	}

	[TestMethod]
	public async Task CreateAsync_WhenEntityDoesNotExist_CreatesEntity()
	{
		Mocker.GetMock<IRepository<TestEntity>>()
			.Setup(x => x.GetByIdAsync(It.Is<Guid>(g => g == _id)))
			.ReturnsAsync((TestEntity?)null)
			.Verifiable();

		var result = await Instance.CreateAsync(_model);

		Mocker.GetMock<IRepository<TestEntity>>().Verify(s => s.GetByIdAsync(It.Is<Guid>(g => g == _id)), Times.Once);
		Mocker.GetMock<IRepository<TestEntity>>().Verify(s => s.CreateAsync(It.IsAny<TestEntity>()), Times.Once);
		Mocker.GetMock<IMapper>().Verify(s => s.Map<TestEntity>(It.Is<TestModel>(m => m.Id == _id)), Times.Once);
		Mocker.GetMock<IMapper>().Verify(s => s.Map<TestModel>(It.Is<TestEntity>(e => e.Id == _id)), Times.Once);
		Mocker.GetMock<IMemoryCache>().Verify(s => s.Remove(It.Is<string>(str => str == "TestEntity")), Times.Once);

		Assert.AreEqual(_model, result);
	}

	[TestMethod]
	public async Task CreateListAsync_WhenEntityExists_ThrowsException()
	{
		await Assert.ThrowsExceptionAsync<Exception>(() => Instance.CreateListAsync([_model]));

		Mocker.GetMock<IRepository<TestEntity>>().Verify(s => s.GetByIdAsync(It.Is<Guid>(g => g == _id)), Times.Once);
		Mocker.GetMock<IRepository<TestEntity>>().Verify(s => s.CreateListAsync(It.IsAny<List<TestEntity>>()), Times.Never);
	}

	[TestMethod]
	public async Task CreateListAsync_WhenEntityDoesNotExist_CreatesEntity()
	{
		Mocker.GetMock<IRepository<TestEntity>>()
			.Setup(x => x.GetByIdAsync(It.Is<Guid>(g => g == _id)))
			.ReturnsAsync((TestEntity?)null)
			.Verifiable();

		var result = await Instance.CreateListAsync([_model]);

		Mocker.GetMock<IRepository<TestEntity>>().Verify(s => s.GetByIdAsync(It.Is<Guid>(g => g == _id)), Times.Once);
		Mocker.GetMock<IRepository<TestEntity>>().Verify(s => s.CreateListAsync(It.IsAny<List<TestEntity>>()), Times.Once);
		Mocker.GetMock<IMapper>().Verify(s => s.Map<List<TestEntity>>(It.IsAny<List<TestModel>>()), Times.Once);
		Mocker.GetMock<IMapper>().Verify(s => s.Map<List<TestModel>>(It.IsAny<List<TestEntity>>()), Times.Once);
		Mocker.GetMock<IMemoryCache>().Verify(s => s.Remove(It.Is<string>(str => str == "TestEntity")), Times.Once);

		Assert.AreEqual(1, result.Count);
		Assert.AreEqual(_model, result[0]);
	}

	[TestMethod]
	public async Task DeleteAsync_WhenEntityExists_DeletesEntity()
	{
		await Instance.DeleteAsync(_id);

		Mocker.GetMock<IRepository<TestEntity>>().Verify(s => s.GetByIdAsync(It.Is<Guid>(g => g == _id)), Times.Once);
		Mocker.GetMock<IRepository<TestEntity>>().Verify(s => s.DeleteAsync(It.IsAny<TestEntity>()), Times.Once);
		Mocker.GetMock<IMemoryCache>().Verify(s => s.Remove(It.Is<string>(str => str == "TestEntity")), Times.Once);
	}

	[TestMethod]
	public async Task DeleteAsync_WhenEntityDoesNotExist_DoesNotDeleteEntity()
	{
		Mocker.GetMock<IRepository<TestEntity>>()
			.Setup(x => x.GetByIdAsync(It.Is<Guid>(g => g == _id)))
			.ReturnsAsync((TestEntity?)null)
			.Verifiable();

		await Instance.DeleteAsync(_id);

		Mocker.GetMock<IRepository<TestEntity>>().Verify(s => s.GetByIdAsync(It.Is<Guid>(g => g == _id)), Times.Once);
		Mocker.GetMock<IRepository<TestEntity>>().Verify(s => s.DeleteAsync(It.IsAny<TestEntity>()), Times.Never);
		Mocker.GetMock<IMemoryCache>().Verify(s => s.Remove(It.Is<string>(str => str == "TestEntity")), Times.Never);
	}

	[TestMethod]
	public async Task DeleteListAsync_WhenEntityExists_DeletesEntity()
	{
		await Instance.DeleteListAsync([_id]);

		Mocker.GetMock<IRepository<TestEntity>>().Verify(s => s.GetListAsync(It.IsAny<Expression<Func<TestEntity, bool>>>()), Times.Once);
		Mocker.GetMock<IRepository<TestEntity>>().Verify(s => s.DeleteListAsync(It.IsAny<List<TestEntity>>()), Times.Once);
		Mocker.GetMock<IMemoryCache>().Verify(s => s.Remove(It.Is<string>(str => str == "TestEntity")), Times.Once);
	}

	[TestMethod]
	public async Task DeleteListAsync_WhenEntityDoesNotExist_DoesNotDeleteEntity()
	{
		Mocker.GetMock<IRepository<TestEntity>>()
			.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<TestEntity, bool>>>()))
			.ReturnsAsync([])
			.Verifiable();

		await Instance.DeleteListAsync([_id]);

		Mocker.GetMock<IRepository<TestEntity>>().Verify(s => s.GetListAsync(It.IsAny<Expression<Func<TestEntity, bool>>>()), Times.Once);
		Mocker.GetMock<IRepository<TestEntity>>().Verify(s => s.DeleteListAsync(It.IsAny<List<TestEntity>>()), Times.Never);
		Mocker.GetMock<IMemoryCache>().Verify(s => s.Remove(It.Is<string>(str => str == "TestEntity")), Times.Never);
	}

	[TestMethod]
	public async Task GetByIdAsync_WhenEntityExists_ReturnsEntity()
	{
		var result = await Instance.GetByIdAsync(_id);

		Mocker.GetMock<IRepository<TestEntity>>().Verify(s => s.GetByIdAsync(It.Is<Guid>(g => g == _id)), Times.Once);
		Mocker.GetMock<IMapper>().Verify(s => s.Map<TestModel>(It.Is<TestEntity>(e => e.Id == _id)), Times.Once);

		Assert.AreEqual(_model, result);
	}

	[TestMethod]
	public async Task GetByIdAsync_WhenEntityDoesNotExist_ReturnsNull()
	{
		Mocker.GetMock<IRepository<TestEntity>>()
			.Setup(x => x.GetByIdAsync(It.Is<Guid>(g => g == _id)))
			.ReturnsAsync((TestEntity?)null)
			.Verifiable();

		var result = await Instance.GetByIdAsync(_id);

		Mocker.GetMock<IRepository<TestEntity>>().Verify(s => s.GetByIdAsync(It.Is<Guid>(g => g == _id)), Times.Once);
		Mocker.GetMock<IMapper>().Verify(s => s.Map<TestModel>(It.IsAny<TestEntity>()), Times.Never);

		Assert.IsNull(result);
	}

	[TestMethod]
	public async Task GetListAsync_WhenEntityExists_ReturnsEntity()
	{
		var result = await Instance.GetListAsync();

		Mocker.GetMock<IRepository<TestEntity>>().Verify(s => s.GetListAsync(It.IsAny<Expression<Func<TestEntity, bool>>>()), Times.Once);
		Mocker.GetMock<IMapper>().Verify(s => s.Map<List<TestModel>>(It.IsAny<List<TestEntity>>()), Times.Once);

		Assert.AreEqual(1, result.Count);
		Assert.AreEqual(_model, result[0]);
	}

	[TestMethod]
	public async Task GetListAsync_WhenEntityDoesNotExist_ReturnsEmpty()
	{
		Mocker.GetMock<IRepository<TestEntity>>()
			.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<TestEntity, bool>>>()))
			.ReturnsAsync([])
			.Verifiable();

		Mocker.GetMock<IMapper>()
			.Setup(x => x.Map<List<TestModel>>(It.IsAny<List<TestEntity>>()))
			.Returns([])
			.Verifiable();

		var result = await Instance.GetListAsync();

		Mocker.GetMock<IRepository<TestEntity>>().Verify(s => s.GetListAsync(It.IsAny<Expression<Func<TestEntity, bool>>>()), Times.Once);
		Mocker.GetMock<IMapper>().Verify(s => s.Map<List<TestModel>>(It.IsAny<List<TestEntity>>()), Times.Once);

		Assert.AreEqual(0, result.Count);
	}

	[TestMethod]
	public async Task UpdateAsync_WhenEntityExists_UpdatesEntity()
	{
		var result = await Instance.UpdateAsync(_model);

		Mocker.GetMock<IRepository<TestEntity>>().Verify(s => s.GetByIdAsync(It.Is<Guid>(g => g == _id)), Times.Once);
		Mocker.GetMock<IRepository<TestEntity>>().Verify(s => s.UpdateAsync(It.IsAny<TestEntity>()), Times.Once);
		Mocker.GetMock<IMapper>().Verify(s => s.Map(It.Is<TestModel>(m => m.Id == _id), It.Is<TestEntity>(e => e.Id == _id)), Times.Once);
		Mocker.GetMock<IMapper>().Verify(s => s.Map<TestModel>(It.Is<TestEntity>(e => e.Id == _id)), Times.Once);
		Mocker.GetMock<IMemoryCache>().Verify(s => s.Remove(It.Is<string>(str => str == "TestEntity")), Times.Once);

		Assert.AreEqual(_model, result);
	}

	[TestMethod]
	public async Task UpdateAsync_WhenEntityDoesNotExist_DoesNotUpdateEntity()
	{
		Mocker.GetMock<IRepository<TestEntity>>()
			.Setup(x => x.GetByIdAsync(It.Is<Guid>(g => g == _id)))
			.ReturnsAsync((TestEntity?)null)
			.Verifiable();

		await Assert.ThrowsExceptionAsync<Exception>(() => Instance.UpdateAsync(_model));

		Mocker.GetMock<IRepository<TestEntity>>().Verify(s => s.GetByIdAsync(It.Is<Guid>(g => g == _id)), Times.Once);
		Mocker.GetMock<IRepository<TestEntity>>().Verify(s => s.UpdateAsync(It.IsAny<TestEntity>()), Times.Never);
		Mocker.GetMock<IMapper>().Verify(s => s.Map(It.Is<TestModel>(m => m.Id == _id), It.Is<TestEntity>(e => e.Id == _id)), Times.Never);
		Mocker.GetMock<IMapper>().Verify(s => s.Map<TestModel>(It.IsAny<TestEntity>()), Times.Never);
		Mocker.GetMock<IMemoryCache>().Verify(s => s.Remove(It.Is<string>(str => str == "TestEntity")), Times.Never);
	}

	[TestMethod]
	public async Task UpdateListAsync_WhenEntityExists_UpdatesEntity()
	{
		var result = await Instance.UpdateListAsync([_model]);


		Mocker.GetMock<IRepository<TestEntity>>().Verify(s => s.UpdateListAsync(It.IsAny<List<TestEntity>>()), Times.Once);
		Mocker.GetMock<IMapper>().Verify(s => s.Map(It.Is<TestModel>(m => m.Id == _id), It.Is<TestEntity>(e => e.Id == _id)), Times.Once);
		Mocker.GetMock<IMapper>().Verify(s => s.Map<List<TestModel>>(It.IsAny<List<TestEntity>>()), Times.Once);
		Mocker.GetMock<IMemoryCache>().Verify(s => s.Remove(It.Is<string>(str => str == "TestEntity")), Times.Once);

		Assert.AreEqual(1, result.Count);
		Assert.AreEqual(_model, result[0]);
	}

	[TestMethod]
	public async Task UpdateListAsync_WhenEntityDoesNotExist_DoesNotUpdateEntity()
	{
		Mocker.GetMock<IRepository<TestEntity>>()
			.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<TestEntity, bool>>>()))
			.ReturnsAsync([])
			.Verifiable();

		await Assert.ThrowsExceptionAsync<Exception>(() => Instance.UpdateListAsync([_model]));

		Mocker.GetMock<IRepository<TestEntity>>().Verify(s => s.GetListAsync(It.IsAny<Expression<Func<TestEntity, bool>>>()), Times.Once);
		Mocker.GetMock<IRepository<TestEntity>>().Verify(s => s.UpdateListAsync(It.IsAny<List<TestEntity>>()), Times.Never);
		Mocker.GetMock<IMapper>().Verify(s => s.Map(It.Is<TestModel>(m => m.Id == _id), It.Is<TestEntity>(e => e.Id == _id)), Times.Never);
		Mocker.GetMock<IMapper>().Verify(s => s.Map<List<TestModel>>(It.IsAny<List<TestEntity>>()), Times.Never);
		Mocker.GetMock<IMemoryCache>().Verify(s => s.Remove(It.Is<string>(str => str == "TestEntity")), Times.Never);
	}
}
