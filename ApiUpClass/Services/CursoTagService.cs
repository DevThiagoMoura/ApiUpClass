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
    public class CursoTagService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CursoTagService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ICollection<CursoTagResponseDto>> FindAll()
        {
            return await _context.CursosTags
                .ProjectTo<CursoTagResponseDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<CursoTagResponseDto> Create(CursoTagDto data)
        {
            var cursoExiste = await _context.Cursos.AnyAsync(x => x.Id == data.CursoId);
            var tagExiste = await _context.Tags.AnyAsync(x => x.Id == data.TagId);

            if (!cursoExiste)
            {
                throw new ErrorServiceException(
                    "Curso nao encontrado",
                    c => c.NotFound(new { message = $"Curso #{data.CursoId} nao encontrado" })
                );
            }

            if (!tagExiste)
            {
                throw new ErrorServiceException(
                    "Tag nao encontrada",
                    c => c.NotFound(new { message = $"Tag #{data.TagId} nao encontrada" })
                );
            }

            var associacaoExiste = await _context.CursosTags
                .AnyAsync(x => x.CursoId == data.CursoId && x.TagId == data.TagId);

            if (associacaoExiste)
            {
                throw new ErrorServiceException(
                    "Associacao ja existente",
                    c => c.Conflict(new { message = "Essa tag ja esta associada ao curso" })
                );
            }

            var cursoTag = _mapper.Map<CursoTag>(data);

            await _context.CursosTags.AddAsync(cursoTag);
            await _context.SaveChangesAsync();

            return _mapper.Map<CursoTagResponseDto>(await FindEntityByIds(data.CursoId, data.TagId));
        }

        private async Task<CursoTag> FindEntityByIds(int cursoId, int tagId)
        {
            var cursoTag = await _context.CursosTags
                .Include(x => x.Curso)
                .Include(x => x.Tag)
                .FirstOrDefaultAsync(x => x.CursoId == cursoId && x.TagId == tagId);

            if (cursoTag is null)
            {
                throw new ErrorServiceException(
                    "Associacao nao encontrada",
                    c => c.NotFound(new { message = "Associacao curso-tag nao encontrada" })
                );
            }

            return cursoTag;
        }

        public async Task Remove(int cursoId, int tagId)
        {
            var cursoTag = await FindEntityByIds(cursoId, tagId);

            _context.CursosTags.Remove(cursoTag);
            await _context.SaveChangesAsync();
        }
    }
}
