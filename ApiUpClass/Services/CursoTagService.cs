using ApiUpClass.DataContexts;
using ApiUpClass.Dtos;
using ApiUpClass.Exceptions;
using ApiUpClass.Models;
using AutoMapper;
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

        public async Task<ICollection<CursoTag>> FindAll()
        {
            return await _context.CursosTags
                .Include(x => x.Curso)
                .Include(x => x.Tag)
                .ToListAsync();
        }

        public async Task<CursoTag> Create(CursoTagDto data)
        {
            var cursoExiste = await _context.Cursos.AnyAsync(x => x.Id == data.CursoId);
            var tagExiste = await _context.Tags.AnyAsync(x => x.Id == data.TagId);

            if (!cursoExiste)
            {
                throw new ErrorServiceException(
                    "Curso não encontrado",
                    c => c.NotFound(new { message = $"Curso #{data.CursoId} não encontrado" })
                );
            }

            if (!tagExiste)
            {
                throw new ErrorServiceException(
                    "Tag não encontrada",
                    c => c.NotFound(new { message = $"Tag #{data.TagId} não encontrada" })
                );
            }

            var associacaoExiste = await _context.CursosTags
                .AnyAsync(x => x.CursoId == data.CursoId && x.TagId == data.TagId);

            if (associacaoExiste)
            {
                throw new ErrorServiceException(
                    "Associação já existente",
                    c => c.Conflict(new { message = "Essa tag ja está associada ao curso" })
                );
            }

            var cursoTag = _mapper.Map<CursoTag>(data);

            await _context.CursosTags.AddAsync(cursoTag);
            await _context.SaveChangesAsync();

            return cursoTag;
        }

        public async Task Remove(int cursoId, int tagId)
        {
            var cursoTag = await _context.CursosTags
                .FirstOrDefaultAsync(x => x.CursoId == cursoId && x.TagId == tagId);

            if (cursoTag is null)
            {
                throw new ErrorServiceException(
                    "Associação não encontrada",
                    c => c.NotFound(new { message = "Associação curso-tag não encontrada" })
                );
            }

            _context.CursosTags.Remove(cursoTag);
            await _context.SaveChangesAsync();
        }
    }
}