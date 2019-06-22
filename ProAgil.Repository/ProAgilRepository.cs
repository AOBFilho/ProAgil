using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProAgil.Domain;

namespace ProAgil.Repository
{
    public class ProAgilRepository : IProAgilRepository
    {
        private readonly ProAgilContext _context;

        public ProAgilRepository(ProAgilContext context)
        {
            _context = context;
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }
        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }
        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }
        public async Task<Evento[]> GetAllEventoAsync(bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(c => c.Lotes)
                .Include(c => c.RedesSociais);
            
            if (includePalestrantes) 
            {
                query = query
                        .Include(c => c.PalestrantesEventos)
                        .ThenInclude(c => c.Palestrante);     
            }

            query = query.AsNoTracking().OrderByDescending(c => c.DataEvento);
            return await query.ToArrayAsync();
        }

        public async Task<Evento[]> GetAllEventoAsyncByTema(string tema, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(c => c.Lotes)
                .Include(c => c.RedesSociais);
            
            if (includePalestrantes) 
            {
                query = query
                        .Include(c => c.PalestrantesEventos)
                        .ThenInclude(c => c.Palestrante);     
            }

            query = query
                    .AsNoTracking()
                    .Where(c => c.Tema.ToLower().Contains(tema.ToLower()))
                    .OrderByDescending(c => c.DataEvento);
                    
            return await query.ToArrayAsync();
        }
        public async Task<Evento> GetEventoAsyncById(int eventoId, bool includePalestrantes = false)
        {
           IQueryable<Evento> query = _context.Eventos
                .Include(c => c.Lotes)
                .Include(c => c.RedesSociais);
            
            if (includePalestrantes) 
            {
                query = query
                        .Include(c => c.PalestrantesEventos)
                        .ThenInclude(c => c.Palestrante);     
            }

            query = query
                    .AsNoTracking()
                    .Where(c => c.Id == eventoId)
                    .OrderByDescending(c => c.DataEvento);
                    
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Palestrante[]> GetAllPalestrantesAsyncByName(string name, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include(c => c.RedesSociais);

            if (includeEventos)
            {
                query = query
                    .Include(c => c.PalestrantesEventos)
                    .ThenInclude(c => c.Evento);
            }

            query = query
                .AsNoTracking()
                .Include(c => c.Nome.ToLower().Contains(name.ToLower()));
            
            return await query.ToArrayAsync();   
        }
        public async Task<Palestrante> GetPalestranteAsyncById(int palestranteId, bool includeEventos = false)
        {
             IQueryable<Palestrante> query = _context.Palestrantes
                .Include(c => c.RedesSociais);
                
            if (includeEventos)
            {
                query = query
                    .Include(c => c.PalestrantesEventos)
                    .ThenInclude(c => c.Evento);
            }

            query = query
                .AsNoTracking()
                .Include(c => c.Id == palestranteId);
            
            return await query.FirstOrDefaultAsync(); 
        }

       
    }
}