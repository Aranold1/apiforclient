using AutoMapper;
using System;
using WebApiForGptBlazor.Mapper;
using WebApiForGptBlazor.Models;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Message, MessageWithoutChat>();
        CreateMap<Chat,ChatWithoutMessages>();
    }
}
