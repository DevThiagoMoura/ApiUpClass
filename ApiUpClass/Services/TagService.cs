using ApiUpClass.Dtos;
using ApiUpClass.Models;
using ApiUpClass.Exceptions;
using ApiUpClass.DataContexts;
using AutoMapper;
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

        public async Task<ICollection<Tag>> FindAll() 
        {
            return await _context.Tags.ToListAsync();
        }

        public async Task<Tag> FindById(int id) 
        {
            var tag = await _context.Tags.FirstOrDefaultAsync(x => x.Id == id);

            if (tag is null) 
            {
                throw new ErrorServiceException(
                    "Tag não encontrada",
                    c => c.NotFound(new {message = $"Tag #{id} não encontrada"})
                    );
            }
            return tag;
        }

        public async Task<Tag> Create(TagDto data) 
        {
            var nomeExiste =await _context.Tags.AnyAsync(x => x.Nome == data.Nome);

            if (nomeExiste) 
            {
                throw new ErrorServiceException(
                    "Tag já cadastrada",
                    c => c.Conflict(new {message = $"A tag {data.Nome} já existe" })
                    );
            }
            var tag = _mapper.Map<Tag>(data);

            await _context.Tags.AddAsync(tag);
            await _context.SaveChangesAsync();

            return tag;
        }

        public async Task<Tag> Update(int id, TagDto data) 
        {
            var tag = await FindById(id);

            var nomeExiste = await _context.Tags.AnyAsync(x => x.Nome == data.Nome && x.Id != id);

            if(nomeExiste) 
            {
                throw new ErrorServiceException(
                    "Tag já cadastrada",
                    c => c.Conflict(new {message = $"A tag {data.Nome} já existe"})
                    );
            }
            _mapper.Map<Tag>(data);

            _context.Tags.Update(tag);
            await _context.SaveChangesAsync();

            return tag;
        }

        public async Task Remove(int id) 
        {
            var tag = await FindById(id);

            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
        }
    }
}