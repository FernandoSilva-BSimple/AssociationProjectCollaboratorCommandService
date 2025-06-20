using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Domain.Visitor;

namespace Domain.Factory
{
    public class AssociationProjectCollaboratorFactory : IAssociationProjectCollaboratorFactory
    {

        private readonly ICollaboratorRepository _collabRepo;
        private readonly IProjectRepository _projectRepo;

        public AssociationProjectCollaboratorFactory(ICollaboratorRepository collaboratorRepository, IProjectRepository projectRepository)
        {
            _collabRepo = collaboratorRepository;
            _projectRepo = projectRepository;
        }

        public async Task<IAssociationProjectCollaborator> Create(Guid projectId, Guid collaboratorId, DateOnly initDate, DateOnly endDate)
        {
            ICollaborator? collaborator = await _collabRepo.GetByIdAsync(collaboratorId);
            IProject? project = await _projectRepo.GetByIdAsync(projectId);

            if (collaborator == null) throw new ArgumentException("Collaborator cannot be null.");
            if (project == null) throw new ArgumentException("Project cannot be null.");

            PeriodDate periodDate = new PeriodDate(initDate, endDate);

            return new AssociationProjectCollaborator(projectId, collaboratorId, periodDate);
        }

        public IAssociationProjectCollaborator Create(Guid id, Guid projectId, Guid collaboratorId, PeriodDate periodDate)
        {
            return new AssociationProjectCollaborator(id, projectId, collaboratorId, periodDate);
        }

        public AssociationProjectCollaborator Create(IAssociationProjectCollaboratorVisitor visitor)
        {
            return new AssociationProjectCollaborator(visitor.Id, visitor.ProjectId, visitor.CollaboratorId, visitor.PeriodDate);
        }
    }
}
