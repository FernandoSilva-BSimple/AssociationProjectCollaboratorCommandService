using Application.DTO;
using Application.Interfaces;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/associationsPC")]
public class AssociationProjectCollaboratorController : ControllerBase
{
    private readonly IAssociationProjectCollaboratorService _service;

    public AssociationProjectCollaboratorController(IAssociationProjectCollaboratorService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<AssociationProjectCollaboratorDTO>> Create([FromBody] CreateAssociationProjectCollaboratorDTO dto)
    {
        var assoc = await _service.Create(dto);

        return assoc.ToActionResult();
    }
}
