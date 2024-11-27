using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Errors;
using AutoMapper;
using Core.DTOs.Assuntos;
using Core.Entities;
using Core.Helpers;
using Core.Interfaces;
using Core.Interfaces.Services.Assuntos;
using Core.Specification.Assuntos;
using Core.Specification.Assuntos.SpecParams;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
public class AssuntoController : BaseApiController
{
    private readonly IGenericRepository<Assunto> _genericAssunto;
    private readonly IAssuntoService _serviceAssunto;
    private readonly IMapper _mapper;
    private readonly IValidator<AssuntoCriarDto> _validatorAssuntoCriarDto;
    private readonly IValidator<AssuntoAtualizarDto> _validatorAssuntoAtualizarDto;

    public AssuntoController(IGenericRepository<Assunto> genericAssunto, 
    IAssuntoService serviceAssunto, IMapper mapper, 
    IValidator<AssuntoCriarDto> validatorAssuntoCriarDto, 
    IValidator<AssuntoAtualizarDto> validatorAssuntoAtualizarDto)
    {
        _genericAssunto = genericAssunto;
        _serviceAssunto = serviceAssunto;
        _mapper = mapper;
        _validatorAssuntoCriarDto = validatorAssuntoCriarDto;
        _validatorAssuntoAtualizarDto = validatorAssuntoAtualizarDto;
    }

    [HttpGet("all")]
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PaginationWithReadOnyList<AssuntoReturnDto>>> GetAssuntos(
        [FromQuery] AssuntoSpecParams paramsQuery)
    {
        var spec = new AssuntoObterTodosAssuntosByFiltroSpecification(paramsQuery);
        var countSpec = new AssuntoTotalCadastradosByFiltroSpecification(paramsQuery);
        var totalItems = await _genericAssunto.CountAsync(countSpec);

        var Assuntos = await _genericAssunto.ListReadOnlyListAsync(spec);

        var data = _mapper.Map<IReadOnlyList<AssuntoReturnDto>>(Assuntos);

        return Ok(new PaginationWithReadOnyList<AssuntoReturnDto>(paramsQuery.PageIndex,
            paramsQuery.PageSize, totalItems, data));
    }

    [HttpGet("details/{id:int}")]
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AssuntoReturnDto>> GetDetalhesPorId(int id)
    {
        var spec = new AssuntoObterTodosAssuntosByFiltroSpecification(new AssuntoSpecParams { CodAs = id });
        var result = await _genericAssunto.GetEntityWithSpec(spec);

        return Ok(_mapper.Map<AssuntoReturnDto>(result));
    }

    [HttpPost]
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AssuntoReturnDto>> PostCadastrarAssunto(AssuntoCriarDto dto)
    {
        var AssuntoValidator = _validatorAssuntoCriarDto.Validate(dto);

        if (!AssuntoValidator.IsValid)
            return BadRequest(new ApiResponse(400, AssuntoValidator.Errors.FirstOrDefault().ErrorMessage));

        var Assunto = _mapper.Map<Assunto>(dto);
        var result = await _serviceAssunto.CreateAssuntoAsync(Assunto);

        if (result.Error != null) return BadRequest(new ApiResponse(400, result.Error.Message));
        var resultDto = _mapper.Map<AssuntoReturnDto>(Assunto);

        return CreatedAtAction(nameof(GetDetalhesPorId), new { id = resultDto.CodAs }, resultDto);
    }

    [HttpPut("editar/{id:int}")]
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AssuntoReturnDto>> PutAtualizarAssunto(int id, AssuntoAtualizarDto dto)
    {
        var AssuntoValidator = _validatorAssuntoAtualizarDto.Validate(dto);

        if (!AssuntoValidator.IsValid)
            return BadRequest(new ApiResponse(400, AssuntoValidator.Errors.FirstOrDefault().ErrorMessage));

        var Assunto = _mapper.Map<Assunto>(dto);
        var result = await _serviceAssunto.UpdateAssuntoAsync(id, Assunto);

        if (result.Error != null) return BadRequest(new ApiResponse(400, result.Error.Message));
        return NoContent();

    }
}
