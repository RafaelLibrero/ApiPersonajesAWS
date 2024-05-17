using ApiPersonajesAWS.Data;
using ApiPersonajesAWS.Models;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System.Data;

namespace ApiPersonajesAWS.Repositories
{
    public class RepositoryPersonajes
    {
        private PersonajesContext context;

        public RepositoryPersonajes(PersonajesContext context)
        {
            this.context = context;
        }

        public async Task<List<Personaje>>
            GetPersonajesAsync()
        {
            return await this.context.Personajes.ToListAsync();
        }

        public async Task<Personaje> FindPersonajeAsync
            (int id)
        {
            return await this.context.Personajes
                .FirstOrDefaultAsync(x => x.IdPersonaje == id);
        }

        private async Task<int> GetMaxIdPersonajeAsync()
        {
            if (this.context.Personajes.Count() == 0)
            {
                return 1;
            }
            else
            {
                return await this.context.Personajes
                    .MaxAsync(x => x.IdPersonaje) + 1;
            }
        }

        public async Task CreatePersonajeAsync
            (string nombre, string imagen)
        {
            Personaje personaje = new Personaje
            {
                IdPersonaje = await this.GetMaxIdPersonajeAsync(),
                Nombre = nombre,
                Imagen = imagen
            };
            this.context.Personajes.Add(personaje);
            await this.context.SaveChangesAsync();
        }

        public async Task UpdatePersonajeAsync
            (Personaje p)
        {
            string sql = "call updatePersonaje (@p_id, @p_nombre, @p_imagen)";
            MySqlParameter pamId = new MySqlParameter("@p_id", p.IdPersonaje);
            MySqlParameter pamNombre = new MySqlParameter("@p_nombre", p.Nombre);
            MySqlParameter pamImagen = new MySqlParameter("@p_imagen", p.Imagen);

            this.context.Database.ExecuteSqlRaw(sql, pamId, pamNombre, pamImagen);
        }
    }
}
