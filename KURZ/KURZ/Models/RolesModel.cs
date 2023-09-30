using KURZ.Interfaces;

namespace KURZ.Models
{
    public class RolesModel : IRolesModel
    {
        //se llama el contexto de la base de datos
        private readonly KurzContext _context;

        //constructor de la clase y recibe como parametro el contexto de la base de datos
        public RolesModel(KurzContext context)
        {
            _context = context;
        }

        public string get_role_name(int id_rol) {
            var role = _context.Roles.FirstOrDefault(e => e.ID_ROL == id_rol);
            string role_name = role.NAME;
            return role_name;
        }


    }
}
