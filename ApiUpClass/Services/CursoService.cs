using ApiUpClass.DataContexts;
using ApiUpClass.Dtos;
using ApiUpClass.Dtos.Responses;
using ApiUpClass.Exceptions;
using ApiUpClass.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace ApiUpClass.Services
{
    public class CursoService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CursoService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ICollection<CursoResponseDto>> FindAll()
        {
            return await _context.Cursos
                .ProjectTo<CursoResponseDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<CursoResponseDto> FindById(int id)
        {
            var curso = await FindEntityById(id);

            return _mapper.Map<CursoResponseDto>(curso);
        }

        public async Task<ICollection<CursoResponseDto>> FindActive()
        {
            return await _context.Cursos
                .Where(c => c.Ativo)
                .ProjectTo<CursoResponseDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<ICollection<CursoResponseDto>> FindByCategoria(int categoriaId)
        {
            var categoriaExiste = await _context.Categorias.AnyAsync(c => c.Id == categoriaId);

            if (!categoriaExiste)
            {
                throw new ErrorServiceException(
                    "Categoria nao encontrada",
                    c => c.NotFound(new { message = $"Categoria #{categoriaId} nao encontrada" })
                );
            }

            return await _context.Cursos
                .Where(c => c.CategoriaId == categoriaId)
                .ProjectTo<CursoResponseDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<ICollection<CursoResponseDto>> FindByTag(int tagId)
        {
            var tagExiste = await _context.Tags.AnyAsync(t => t.Id == tagId);

            if (!tagExiste)
            {
                throw new ErrorServiceException(
                    "Tag nao encontrada",
                    c => c.NotFound(new { message = $"Tag #{tagId} nao encontrada" })
                );
            }

            return await _context.Cursos
                .Where(c => c.CursosTags!.Any(ct => ct.TagId == tagId))
                .ProjectTo<CursoResponseDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<ICollection<ModuloResponseDto>> FindModulos(int id)
        {
            var cursoExiste = await _context.Cursos.AnyAsync(c => c.Id == id);

            if (!cursoExiste)
            {
                throw new ErrorServiceException(
                    "Curso nao encontrado",
                    c => c.NotFound(new { message = $"Curso #{id} nao encontrado" })
                );
            }

            return await _context.Modulos
                .Where(m => m.cursoId == id)
                .OrderBy(m => m.Ordem)
                .ProjectTo<ModuloResponseDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        private async Task<Curso> FindEntityById(int id)
        {
            var curso = await _context.Cursos
                .Include(c => c.Categoria)
                .Include(c => c.CursosTags)!
                    .ThenInclude(ct => ct.Tag)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (curso is null)
            {
                throw new ErrorServiceException(
                    "Curso nao encontrado",
                    c => c.NotFound(new { message = $"Curso #{id} nao encontrado" })
                );
            }

            return curso;
        }

        public async Task<CursoResponseDto> Create(CursoDto data)
        {
            var categoriaExiste = await _context.Categorias
                .AnyAsync(c => c.Id == data.CategoriaId);

            if (!categoriaExiste)
            {
                throw new ErrorServiceException(
                    "Categoria nao encontrada",
                    c => c.NotFound(new { message = $"Categoria #{data.CategoriaId} nao encontrada" })
                );
            }

            var curso = _mapper.Map<Curso>(data);

            await _context.Cursos.AddAsync(curso);
            await _context.SaveChangesAsync();

            return await FindById(curso.Id);
        }

        public async Task<CursoResponseDto> Update(int id, CursoUpdateDto data)
        {
            var curso = await FindEntityById(id);

            var categoriaExiste = await _context.Categorias
                .AnyAsync(c => c.Id == data.CategoriaId);

            if (!categoriaExiste)
            {
                throw new ErrorServiceException(
                    "Categoria nao encontrada",
                    c => c.NotFound(new { message = $"Categoria #{data.CategoriaId} nao encontrada" })
                );
            }

            _mapper.Map(data, curso);

            _context.Cursos.Update(curso);
            await _context.SaveChangesAsync();

            return await FindById(curso.Id);
        }

        public async Task Remove(int id)
        {
            var curso = await FindEntityById(id);

            _context.Cursos.Remove(curso);
            await _context.SaveChangesAsync();
        }

        public async Task<CursoResponseDto> AddTags(int id, CursoTagsDto data)
        {
            var cursoExiste = await _context.Cursos.AnyAsync(c => c.Id == id);

            if (!cursoExiste)
            {
                throw new ErrorServiceException(
                    "Curso nao encontrado",
                    c => c.NotFound(new { message = $"Curso #{id} nao encontrado" })
                );
            }

            var ids = data.Ids.Distinct().ToList();
            var tags = await _context.Tags
                .Where(t => ids.Contains(t.Id))
                .ToListAsync();

            var tagsEncontradas = tags.Select(t => t.Id).ToHashSet();
            var tagsNaoEncontradas = ids.Where(tagId => !tagsEncontradas.Contains(tagId)).ToList();

            if (tagsNaoEncontradas.Count > 0)
            {
                throw new ErrorServiceException(
                    "Tags nao encontradas",
                    c => c.NotFound(new { message = $"Tags nao encontradas: {string.Join(", ", tagsNaoEncontradas)}" })
                );
            }

            var tagsAssociadas = await _context.CursosTags
                .Where(x => x.CursoId == id && ids.Contains(x.TagId))
                .Select(x => x.TagId)
                .ToListAsync();

            foreach (var tagId in ids.Except(tagsAssociadas))
            {
                await _context.CursosTags.AddAsync(new CursoTag
                {
                    CursoId = id,
                    TagId = tagId
                });
            }

            await _context.SaveChangesAsync();

            return await FindById(id);
        }
    }
}
