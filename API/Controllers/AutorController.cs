using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Errors;
using AutoMapper;
using Core.DTOs.Autors;
using Core.Entities;
using Core.Helpers;
using Core.Interfaces;
using Core.Interfaces.Services.Autores;
using Core.Specification.Autors;
using Core.Specification.Autors.SpecParams;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
public class AutorController : BaseApiController
{
    private readonly IGenericRepository<Autor> _genericAutor;
    private readonly IAutorService _serviceAutor;
    private readonly IMapper _mapper;
    private readonly IValidator<AutorCriarDto> _validatorAutorCriarDto;
    private readonly IValidator<AutorAtualizarDto> _validatorAutorAtualizarDto;

    public AutorController(IGenericRepository<Autor> genericAutor, IAutorService serviceAutor, IMapper mapper, IValidator<AutorCriarDto> validatorAutorCriarDto, IValidator<AutorAtualizarDto> validatorAutorAtualizarDto)
    {
        _genericAutor = genericAutor;
        _serviceAutor = serviceAutor;
        _mapper = mapper;
        _validatorAutorCriarDto = validatorAutorCriarDto;
        _validatorAutorAtualizarDto = validatorAutorAtualizarDto;
    }

    [HttpGet("all")]
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PaginationWithReadOnyList<AutorReturnDto>>> GetAutors(
        [FromQuery] AutorSpecParams paramsQuery)
    {
        var spec = new AutorObterTodosAutoresByFiltroSpecification(paramsQuery);
        var countSpec = new AutorTotalCadastradosByFiltroSpecification(paramsQuery);
        var totalItems = await _genericAutor.CountAsync(countSpec);

        var Autors = await _genericAutor.ListReadOnlyListAsync(spec);

        var data = _mapper.Map<IReadOnlyList<AutorReturnDto>>(Autors);

        return Ok(new PaginationWithReadOnyList<AutorReturnDto>(paramsQuery.PageIndex,
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
    public async Task<ActionResult<AutorReturnDto>> GetDetalhesPorId(int id)
    {
        var spec = new AutorObterTodosAutoresByFiltroSpecification(new AutorSpecParams { CodAu = id });
        var result = await _genericAutor.GetEntityWithSpec(spec);

        return Ok(_mapper.Map<AutorReturnDto>(result));
    }

    [HttpPost]
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AutorReturnDto>> PostCadastrarAutor(AutorCriarDto dto)
    {
        var AutorValidator = _validatorAutorCriarDto.Validate(dto);

        if (!AutorValidator.IsValid)
            return BadRequest(new ApiResponse(400, AutorValidator.Errors.FirstOrDefault().ErrorMessage));

        var Autor = _mapper.Map<Autor>(dto);
        var result = await _serviceAutor.CreateAutorAsync(Autor);

        if (result.Error != null) return BadRequest(new ApiResponse(400, result.Error.Message));
        var resultDto = _mapper.Map<AutorReturnDto>(Autor);

        return CreatedAtAction(nameof(GetDetalhesPorId), new { id = resultDto.CodAu }, resultDto);
    }

    [HttpPut("editar/{id:int}")]
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AutorReturnDto>> PutAtualizarAutor(int id, AutorAtualizarDto dto)
    {
        var AutorValidator = _validatorAutorAtualizarDto.Validate(dto);

        if (!AutorValidator.IsValid)
            return BadRequest(new ApiResponse(400, AutorValidator.Errors.FirstOrDefault().ErrorMessage));

        var Autor = _mapper.Map<Autor>(dto);
        var result = await _serviceAutor.UpdateAutorAsync(id, Autor);

        if (result.Error != null) return BadRequest(new ApiResponse(400, result.Error.Message));
        return NoContent();

    }
}
