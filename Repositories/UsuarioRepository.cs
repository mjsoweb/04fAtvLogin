using Microsoft.EntityFrameworkCore;

public Exo.WebApi.Controllers.Usuario Login(string email, string senha)
{
    return DbContext.Usuarios.FirstOrDefault(u => u.Email == email && u.Senha == senha);
}
