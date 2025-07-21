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
        private readonly IAssociationProjectCollaboratorRepository _assocRepo;

        public AssociationProjectCollaboratorFactory(ICollaboratorRepository collaboratorRepository, IProjectRepository projectRepository, IAssociationProjectCollaboratorRepository associationProjectCollaboratorRepository)
        {
            _collabRepo = collaboratorRepository;
            _projectRepo = projectRepository;
            _assocRepo = associationProjectCollaboratorRepository;
        }

        public async Task<IAssociationProjectCollaborator> Create(Guid projectId, Guid collaboratorId, DateOnly initDate, DateOnly endDate)
        {
            PeriodDate assocPeriodDate = new PeriodDate(initDate, endDate);

            var collaborator = await _collabRepo.GetByIdAsync(collaboratorId);
            if (collaborator == null) throw new ArgumentException("Collaborator cannot be null.");

            var collaboratorStart = DateOnly.FromDateTime(collaborator.PeriodDateTime._initDate);
            var collaboratorEnd = DateOnly.FromDateTime(collaborator.PeriodDateTime._finalDate);

            if (initDate < collaboratorStart || endDate > collaboratorEnd) throw new ArgumentException("Association period must be within the collaborator's active period.");

            var project = await _projectRepo.GetByIdAsync(projectId);
            if (project == null) throw new ArgumentException("Project cannot be null.");

            var projectStart = project.PeriodDate.InitDate;
            var projectEnd = project.PeriodDate.FinalDate;

            if (initDate < projectStart || endDate > projectEnd) throw new ArgumentException("Association period must be within the project's active period.");

            var associationHasOverlap = await _assocRepo.ExistsWithCollaboratorAndProjectAndOverlappingPeriodAsync(collaboratorId, projectId, assocPeriodDate);
            if (associationHasOverlap) throw new ArgumentException("Association period for this collaborator and project must not overlap with another association.");


            return new AssociationProjectCollaborator(projectId, collaboratorId, assocPeriodDate);
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
