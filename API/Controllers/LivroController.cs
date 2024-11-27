using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Errors;
using AutoMapper;
using Core.DTOs.Livros;
using Core.Entities;
using Core.Helpers;
using Core.Interfaces;
using Core.Interfaces.Services.Livros;
using Core.Specification.Livros;
using Core.Specification.Livros.SpecParams;
using DinkToPdf;
using DinkToPdf.Contracts;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
public class LivroController : BaseApiController
{
    private readonly IGenericRepository<Livro> _genericLivro;
    private readonly ILivroService _serviceLivro;
    private readonly IMapper _mapper;
    private readonly IValidator<LivroCriarDto> _validatorLivroCriarDto;
    private readonly IValidator<LivroAtualizarDto> _validatorLivroAtualizarDto;
    private readonly IConverter _converter;

    public LivroController(IGenericRepository<Livro> genericLivro, ILivroService serviceLivro, IMapper mapper, IValidator<LivroCriarDto> validatorLivroCriarDto, IValidator<LivroAtualizarDto> validatorLivroAtualizarDto, IConverter converter)
    {
        _genericLivro = genericLivro;
        _serviceLivro = serviceLivro;
        _mapper = mapper;
        _validatorLivroCriarDto = validatorLivroCriarDto;
        _validatorLivroAtualizarDto = validatorLivroAtualizarDto;
        _converter = converter;
    }

    [HttpGet("all")]
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PaginationWithReadOnyList<LivroReturnDto>>> Getlivros(
        [FromQuery] LivroSpecParams paramsQuery)
    {
        var spec = new LivroObterTodosLivrosByFiltroSpecification(paramsQuery);
        var countSpec = new LivroTotalCadastradosByFiltroSpecification(paramsQuery);
        var totalItems = await _genericLivro.CountAsync(countSpec);

        var livros = await _genericLivro.ListReadOnlyListAsync(spec);

        var data = _mapper.Map<IReadOnlyList<LivroReturnDto>>(livros);

        return Ok(new PaginationWithReadOnyList<LivroReturnDto>(paramsQuery.PageIndex,
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
    public async Task<ActionResult<LivroReturnDto>> GetDetalhesPorId(int id)
    {
        var spec = new LivroObterTodosLivrosByFiltroSpecification(new LivroSpecParams { CodI = id });
        var result = await _genericLivro.GetEntityWithSpec(spec);

        return Ok(_mapper.Map<LivroReturnDto>(result));
    }

    [HttpPost]
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<LivroReturnDto>> PostCadastrarLivro(LivroCriarDto dto)
    {
        var livroValidator = _validatorLivroCriarDto.Validate(dto);

        if (!livroValidator.IsValid)
            return BadRequest(new ApiResponse(400, livroValidator.Errors.FirstOrDefault().ErrorMessage));

        var livro = _mapper.Map<Livro>(dto);
        var result = await _serviceLivro.CreateLivroAsync(livro);

        if (result.Error != null) return BadRequest(new ApiResponse(400, result.Error.Message));
        var resultDto = _mapper.Map<LivroReturnDto>(livro);

        return CreatedAtAction(nameof(GetDetalhesPorId), new { id = resultDto.CodI }, resultDto);
    }

    [HttpPut("editar/{id:int}")]
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<LivroReturnDto>> PutAtualizarLivro(int id, LivroAtualizarDto dto)
    {
        var livroValidator = _validatorLivroAtualizarDto.Validate(dto);

        if (!livroValidator.IsValid)
            return BadRequest(new ApiResponse(400, livroValidator.Errors.FirstOrDefault().ErrorMessage));

        var livro = _mapper.Map<Livro>(dto);
        var result = await _serviceLivro.UpdateLivroAsync(id, livro);

        if (result.Error != null) return BadRequest(new ApiResponse(400, result.Error.Message));
        return NoContent();

    }

    [HttpGet("gerar-pdf"), DisableRequestSizeLimit]
    public async Task<IActionResult> Download()
    {
        var spec = new LivroObterTodosLivrosByFiltroSpecification(new LivroSpecParams());
        var livros = await _genericLivro.ListReadOnlyListAsync(spec);

        var data = _mapper.Map<IReadOnlyList<LivroReturnDto>>(livros);

        var html = ConvertUserListToHtmlTable(data);

        var doc = new HtmlToPdfDocument()
        {
            GlobalSettings = {
                    PaperSize = PaperKind.A4,
                    Orientation = Orientation.Portrait
                },
            Objects = {
                    new ObjectSettings()
                    {
                        HtmlContent = html
                    }
                }
        };
        var file = new FileDto("LivrosReport.pdf", _converter.Convert(doc));
        return File(file.FileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", file.FileName);

    }
    private string ConvertUserListToHtmlTable(IReadOnlyList<LivroReturnDto> livros)
    {
        var header1 = "<th>Código</th>";
        var header2 = "<th>Título</th>";
        var header3 = "<th>Editora</th>";
        var header4 = "<th>Edição</th>";
        var header5 = "<th>Ano Publicação</th>";
        var headers = $"<tr>{header1}{header2}{header3}{header4}{header5}</tr>";
        var rows = new StringBuilder();
        foreach (var livro in livros)
        {
            var column1 = $"<td>{livro.CodI}</td>";
            var column2 = $"<td>{livro.Titulo}</td>";
            var column3 = $"<td>{livro.Editora}</td>";
            var column4 = $"<td>{livro.Edicao}</td>";
            var column5 = $"<td>{livro.AnoPublicacao}</td>";
            var row = $"<tr>{column1}{column2}{column3}{column4}{column5}</tr>";
            rows.Append(row);
        }
        return $"<table>{headers}{rows.ToString()}</table>";
    }
}

public class FileDto
{
    public string FileName { get; set; }
    public byte[] FileBytes { get; set; }
    public FileDto(string fileName, byte[] fileBytes)
    {
        FileName = fileName;
        FileBytes = fileBytes;
    }
}
