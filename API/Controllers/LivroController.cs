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
using Core.Interfaces.Repositories.Livros;
using Core.Interfaces.Services.Assuntos;
using Core.Interfaces.Services.Autores;
using Core.Interfaces.Services.Livros;
using Core.Specification.Livros;
using Core.Specification.Livros.SpecParams;
using DinkToPdf;
using DinkToPdf.Contracts;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PugPdf.Core;

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
    private readonly IAutorRepository _repoAutor;
    private readonly IAssuntoRepository _repoAssunto;
    private readonly ILivroRepository _repoLivro;

    public LivroController(IGenericRepository<Livro> genericLivro,
        ILivroService serviceLivro, IMapper mapper, IValidator<LivroCriarDto> validatorLivroCriarDto,
        IValidator<LivroAtualizarDto> validatorLivroAtualizarDto,
        IConverter converter,
        IAutorRepository repoAutor,
        IAssuntoRepository repoAssunto, ILivroRepository repoLivro)
    {
        _genericLivro = genericLivro;
        _serviceLivro = serviceLivro;
        _mapper = mapper;
        _validatorLivroCriarDto = validatorLivroCriarDto;
        _validatorLivroAtualizarDto = validatorLivroAtualizarDto;
        _converter = converter;
        _repoAutor=repoAutor;
        _repoAssunto=repoAssunto;
        _repoLivro=repoLivro;
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
        var spec = new LivroObterTodosLivrosByFiltroSpecification(new LivroSpecParams { CodL = id, IncluirAssuntos = true, IncluirAutores = true });
        var result = await _genericLivro.GetEntityWithSpec(spec);

        if (result.Livro_AutorList.Any())
        {
            var idsAutores = result.Livro_AutorList.DistinctBy(x => x.Autor_CodAu).ToList();

            var listAutores = await _repoAutor.GetListAllAutoresByIds(idsAutores);
            foreach (var idAutor in idsAutores)
            {
                var autor = result.Livro_AutorList.FirstOrDefault(x => x.Autor_CodAu == idAutor.Autor_CodAu);
                autor.Autor = listAutores.FirstOrDefault(x => x.CodAu == idAutor.Autor_CodAu);
            }
        }

        if (result.Livro_AssuntoList.Any())
        {
            var idsAutores = result.Livro_AssuntoList.DistinctBy(x => x.Assunto_CodAs).ToList();

            var listAutores = await _repoAssunto.GetListAllAssuntosByIds(idsAutores);
            foreach (var idAutor in idsAutores)
            {
                var autor = result.Livro_AssuntoList.FirstOrDefault(x => x.Assunto_CodAs == idAutor.Assunto_CodAs);
                autor.Assunto = listAutores.FirstOrDefault(x => x.CodAs == idAutor.Assunto_CodAs);
            }
        }
        var resultMapper = _mapper.Map<LivroReturnDto>(result);

        return Ok(resultMapper);
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

        return CreatedAtAction(nameof(GetDetalhesPorId), new { id = resultDto.CodL }, resultDto);
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

    [HttpDelete("remover/{id:int}")]
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<LivroReturnDto>> DeleteLivroPorBy(int id)
    {
        var result = await _serviceLivro.ExcludeLivroAsync(id);

        if (result.Error != null) return BadRequest(new ApiResponse(400, result.Error.Message));
        return NoContent();

    }


    [HttpGet("gerar-pdf"), DisableRequestSizeLimit]
    public async Task<IActionResult> Download()
    {
        //var spec = new LivroObterTodosLivrosByFiltroSpecification(new LivroSpecParams());
        var livros = await _repoLivro.GetTodosLivrosByView();

        var data = _mapper.Map<IReadOnlyList<LivroRelatorio>>(livros);

        var html = ConvertUserListToHtmlTable(data);

        var renderer = new HtmlToPdf();

        renderer.PrintOptions.Title = "My title";

        var pdf = await renderer.RenderHtmlAsPdfAsync(html);
        var byteArray = pdf.BinaryData;

        return File(byteArray, "application/pdf", "My title");

    }
    private string ConvertUserListToHtmlTable(IReadOnlyList<LivroRelatorio> livros)
    {
        var header1 = "<th>Código</th>";
        var header2 = "<th>Título</th>";
        var header3 = "<th>Editora</th>";
        var header4 = "<th>Edição</th>";
        var header5 = "<th>Ano Publicação</th>";
        var header6 = "<th>Código Autor</th>";
        var header7 = "<th>Nome Autor</th>";
        var header8 = "<th>Código Assunto</th>";
        var header9 = "<th>Assunto Descrição</th>";
        var headers = $"<tr>{header1}{header2}{header3}{header4}{header5}{header6}{header7}{header8}{header9}</tr>";
        var rows = new StringBuilder();
        foreach (var livro in livros)
        {
            var column1 = $"<td>{livro.CodL}</td>";
            var column2 = $"<td>{livro.Titulo}</td>";
            var column3 = $"<td>{livro.Editora}</td>";
            var column4 = $"<td>{livro.Edicao}</td>";
            var column5 = $"<td>{livro.AnoPublicacao}</td>";
            var column6 = $"<td>{livro.CodAu}</td>";
            var column7 = $"<td>{livro.Nome}</td>";
            var column8 = $"<td>{livro.CodAs}</td>";
            var column9 = $"<td>{livro.Descricao}</td>";
            var row = $"<tr>{column1}{column2}{column3}{column4}{column5}{column6}{column7}{column8}{column9}</tr>";
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
