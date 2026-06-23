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
    public class TagService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public TagService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ICollection<TagResponseDto>> FindAll()
        {
            return await _context.Tags
                .ProjectTo<TagResponseDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<TagResponseDto> FindById(int id)
        {
            var tag = await FindEntityById(id);

            return _mapper.Map<TagResponseDto>(tag);
        }

        private async Task<Tag> FindEntityById(int id)
        {
            var tag = await _context.Tags.FirstOrDefaultAsync(x => x.Id == id);

            if (tag is null)
            {
                throw new ErrorServiceException(
                    "Tag nao encontrada",
                    c => c.NotFound(new { message = $"Tag #{id} nao encontrada" })
                );
            }

            return tag;
        }

        public async Task<TagResponseDto> Create(TagDto data)
        {
            var nomeExiste = await _context.Tags.AnyAsync(x => x.Nome == data.Nome);

            if (nomeExiste)
            {
                throw new ErrorServiceException(
                    "Tag ja cadastrada",
                    c => c.Conflict(new { message = $"A tag {data.Nome} ja existe" })
                );
            }

            var tag = _mapper.Map<Tag>(data);

            await _context.Tags.AddAsync(tag);
            await _context.SaveChangesAsync();

            return _mapper.Map<TagResponseDto>(tag);
        }

        public async Task<TagResponseDto> Update(int id, TagDto data)
        {
            var tag = await FindEntityById(id);

            var nomeExiste = await _context.Tags.AnyAsync(x => x.Nome == data.Nome && x.Id != id);

            if (nomeExiste)
            {
                throw new ErrorServiceException(
                    "Tag ja cadastrada",
                    c => c.Conflict(new { message = $"A tag {data.Nome} ja existe" })
                );
            }

            _mapper.Map(data, tag);

            _context.Tags.Update(tag);
            await _context.SaveChangesAsync();

            return _mapper.Map<TagResponseDto>(tag);
        }

        public async Task Remove(int id)
        {
            var tag = await FindEntityById(id);

            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
        }
    }
}
