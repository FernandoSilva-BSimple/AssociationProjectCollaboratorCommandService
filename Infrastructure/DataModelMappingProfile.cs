using AutoMapper;
using Domain.Models;
using Infrastructure.DataModel;
using Infrastructure.Resolvers;

namespace Infrastructure;

public class DataModelMappingProfile : Profile
{
    public DataModelMappingProfile()
    {
        CreateMap<AssociationProjectCollaborator, AssociationProjectCollaboratorDataModel>();
        CreateMap<AssociationProjectCollaboratorDataModel, AssociationProjectCollaborator>()
            .ConvertUsing<AssociationProjectCollaboratorDataModelConverter>();
        CreateMap<Collaborator, CollaboratorDataModel>();
        CreateMap<CollaboratorDataModel, Collaborator>()
            .ConvertUsing<CollaboratorDataModelConverter>();
        CreateMap<Project, ProjectDataModel>();
        CreateMap<ProjectDataModel, Project>()
            .ConvertUsing<ProjectDataModelConverter>();
    }
}