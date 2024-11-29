using AutoMapper;

using Backend.MoneyTransfer.Application.Common.Mappings;
using Backend.MoneyTransfer.Application.Features.Users.Queries;
using Backend.MoneyTransfer.Domain.Entities;

using NUnit.Framework;

using System.Runtime.Serialization;

namespace Backend.MoneyTransfer.UnitTest.Mappings;

public class MappingTests
{
    private readonly IConfigurationProvider _configuration;
    private readonly IMapper _mapper;

    public MappingTests()
    {
        _configuration = new MapperConfiguration(config =>
            config.AddProfile<MappingProfile>());

        _mapper = _configuration.CreateMapper();
    }

    [Test]
    public void Mapper_configuration_should_valid()
    {
        _configuration.AssertConfigurationIsValid();
    }

    [Test]
    [TestCase(typeof(User), typeof(BalanceResponse))]
    public void Mapper_should_map_from_source_to_destination(Type source, Type destination)
    {
        var instance = GetInstanceOf(source);

        _mapper.Map(instance, source, destination);
    }

    private object GetInstanceOf(Type type)
    {
        if (type.GetConstructor(Type.EmptyTypes) != null)
            return Activator.CreateInstance(type)!;

        return FormatterServices.GetUninitializedObject(type);
    }
}